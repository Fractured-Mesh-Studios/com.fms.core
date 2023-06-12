using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Debug = DebugEngine.Debug;


namespace GameEngine
{
    public class InputProvider : MonoBehaviour
    {
        public enum InputProviderMode
        {
            None,
            SendMessages,
            BroadcastMessages,
        }

        [Header("Input")]
        public bool DisableOverInterface = true;
        public InputActionAsset Asset;
        public InputProviderMode Mode;
        public UpdateMethod UpdateMethod = UpdateMethod.Update;

        [HideInInspector] public string ActionMapName = string.Empty;
        [HideInInspector] public bool[] MapKeys;
        [SerializeField] private bool EnableDebug;

        private InputActionMap MapReference;
        private InputActionRebindingExtensions.RebindingOperation Operation;
        private bool OverInterface;

        private void OnEnable()
        {
            //MapReference = Asset.FindActionMap(ActionMapName);
            GenerateKeys();

            if (MapReference != null)
            {
                InputAction Action = null;
                for (int i = 0; i < MapReference.actions.Count; i++)
                {
                    Action = MapReference.actions[i];
                    if (MapKeys[i])
                    {
                        Debug.Log(("Action Added " + Action.name).Color(Color.green), gameObject, EnableDebug);
                        Action.performed += AddMessage;
                        Action.Enable();
                    }
                }
            }
        }

        private void OnDisable()
        {
            if (ActionMapName == string.Empty) return;

            MapReference = Asset.FindActionMap(ActionMapName);

            if (MapReference != null)
            {
                InputAction Action = null;
                for (int i = 0; i < MapReference.actions.Count; i++)
                {
                    Action = MapReference.actions[i];
                    if (Action.enabled)
                    {
                        Debug.Log(("Action Removed " + Action.name).Color(Color.red), gameObject, EnableDebug);
                        Action.performed -= AddMessage;
                        Action.Disable();
                    }
                }
            }

            MapReference = null;
        }

        #region Update
        private void Update()
        {
            if(UpdateMethod == UpdateMethod.Update && EventSystem.current)
            {
                OverInterface = EventSystem.current.IsPointerOverGameObject();
            }
        }

        private void FixedUpdate()
        {
            if (UpdateMethod == UpdateMethod.FixedUpdate && EventSystem.current)
            {
                OverInterface = EventSystem.current.IsPointerOverGameObject();
            }
        }

        private void LateUpdate()
        {
            if (UpdateMethod == UpdateMethod.LateUpdate && EventSystem.current)
            {
                OverInterface = EventSystem.current.IsPointerOverGameObject();
            }
        }
        #endregion

        private void AddMessage(InputAction.CallbackContext context)
        {
            if (DisableOverInterface && OverInterface) { return; }

            switch (Mode)
            {
                case InputProviderMode.SendMessages: 
                    SendMessage("On" + context.action.name, context); 
                    break;
                case InputProviderMode.BroadcastMessages: 
                    BroadcastMessage("On" + context.action.name, context); 
                    break;
            }
        }

        public void GenerateKeys()
        {
            int Keys = 0;
            int MapReferenceLength = 0;
            if (MapReference == null)
            {
                MapReference = Asset.FindActionMap(ActionMapName);
                MapReferenceLength = MapReference.actions.Count;
            }

            if (MapKeys == null || MapKeys.Length == 0 || MapKeys.Length != MapReferenceLength)
            {
                MapKeys = new bool[MapReferenceLength];
                for (int i = 0; i < MapKeys.Length; i++)
                {
                    MapKeys[i] = true;
                    Keys++;
                }

                Debug.Log("Generated " + Keys + " Keys", gameObject, EnableDebug);
            }
        }

        #region Rebinding

        public bool Rebind(string ActionName, System.Action OnComplete)
        {
            return Rebind(ActionName, OnComplete, "Mouse");
        }

        public bool Rebind(string ActionName, System.Action OnComplete, string Exclude)
        {
            if(Operation != null)
            {
                Debug.LogError("Current Rebinding Operation Running <"+Operation.action.name+">", gameObject, EnableDebug);
                return false;
            }

            InputAction Action = MapReference.FindAction(ActionName);
            Operation = Action.PerformInteractiveRebinding()
                .WithControlsExcluding(Exclude)
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(op => { 
                    RebindCompleted(); 
                    if(OnComplete != null) 
                        OnComplete.Invoke(); 
                })
                .Start();

            return Operation.started;
        }

        private void RebindCompleted()
        {
            Operation.Dispose();
            Operation = null;

            Debug.Log("Rebind Completed", gameObject, EnableDebug);
        }

        #endregion
    }
}
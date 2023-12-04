using System;
using System.Reflection;
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
        public bool overInterface = true;
        public InputActionAsset asset;
        public InputProviderMode mode;
        public UpdateMethod updateMethod = UpdateMethod.Update;

        [HideInInspector] public string actionMapName = string.Empty;
        [HideInInspector] public bool[] mapKeys;
        [SerializeField] private bool m_enableDebug;

        private InputActionMap m_map;
        private InputActionRebindingExtensions.RebindingOperation m_operation;
        private bool m_overInterface;

        private void OnEnable()
        {
            //m_map = asset.FindActionMap(actionMapName);
            GenerateKeys();

            if (m_map != null)
            {
                InputAction Action = null;
                for (int i = 0; i < m_map.actions.Count; i++)
                {
                    Action = m_map.actions[i];
                    if (mapKeys[i])
                    {
                        Debug.Log(("Action Added " + Action.name).Color(Color.green), gameObject, m_enableDebug);
                        Action.performed += AddMessage;
                        Action.Enable();
                    }
                }
            }
        }

        private void OnDisable()
        {
            if (actionMapName == string.Empty) return;

            m_map = asset.FindActionMap(actionMapName);

            if (m_map != null)
            {
                InputAction Action = null;
                for (int i = 0; i < m_map.actions.Count; i++)
                {
                    Action = m_map.actions[i];
                    if (Action.enabled)
                    {
                        Debug.Log(("Action Removed " + Action.name).Color(Color.red), gameObject, m_enableDebug);
                        Action.performed -= AddMessage;
                        Action.Disable();
                    }
                }
            }

            m_map = null;
        }

        #region Update
        private void Update()
        {
            if(updateMethod == UpdateMethod.Update && EventSystem.current)
            {
                m_overInterface = EventSystem.current.IsPointerOverGameObject();
            }
        }

        private void FixedUpdate()
        {
            if (updateMethod == UpdateMethod.FixedUpdate && EventSystem.current)
            {
                m_overInterface = EventSystem.current.IsPointerOverGameObject();
            }
        }

        private void LateUpdate()
        {
            if (updateMethod == UpdateMethod.LateUpdate && EventSystem.current)
            {
                m_overInterface = EventSystem.current.IsPointerOverGameObject();
            }
        }
        #endregion

        internal InputValue m_inputValue;

        private void AddMessage(InputAction.CallbackContext context)
        {
            if (!overInterface && m_overInterface) { return; }

            if (m_inputValue == null)
                m_inputValue = new InputValue();

            Type type = m_inputValue.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo field = type.GetField("m_Context", flags);
            field.SetValue(m_inputValue, context);

            switch (mode)
            {
                case InputProviderMode.SendMessages:
                    SendMessage("On" + context.action.name, m_inputValue, SendMessageOptions.DontRequireReceiver);
                    break;
                case InputProviderMode.BroadcastMessages:
                    BroadcastMessage("On" + context.action.name, m_inputValue, SendMessageOptions.DontRequireReceiver);
                    break;
            }

            m_inputValue = null;
        }

        public void GenerateKeys()
        {
            int Keys = 0;
            int m_mapLength = 0;
            if (m_map == null)
            {
                m_map = asset.FindActionMap(actionMapName);
                m_mapLength = m_map.actions.Count;
            }

            if (mapKeys == null || mapKeys.Length == 0 || mapKeys.Length != m_mapLength)
            {
                mapKeys = new bool[m_mapLength];
                for (int i = 0; i < mapKeys.Length; i++)
                {
                    mapKeys[i] = true;
                    Keys++;
                }

                Debug.Log("Generated " + Keys + " Keys", gameObject, m_enableDebug);
            }
        }

        #region Rebinding

        public bool Rebind(string ActionName, System.Action OnComplete)
        {
            return Rebind(ActionName, OnComplete, "Mouse");
        }

        public bool Rebind(string ActionName, System.Action OnComplete, string Exclude)
        {
            if(m_operation != null)
            {
                Debug.LogError("Current Rebinding Operation Running <"+m_operation.action.name+">", gameObject, m_enableDebug);
                return false;
            }

            InputAction Action = m_map.FindAction(ActionName);
            m_operation = Action.PerformInteractiveRebinding()
                .WithControlsExcluding(Exclude)
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(op => { 
                    RebindCompleted(); 
                    if(OnComplete != null) 
                        OnComplete.Invoke(); 
                })
                .Start();

            return m_operation.started;
        }

        private void RebindCompleted()
        {
            m_operation.Dispose();
            m_operation = null;

            Debug.Log("Rebind Completed", gameObject, m_enableDebug);
        }

        #endregion
    }
}
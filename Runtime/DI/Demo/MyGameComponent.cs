// Game/PlayerController.cs
using UnityEngine;
using CoreEngine.DI;


namespace CoreEngine.DI.Demo
{
    public class MyGameComponent : MonoBehaviour
    {
        // Inyección de campo
        [Inject] 
        private IMyGameService _gameService;

        void Start()
        {
            if (_gameService != null)
            {
                _gameService.LogMessage("PlayerController iniciado y el servicio fue inyectado en el campo.");
                Debug.Log("Número aleatorio del servicio: " + _gameService.GetRandomNumber());
            }
            else
            {
                Debug.LogError("PlayerController: IMyGameService no se inyectó correctamente en el campo.");
            }
        }

        // Inyección de método
        [Inject]
        private void SetUpPlayer(IMyGameService serviceViaMethod)
        {
            if (serviceViaMethod != null)
            {
                serviceViaMethod.LogMessage("PlayerController: SetUpPlayer llamado y el servicio fue inyectado en el método.");
            }
            else
            {
                Debug.LogError("PlayerController: IMyGameService no se inyectó correctamente en el método.");
            }
        }
    }
}
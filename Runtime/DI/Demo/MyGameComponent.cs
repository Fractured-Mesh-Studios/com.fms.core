// Game/PlayerController.cs
using UnityEngine;
using CoreEngine.DI;


namespace CoreEngine.DI.Demo
{
    public class MyGameComponent : MonoBehaviour
    {
        // Inyecci�n de campo
        [Inject] 
        private IMyGameService _gameService;

        void Start()
        {
            if (_gameService != null)
            {
                _gameService.LogMessage("PlayerController iniciado y el servicio fue inyectado en el campo.");
                Debug.Log("N�mero aleatorio del servicio: " + _gameService.GetRandomNumber());
            }
            else
            {
                Debug.LogError("PlayerController: IMyGameService no se inyect� correctamente en el campo.");
            }
        }

        // Inyecci�n de m�todo
        [Inject]
        private void SetUpPlayer(IMyGameService serviceViaMethod)
        {
            if (serviceViaMethod != null)
            {
                serviceViaMethod.LogMessage("PlayerController: SetUpPlayer llamado y el servicio fue inyectado en el m�todo.");
            }
            else
            {
                Debug.LogError("PlayerController: IMyGameService no se inyect� correctamente en el m�todo.");
            }
        }
    }
}
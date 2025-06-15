// Services/IMyGameService.cs
//using System;
using UnityEngine;

namespace CoreEngine.DI.Demo
{
    public interface IMyGameService
    {
        void LogMessage(string message);
        int GetRandomNumber();
    }

    public class MyGameService : MonoBehaviour, IMyGameService
    {
        public void LogMessage(string message)
        {
            Debug.Log($"[MyGameService] {message}");
        }

        public int GetRandomNumber()
        {
            return Random.Range(0, 100);
        }
    }
}

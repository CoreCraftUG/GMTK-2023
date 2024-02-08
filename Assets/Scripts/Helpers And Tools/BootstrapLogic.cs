using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using HeathenEngineering.SteamworksIntegration;
using AppClient = HeathenEngineering.SteamworksIntegration.API.App.Client;

namespace CoreCraft.Core
{
    public class BootstrapLogic : MonoBehaviour
    {
        public UnityEvent<float> BootTimerEvent = new UnityEvent<float>();
        public UnityEvent BootStrapCompleteEvent = new UnityEvent();

        private void Start()
        {
            BootTimerEvent.AddListener((float i) =>
            {
                Debug.Log($"Loading complete in {i}");
            });

            StartCoroutine(Validate());
        }


        private IEnumerator Validate()
        {
            // Visualize Timer
            BootTimerEvent.Invoke(4f);
            yield return new WaitForSeconds(1f);

            yield return new WaitUntil(() =>
            {
                return EventManager.Instance != null && SteamAchievementHelper.Instance != null;
            });
            Debug.Log($"EventManager is ready!");
            SteamAchievementHelper.Instance.EventManagerReady = true;

            // Visualize Timer
            BootTimerEvent.Invoke(3f);
            yield return new WaitForSeconds(1f);

            // Visualize Timer
            BootTimerEvent.Invoke(2f);
            yield return new WaitForSeconds(1f);

            // Initialize Steam
            yield return new WaitUntil(() => SteamSettings.Initialized);
            Debug.Log($"Steam API is initialized as App {AppClient.BuildId} starting Scene Load!");

            // Visualize Timer
            BootTimerEvent.Invoke(1f);
            yield return new WaitForSeconds(1f);

            BootStrapCompleteEvent.Invoke();
        }
    }
}
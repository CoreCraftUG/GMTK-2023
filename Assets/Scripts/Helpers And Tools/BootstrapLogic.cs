using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using HeathenEngineering.SteamworksIntegration;
using JamCraft.GMTK2023.Code;
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
                return EventManager.Instance != null && SteamAchievementHelper.Instance != null && SteamInitializationErrorHandler.Instance != null && GameInputManager.Instance != null && GameSettingsFile.Instance != null && GameSettingsManager.Instance != null;
            });
            Debug.Log($"EventManager is ready!");

            // Visualize Timer
            BootTimerEvent.Invoke(3f);
            yield return new WaitForSeconds(1f);

            // Visualize Timer
            BootTimerEvent.Invoke(2f);
            yield return new WaitForSeconds(1f);

            // Initialize Steam
            yield return new WaitUntil(() => SteamSettings.Initialized || SteamSettings.HasInitializationError);

            if (SteamSettings.Initialized)
            {
                SteamAchievementHelper.Instance.SteamOnline = true;
                SteamStatManager.Instance.SteamOnline = true;
            }
            else if (SteamSettings.HasInitializationError)
            {
                SteamAchievementHelper.Instance.SteamOnline = false;
                SteamStatManager.Instance.SteamOnline = false;

                yield return StartCoroutine(SteamInitializationErrorHandler.Instance.StartHasInitializationError());
            }

            SteamAchievementHelper.Instance.EventManagerReady = true;
            SteamStatManager.Instance.EventManagerReady = true;
            SoundManager.Instance.EventManagerReady = true;

            // Visualize Timer
            BootTimerEvent.Invoke(1f);
            yield return new WaitForSeconds(1f);

            BootStrapCompleteEvent.Invoke();
        }
    }
}
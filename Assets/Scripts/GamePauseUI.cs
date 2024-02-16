using System;
using UnityEngine;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class GamePauseUI : MonoBehaviour
    {
        public static GamePauseUI Instance { get; private set; }

        [Header("UI Buttons")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _mainMenuButton;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            // Unpause the game and close the pause menu.
            _resumeButton.onClick.AddListener(() =>
            {
                GameStateManager.Instance.TogglePauseGame();
            });

            // Show the options menu and hide the pause menu.
            _optionsButton.onClick.AddListener(() =>
            {
                GameOptionsUI.Instance.Show();
                Hide();
            });

            // Got to main menu.
            _mainMenuButton.onClick.AddListener(() =>
            {
                Loader.Load("mainmenu_scene");
            });
        }

        private void Start()
        {
            // Subscribe to the events.
            GameStateManager.Instance.OnGamePaused += GameStateManager_OnOnGamePaused;
            GameStateManager.Instance.OnGameUnpaused += GameStateManager_OnOnGameUnpaused;

            Hide();
        }

        // If the game unpauses, hide the pause menu.
        private void GameStateManager_OnOnGameUnpaused(object sender, EventArgs e)
        {
            Hide();
        }

        // If the game pauses, show the pause menu.
        private void GameStateManager_OnOnGamePaused(object sender, EventArgs e)
        {
            Show();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                // Unsubscribe from events in case of destruction.
                GameStateManager.Instance.OnGamePaused -= GameStateManager_OnOnGamePaused;
                GameStateManager.Instance.OnGameUnpaused -= GameStateManager_OnOnGameUnpaused;

                _resumeButton.onClick.RemoveAllListeners();
                _optionsButton.onClick.RemoveAllListeners();
                _mainMenuButton.onClick.RemoveAllListeners();
            }
        }

        private void OnApplicationQuit()
        {
            if (EventManager.Instance != null)
            {
                // Unsubscribe from events in case of destruction.
                GameStateManager.Instance.OnGamePaused -= GameStateManager_OnOnGamePaused;
                GameStateManager.Instance.OnGameUnpaused -= GameStateManager_OnOnGameUnpaused;

                _resumeButton.onClick.RemoveAllListeners();
                _optionsButton.onClick.RemoveAllListeners();
                _mainMenuButton.onClick.RemoveAllListeners();
            }
        }
    }
}
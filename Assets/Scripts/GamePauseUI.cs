using System;
using DG.Tweening;
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

            _resumeButton.onClick.AddListener(() =>
            {
                GameStateManager.Instance.TogglePauseGame();
            });

            _optionsButton.onClick.AddListener(() =>
            {
                GameOptionsUI.Instance.Show();
                Hide();
            });

            _mainMenuButton.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.mainmenu_scene);
            });
        }

        private void Start()
        {
            GameStateManager.Instance.OnGamePaused += GameStateManager_OnOnGamePaused;
            GameStateManager.Instance.OnGameUnpaused += GameStateManager_OnOnGameUnpaused;

            Hide();
        }

        private void GameStateManager_OnOnGameUnpaused(object sender, EventArgs e)
        {
            Hide();
        }

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
            GameStateManager.Instance.OnGamePaused -= GameStateManager_OnOnGamePaused;
            GameStateManager.Instance.OnGameUnpaused -= GameStateManager_OnOnGameUnpaused;
        }
    }
}
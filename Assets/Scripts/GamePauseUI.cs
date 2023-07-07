using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class GamePauseUI : MonoBehaviour
    {
        [Header("UI Buttons")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _mainMenuButton;

        private void Awake()
        {
            _resumeButton.onClick.AddListener(() =>
            {
                GameStateManager.Instance.TogglePauseGame();
            });

            _optionsButton.onClick.AddListener(() =>
            {

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


        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
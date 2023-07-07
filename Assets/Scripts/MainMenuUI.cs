using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI Buttons")] 
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _coreCraftButton;

        [Header("Quit Panel")]
        [SerializeField] private GameObject _quitPanel;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private void Awake()
        {
            SetupUIButtons();

            Time.timeScale = 1f;
        }

        private void Start()
        {
            _quitPanel.SetActive(false);
        }

        private void SetupUIButtons()
        {
            // Load the game scene.
            _playButton.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.game_scene);
            });

            // Show quit panel on click.
            _quitButton.onClick.AddListener(() =>
            {
                _quitPanel.SetActive(true);
            });

            // TODO: Link to our website?
            _coreCraftButton.onClick.AddListener(() =>
            {
                
            });

            // Close the program.
            _yesButton.onClick.AddListener(() =>
            {
                // TODO: Add Game Jam page. Application.OpenURL("url").
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });

            // Close the quit panel.
            _noButton.onClick.AddListener(() =>
            {
                _quitPanel.SetActive(false);
            });
        }
    }
}
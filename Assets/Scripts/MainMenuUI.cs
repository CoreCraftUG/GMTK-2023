using UnityEngine;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class MainMenuUI : MonoBehaviour
    {
        public static MainMenuUI Instance { get; private set; }

        [Header("UI Buttons")] 
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _coreCraftButton;

        [Header("Quit Panel")]
        [SerializeField] private GameObject _quitPanel;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            SetupUIButtons();

            Time.timeScale = 1f;
        }

        private void Start()
        {
            _quitPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            _optionsButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
            _coreCraftButton.onClick.RemoveAllListeners();
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }

        private void OnApplicationQuit()
        {
            _playButton.onClick.RemoveAllListeners();
            _optionsButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
            _coreCraftButton.onClick.RemoveAllListeners();
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }

        private void SetupUIButtons()
        {
            // Load the game scene.
            _playButton.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.game_scene);
            });

            // Show the options menu and hide the pause menu.
            _optionsButton.onClick.AddListener(() =>
            {
                GameOptionsUI.Instance.Show();
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
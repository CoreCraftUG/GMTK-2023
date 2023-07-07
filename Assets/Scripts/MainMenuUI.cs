using UnityEngine;
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
        }

        private void Start()
        {
            _quitPanel.SetActive(false);
        }

        private void SetupUIButtons()
        {
            _playButton.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.game_scene);
            });

            _quitButton.onClick.AddListener(() =>
            {
                _quitPanel.SetActive(true);
            });

            _coreCraftButton.onClick.AddListener(() =>
            {
                Application.OpenURL("https://www.corecraft-games.com");
            });

            _yesButton.onClick.AddListener(() =>
            {
                // TODO: Add Game Jam page. Application.OpenURL("url").
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });

            _noButton.onClick.AddListener(() =>
            {
                _quitPanel.SetActive(false);
            });
        }
    }
}
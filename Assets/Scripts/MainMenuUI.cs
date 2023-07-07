using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI Buttons")] 
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;

        private void Awake()
        {
            SetupUIButtons();
        }

        private void SetupUIButtons()
        {
            _playButton.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.game_scene);
            });
            _quitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}
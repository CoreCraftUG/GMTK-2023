using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class GameOverUI : MonoBehaviour
    {
        public static GameOverUI Instance { get; private set; }

        [Header("UI Buttons")] 
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _mainMenuButton;

        [Header("UI Texts")]
        [SerializeField] private TextMeshProUGUI _finalScoreText;
        [SerializeField] private TextMeshProUGUI _finalLevelText;

        private int _level;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;
            
            _retryButton.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.game_scene);
            });

            _mainMenuButton.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.mainmenu_scene);
            });
        }

        private void Start()
        {
            EventManager.Instance.GameOverEvent.AddListener(() =>
            {
                GameStateManager.Instance.IsGameOver = true;
                // TODO: Fix the score system.

                _finalScoreText.text = "Your final score is: " + ScoreUI.Instance.NewScore;
                _finalLevelText.text = "You've reached level: " + ScoreUI.Instance.Level;
                Show();
            });

            Hide();
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                _retryButton.onClick.RemoveAllListeners();
                _mainMenuButton.onClick.RemoveAllListeners();
                EventManager.Instance.GameOverEvent.RemoveAllListeners();
            }
        }

        private void OnApplicationQuit()
        {
            if (EventManager.Instance != null)
            {
                _retryButton.onClick.RemoveAllListeners();
                _mainMenuButton.onClick.RemoveAllListeners();
                EventManager.Instance.GameOverEvent.RemoveAllListeners();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
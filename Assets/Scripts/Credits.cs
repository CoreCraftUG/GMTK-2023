using UnityEngine;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class Credits : MonoBehaviour
    {
        [SerializeField] private Button _backButton;

        public static Credits Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            _backButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                _backButton.onClick.RemoveAllListeners();
            }
        }

        private void OnApplicationQuit()
        {
            if (EventManager.Instance != null)
            {
                _backButton.onClick.RemoveAllListeners();
            }
        }

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            MainMenuUI.Instance.Show();
        }
    }
}
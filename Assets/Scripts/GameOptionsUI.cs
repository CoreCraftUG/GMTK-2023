using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class GameOptionsUI : MonoBehaviour
    {
        public static GameOptionsUI Instance { get; private set; }

        [Header("UI Buttons")]
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _backButton;

        [Header("UI Dropdowns")]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;

        [Header("UI Slider")] 
        [SerializeField] private Slider _mainVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            _mainVolumeSlider.onValueChanged.AddListener(value =>
            {
                SoundManager.Instance.ChangeVolume(value);
                Debug.Log(value);
                Debug.Log(SoundManager.Instance._volume);
            });

            _musicVolumeSlider.onValueChanged.AddListener(value =>
            {
                SoundManager.Instance.ChangeVolume(value);
                Debug.Log(value);
                Debug.Log(SoundManager.Instance._volume);
            });

            _sfxVolumeSlider.onValueChanged.AddListener(value =>
            {
                SoundManager.Instance.ChangeVolume(value);
                Debug.Log(value);
                Debug.Log(SoundManager.Instance._volume);
            });

            _saveButton.onClick.AddListener(() =>
            {

            });

            _backButton.onClick.AddListener(() =>
            {
                GamePauseUI.Instance.Show();
                Hide();
            });
        }

        private void Start()
        {
            GameStateManager.Instance.OnGameUnpaused += GameStateManager_OnOnGameUnpaused;

            Hide();
        }

        private void GameStateManager_OnOnGameUnpaused(object sender, EventArgs e)
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
        }
    }
}
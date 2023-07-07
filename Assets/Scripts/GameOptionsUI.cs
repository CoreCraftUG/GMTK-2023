using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class GameOptionsUI : MonoBehaviour
    {
        public const string PLAYER_PREFS_MAIN_VOLUME = "MainVolume";
        public const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
        public const string PLAYER_PREFS_SFX_VOLUME = "SfxVolume";

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

        [Header("UI Texts")] 
        [SerializeField] private TextMeshProUGUI _mainVolumeText;
        [SerializeField] private TextMeshProUGUI _musicVolumeText;
        [SerializeField] private TextMeshProUGUI _sfxVolumeText;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            // Add functions to the sliders.
            _mainVolumeSlider.onValueChanged.AddListener(OnMainVolumeValueChanged);

            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeValueChanged);

            _sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeValueChanged);

            // Save sound values.
            _saveButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetFloat(PLAYER_PREFS_MAIN_VOLUME, SoundManager.Instance.MainVolume);
                PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, SoundManager.Instance.MusicVolume);
                PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME, SoundManager.Instance.SfxVolume);
                PlayerPrefs.Save();
            });

            // Close options menu and show pause menu.
            _backButton.onClick.AddListener(() =>
            {
                GamePauseUI.Instance.Show();
                Hide();
            });
        }

        // Change the volume to the slider value and set the text accordingly.
        private void OnSfxVolumeValueChanged(float value)
        {
            SoundManager.Instance.ChangeSfxVolume(_sfxVolumeSlider.value);
            _sfxVolumeText.text = Mathf.Round(SoundManager.Instance.SfxVolume * 10).ToString();
        }

        // Change the volume to the slider value and set the text accordingly.

        private void OnMusicVolumeValueChanged(float value)
        {
            SoundManager.Instance.ChangeMusicVolume(_musicVolumeSlider.value);
            _musicVolumeText.text = Mathf.Round(SoundManager.Instance.MusicVolume * 10).ToString();
        }

        // Change the volume to the slider value and set the text accordingly.

        private void OnMainVolumeValueChanged(float value)
        {
            SoundManager.Instance.ChangeMainVolume(_mainVolumeSlider.value);
            _mainVolumeText.text = Mathf.Round(SoundManager.Instance.MainVolume * 10).ToString();
        }

        private void Start()
        {
            // Subscribe to the unpause event.
            GameStateManager.Instance.OnGameUnpaused += GameStateManager_OnOnGameUnpaused;

            // Set the texts to the according values + some black magic so it looks correct.
            _mainVolumeText.text = Mathf.Round(SoundManager.Instance.MainVolume * 10).ToString();

            _musicVolumeText.text = Mathf.Round(SoundManager.Instance.MusicVolume * 10).ToString();

            _sfxVolumeText.text = Mathf.Round(SoundManager.Instance.SfxVolume * 10).ToString();

            // Set the slider values to the sound manager values.
            _mainVolumeSlider.value = SoundManager.Instance.MainVolume;

            _musicVolumeSlider.value = SoundManager.Instance.MusicVolume;

            _sfxVolumeSlider.value = SoundManager.Instance.SfxVolume;

            Hide();
        }

        // If the game unpauses, hide the options menu.
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
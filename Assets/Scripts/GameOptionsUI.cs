using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputSettings;

namespace JamCraft.GMTK2023.Code
{
    public class GameOptionsUI : MonoBehaviour
    {
        public const string PLAYER_PREFS_MAIN_VOLUME = "MainVolume";
        public const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
        public const string PLAYER_PREFS_SFX_VOLUME = "SfxVolume";
        public const string PLAYER_PREFS_CAMERA_HEIGHT = "CameraHeightValue";

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
        [SerializeField] private Slider _cameraHeightSlider;

        [Header("UI Texts")] 
        [SerializeField] private TextMeshProUGUI _mainVolumeText;
        [SerializeField] private TextMeshProUGUI _musicVolumeText;
        [SerializeField] private TextMeshProUGUI _sfxVolumeText;
        [SerializeField] private TextMeshProUGUI _cameraHeightText;

        private List<Resolution> _supportedResolutions;


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

            _cameraHeightSlider.onValueChanged.AddListener(OnCameraHeightValueChanged);

            // Save sound values.
            _saveButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetFloat(PLAYER_PREFS_MAIN_VOLUME, SoundManager.Instance.MainVolume);
                PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, SoundManager.Instance.MusicVolume);
                PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME, SoundManager.Instance.SfxVolume);
                PlayerPrefs.SetFloat(PLAYER_PREFS_CAMERA_HEIGHT, GameSettingsManager.Instance.CameraHeight);
                PlayerPrefs.Save();
            });

            // Close options menu and show pause menu.
            _backButton.onClick.AddListener(() =>
            {
                if (GamePauseUI.Instance != null)
                {
                    GamePauseUI.Instance.Show();
                }
                
                Hide();
            });

            // Add function to the resolution dropdown.
            _resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        private void OnCameraHeightValueChanged(float value)
        {
            GameSettingsManager.Instance.ChangeCameraHeight(_cameraHeightSlider.value);
            _cameraHeightText.text = Mathf.Round(GameSettingsManager.Instance.CameraHeight * 10).ToString();
        }

        private void SetResolution(int index)
        {
            Screen.SetResolution(_supportedResolutions[index].width, _supportedResolutions[index].height, FullScreenMode.ExclusiveFullScreen);
            _resolutionDropdown.value = index;
            _resolutionDropdown.RefreshShownValue();
            
            // TODO: Save to playerprefs.
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

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnGameUnpaused += GameStateManager_OnOnGameUnpaused;
            }

            // Set the texts to the according values + some black magic so it looks correct.
            _mainVolumeText.text = Mathf.Round(SoundManager.Instance.MainVolume * 10).ToString();

            _musicVolumeText.text = Mathf.Round(SoundManager.Instance.MusicVolume * 10).ToString();

            _sfxVolumeText.text = Mathf.Round(SoundManager.Instance.SfxVolume * 10).ToString();

            _cameraHeightText.text = Mathf.Round(GameSettingsManager.Instance.CameraHeight * 10).ToString();

            // Set the slider values to the sound manager values.
            _mainVolumeSlider.value = SoundManager.Instance.MainVolume;

            _musicVolumeSlider.value = SoundManager.Instance.MusicVolume;

            _sfxVolumeSlider.value = SoundManager.Instance.SfxVolume;

            _cameraHeightSlider.value = GameSettingsManager.Instance.CameraHeight;

            // Fill the dropdown with the supported resolutions.
            AddResolutions();

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

        private void AddResolutions()
        {
            _supportedResolutions = new List<Resolution>();
            Resolution[] resolutions = Screen.resolutions;

            Array.Reverse(resolutions);

            for (int i = 0; i < resolutions.Length; i++)
            {
                _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(ResolutionToString(resolutions[i])));
                _supportedResolutions.Add(resolutions[i]);
            }

            // TODO: Set dropdown value to the saved one.
            //_resolutionDropdown.value = 
            _resolutionDropdown.RefreshShownValue();
        }

        private string ResolutionToString(Resolution res)
        {
            return res.width + " x " + res.height + " @ " + res.refreshRate + " Hz";
        }
    }
}
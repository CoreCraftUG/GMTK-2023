using Cinemachine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public const string PLAYER_PREFS_RESOLUTION = "ResolutionValue";

        public static GameOptionsUI Instance { get; private set; }

        [Header("UI Buttons")]
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _resetToDefaultButton;
        [SerializeField] private Button _backButton;

        [Header("Options Categories Buttons")]
        [SerializeField] private Button _graphicsButton;
        [SerializeField] private Button _soundsButton;
        [SerializeField] private Button _controlsButton;
        [SerializeField] private Button _accessibilityButton;

        [Header("Options Categories Panels")] 
        [SerializeField] private GameObject _graphicsPanel;
        [SerializeField] private GameObject _soundsPanel;
        [SerializeField] private GameObject _controlsPanel;
        [SerializeField] private GameObject _accessibilityPanel;

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

        [Header("Keybindings")] 
        [SerializeField] private GameObject _rebindPanel;
        [SerializeField] private TextMeshProUGUI _turnTableRightKeybindingText1;
        [SerializeField] private TextMeshProUGUI _turnTableRightKeybindingText2;
        [SerializeField] private Button _turnTableRightKeybindingButton1;
        [SerializeField] private Button _turnTableRightKeybindingButton2;
        [SerializeField] private TextMeshProUGUI _turnTableLeftKeybindingText1;
        [SerializeField] private TextMeshProUGUI _turnTableLeftKeybindingText2;
        [SerializeField] private Button _turnTableLeftKeybindingButton1;
        [SerializeField] private Button _turnTableLeftKeybindingButton2;
        [SerializeField] private TextMeshProUGUI _placeCardKeybindingText1;
        [SerializeField] private TextMeshProUGUI _placeCardKeybindingText2;
        [SerializeField] private Button _placeCardKeybindingButton1;
        [SerializeField] private Button _placeCardKeybindingButton2;

        private List<Resolution> _supportedResolutions;

        [Space]
        public Transform OptionsCameraFocus;

        [SerializeField] private CinemachineVirtualCamera _uiCamera;

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
                PlayerPrefs.SetInt(PLAYER_PREFS_RESOLUTION, GameSettingsManager.Instance.ResolutionIndex);
                PlayerPrefs.Save();
            });

            // Close options menu and show pause menu.
            _backButton.onClick.AddListener(() =>
            {
                if (GamePauseUI.Instance != null)
                {
                    GamePauseUI.Instance.Show();
                }

                if (MainMenuUI.Instance != null)
                {
                    MainMenuUI.Instance.Show();
                    _uiCamera.Follow = MainMenuUI.Instance.MainMenuCenterTransform;
                }
                
                Hide();
            });

            _graphicsButton?.onClick.AddListener(() =>
            {
                _graphicsPanel.SetActive(true);

                _soundsPanel.SetActive(false);
                _controlsPanel.SetActive(false);
                _accessibilityPanel.SetActive(false);
            });
            
            _soundsButton?.onClick.AddListener(() =>
            {
                _soundsPanel.SetActive(true);

                _graphicsPanel.SetActive(false);
                _controlsPanel.SetActive(false);
                _accessibilityPanel.SetActive(false);
            });

            _controlsButton?.onClick.AddListener(() =>
            {
                _controlsPanel.SetActive(true);

                _graphicsPanel.SetActive(false);
                _soundsPanel.SetActive(false);
                _accessibilityPanel.SetActive(false);
            });

            _accessibilityButton?.onClick.AddListener(() =>
            {
                _accessibilityPanel.SetActive(true);

                _graphicsPanel.SetActive(false);
                _soundsPanel.SetActive(false);
                _controlsPanel.SetActive(false);
            });

            #region Keybindings

            _turnTableRightKeybindingButton1.onClick.AddListener(() =>
            {
                RebindBinding(GameInputManager.Binding.TurnTableRight, 0);
            });

            _turnTableRightKeybindingButton2.onClick.AddListener(() =>
            {
                RebindBinding(GameInputManager.Binding.TurnTableRight, 1);
            });

            _turnTableLeftKeybindingButton1.onClick.AddListener(() =>
            {
                RebindBinding(GameInputManager.Binding.TurnTableLeft, 0);
            });

            _turnTableLeftKeybindingButton2.onClick.AddListener(() =>
            {
                RebindBinding(GameInputManager.Binding.TurnTableLeft, 1);
            });

            _placeCardKeybindingButton1.onClick.AddListener(() =>
            {
                RebindBinding(GameInputManager.Binding.PlaceCard, 0);
            });

            _placeCardKeybindingButton2.onClick.AddListener(() =>
            {
                RebindBinding(GameInputManager.Binding.PlaceCard, 1);
            });

            #endregion

            // Fill the dropdown with the supported resolutions.
            AddResolutions();

            // Add function to the resolution dropdown.
            _resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        private void OnCameraHeightValueChanged(float value)
        {
            if (SceneManager.GetActiveScene().name == "game_scene")
            {
                GameSettingsManager.Instance.ChangeCameraHeight(_cameraHeightSlider.value);
            }
            
            _cameraHeightText.text = Mathf.Round(GameSettingsManager.Instance.CameraHeight * 10).ToString();
        }

        public void SetResolution(int index)
        {
            Screen.SetResolution(_supportedResolutions[index].width, _supportedResolutions[index].height, FullScreenMode.ExclusiveFullScreen);
            _resolutionDropdown.value = index;
            _resolutionDropdown.RefreshShownValue();

            GameSettingsManager.Instance.ResolutionIndex = _resolutionDropdown.value;
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

            UpdateVisual();

            Hide();
            HideRebindPanel();
        }

        private void UpdateVisual()
        {
            // Set the texts to the according values + some black magic so it looks correct.
            _mainVolumeText.text = Mathf.Round(SoundManager.Instance.MainVolume * 10f).ToString();

            _musicVolumeText.text = Mathf.Round(SoundManager.Instance.MusicVolume * 10f).ToString();

            _sfxVolumeText.text = Mathf.Round(SoundManager.Instance.SfxVolume * 10f).ToString();

            _cameraHeightText.text = Mathf.Round(GameSettingsManager.Instance.CameraHeight * 10f).ToString();

            // Set the slider values to the sound manager values.
            _mainVolumeSlider.value = SoundManager.Instance.MainVolume;

            _musicVolumeSlider.value = SoundManager.Instance.MusicVolume;

            _sfxVolumeSlider.value = SoundManager.Instance.SfxVolume;

            _cameraHeightSlider.value = GameSettingsManager.Instance.CameraHeight;

            #region Keybindings

            _turnTableRightKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.TurnTableRight, 0);
            _turnTableRightKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.TurnTableRight, 1);
            _turnTableLeftKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.TurnTableLeft, 0);
            _turnTableLeftKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.TurnTableLeft, 1);
            _placeCardKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.PlaceCard, 0);
            _placeCardKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.PlaceCard, 1);

            #endregion
        }

        /// <summary>
        /// Rebind a binding.
        /// </summary>
        /// <param name="binding">The binding that gets changed.</param>
        /// <param name="bindingIndex">The binding index of the binding that gets changed. E.g. an action can have multiple bindings, jump can be space and arrow up.</param>
        private void RebindBinding(GameInputManager.Binding binding, int bindingIndex)
        {
            ShowRebindPanel();

            GameInputManager.Instance.RebindBinding(binding, () =>
            {
                HideRebindPanel();
                UpdateVisual();
            }, bindingIndex);
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

        private void ShowRebindPanel()
        {
            _rebindPanel.SetActive(true);
        }

        private void HideRebindPanel()
        {
            _rebindPanel.SetActive(false);
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
                _supportedResolutions?.Add(resolutions[i]);
            }
            
            _resolutionDropdown.RefreshShownValue();
        }

        private string ResolutionToString(Resolution res)
        {
            return res.width + " x " + res.height + " | " + res.refreshRateRatio + " Hz";
        }
    }
}
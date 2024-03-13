using Cinemachine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static JamCraft.GMTK2023.Code.GameInputManager;

namespace JamCraft.GMTK2023.Code
{
    public class GameOptionsUI : MonoBehaviour
    {
        public static GameOptionsUI Instance { get; private set; }

        public const string PLAYER_PREFS_MAIN_VOLUME = "MainVolume";
        public const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
        public const string PLAYER_PREFS_SFX_VOLUME = "SfxVolume";
        public const string PLAYER_PREFS_CAMERA_HEIGHT = "CameraHeightValue";
        public const string PLAYER_PREFS_RESOLUTION = "ResolutionValue";

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
        [SerializeField] private TextMeshProUGUI _rebindPanelText;
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

        public UnityEvent OnResetToDefault;

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

            _resetToDefaultButton.onClick.AddListener(() =>
            {
                OnResetToDefault?.Invoke();
                UpdateVisual();
            });

            // Close options menu and show pause menu.
            _backButton.onClick.AddListener(() =>
            {
                Hide();

                if (GamePauseUI.Instance != null)
                {
                    GamePauseUI.Instance.Show();
                }

                if (MainMenuUI.Instance != null)
                {
                    MainMenuUI.Instance.Show();
                }
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
                RebindBinding(Actions.TurnTableRight, 0);
            });

            _turnTableRightKeybindingButton2.onClick.AddListener(() =>
            {
                RebindBinding(Actions.TurnTableRight, 1);
            });

            _turnTableLeftKeybindingButton1.onClick.AddListener(() =>
            {
                RebindBinding(Actions.TurnTableLeft, 0);
            });

            _turnTableLeftKeybindingButton2.onClick.AddListener(() =>
            {
                RebindBinding(Actions.TurnTableLeft, 1);
            });

            _placeCardKeybindingButton1.onClick.AddListener(() =>
            {
                RebindBinding(Actions.PlaceCard, 0);
            });

            _placeCardKeybindingButton2.onClick.AddListener(() =>
            {
                RebindBinding(Actions.PlaceCard, 1);
            });

            #endregion

            // Fill the dropdown with the supported resolutions.
            AddResolutions();

            // Add function to the resolution dropdown.
            _resolutionDropdown.onValueChanged.AddListener(SetResolution);

            EventManager.Instance.OnGameOptionsUIInitialized?.Invoke();
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

            if (GameInputManager.Instance != null)
            {
                GameInputManager.Instance.OnDuplicateKeybindingFound.AddListener(GameInputManager_OnDuplicateKeybindingFound);
            }

            if (GameInputManager.Instance != null)
            {
                OnResetToDefault.AddListener(GameInputManager.Instance.GameOptionsUI_OnResetToDefault);
                GameInputManager.Instance.OnInputDeviceChanged.AddListener(SwapInputIcons);
                GameInputManager.Instance.OnInputDeviceChanged.AddListener(SetGamepadFocusOptionsMenu);
            }

            UpdateVisual();

            Hide();
            HideRebindPanel();
        }

        private void SetGamepadFocusOptionsMenu(ControlScheme controlScheme)
        {
            if (controlScheme == ControlScheme.Gamepad)
            {
                if (!gameObject.activeSelf) return;

                _graphicsButton.Select();
            }
        }

        private void SwapInputIcons(ControlScheme controlScheme)
        {
            switch (controlScheme)
            {
                default:
                case ControlScheme.Keyboard:
                    _turnTableRightKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableRight, 0);
                    _turnTableRightKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableRight, 1);
                    _turnTableLeftKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableLeft, 0);
                    _turnTableLeftKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableLeft, 1);
                    _placeCardKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.PlaceCard, 0);
                    _placeCardKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.PlaceCard, 1);

                    _turnTableRightKeybindingButton1.onClick.RemoveAllListeners();
                    _turnTableRightKeybindingButton2.onClick.RemoveAllListeners();
                    _turnTableLeftKeybindingButton1.onClick.RemoveAllListeners();
                    _turnTableLeftKeybindingButton2.onClick.RemoveAllListeners();
                    _placeCardKeybindingButton1.onClick.RemoveAllListeners();
                    _placeCardKeybindingButton2.onClick.RemoveAllListeners();

                    _turnTableRightKeybindingButton1.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.TurnTableRight, 0);
                    });

                    _turnTableRightKeybindingButton2.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.TurnTableRight, 1);
                    });

                    _turnTableLeftKeybindingButton1.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.TurnTableLeft, 0);
                    });

                    _turnTableLeftKeybindingButton2.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.TurnTableLeft, 1);
                    });

                    _placeCardKeybindingButton1.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.PlaceCard, 0);
                    });

                    _placeCardKeybindingButton2.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.PlaceCard, 1);
                    });
                    break;
                case ControlScheme.Gamepad:
                    _turnTableRightKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableRight, 2);
                    _turnTableRightKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableRight, 3);
                    _turnTableLeftKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableLeft, 2);
                    _turnTableLeftKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableLeft, 3);
                    _placeCardKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.PlaceCard, 2);
                    _placeCardKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.PlaceCard, 3);

                    _turnTableRightKeybindingButton1.onClick.RemoveAllListeners();
                    _turnTableRightKeybindingButton2.onClick.RemoveAllListeners();
                    _turnTableLeftKeybindingButton1.onClick.RemoveAllListeners();
                    _turnTableLeftKeybindingButton2.onClick.RemoveAllListeners();
                    _placeCardKeybindingButton1.onClick.RemoveAllListeners();
                    _placeCardKeybindingButton2.onClick.RemoveAllListeners();

                    _turnTableRightKeybindingButton1.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.TurnTableRight, 2);
                    });

                    _turnTableRightKeybindingButton2.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.TurnTableRight, 3);
                    });

                    _turnTableLeftKeybindingButton1.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.TurnTableLeft, 2);
                    });

                    _turnTableLeftKeybindingButton2.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.TurnTableLeft, 3);
                    });

                    _placeCardKeybindingButton1.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.PlaceCard, 2);
                    });

                    _placeCardKeybindingButton2.onClick.AddListener(() =>
                    {
                        RebindBinding(GameInputManager.Actions.PlaceCard, 3);
                    });
                    break;
            }
        }

        private void GameInputManager_OnDuplicateKeybindingFound(InputBinding binding)
        {
            _rebindPanelText.text = $"Key is already being used for {binding.action}!\nPlease use another key.";
        }

        public void UpdateVisual()
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

            _turnTableRightKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableRight, 0);
            _turnTableRightKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableRight, 1);
            _turnTableLeftKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableLeft, 0);
            _turnTableLeftKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.TurnTableLeft, 1);
            _placeCardKeybindingText1.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.PlaceCard, 0);
            _placeCardKeybindingText2.text = GameInputManager.Instance.GetBindingText(GameInputManager.Actions.PlaceCard, 1);

            #endregion
        }

        /// <summary>
        /// Rebind a actions.
        /// </summary>
        /// <param name="actions">The actions that gets changed.</param>
        /// <param name="bindingIndex">The actions index of the actions that gets changed. E.g. an action can have multiple bindings, jump can be space and arrow up.</param>
        private void RebindBinding(GameInputManager.Actions actions, int bindingIndex)
        {
            ShowRebindPanel(actions);

            GameInputManager.Instance.RebindBinding(actions, () =>
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
            _uiCamera.Follow = OptionsCameraFocus;

            CinemachineComponentBase componentBase = _uiCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is CinemachineFramingTransposer)
            {
                (componentBase as CinemachineFramingTransposer).m_CameraDistance = 1f;
            }

            gameObject.SetActive(true);

            _graphicsButton.Select();
        }

        private void ShowRebindPanel(GameInputManager.Actions actions)
        {
            _rebindPanelText.text = $"Press a key to rebind {actions}.\nPress {Keyboard.current.escapeKey.displayName} or {Gamepad.current.buttonEast.displayName} to cancel the process.";

            _rebindPanel.SetActive(true);
        }

        public void HideRebindPanel()
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

        private void OnDestroy()
        {
            if (GameStateManager.Instance != null)
            {
                // Unsubscribe from events in case of destruction.
                GameStateManager.Instance.OnGameUnpaused -= GameStateManager_OnOnGameUnpaused;
            }

            if (GameInputManager.Instance != null)
            {
                GameInputManager.Instance.OnInputDeviceChanged.RemoveListener(SetGamepadFocusOptionsMenu);
                GameInputManager.Instance.OnInputDeviceChanged.RemoveListener(SwapInputIcons);

                GameInputManager.Instance.OnDuplicateKeybindingFound.RemoveListener(GameInputManager_OnDuplicateKeybindingFound);
            }
        }

        private void OnApplicationQuit()
        {
            if (GameStateManager.Instance != null)
            {
                // Unsubscribe from events in case of destruction.
                GameStateManager.Instance.OnGameUnpaused -= GameStateManager_OnOnGameUnpaused;
            }

            if (GameInputManager.Instance != null)
            {
                GameInputManager.Instance.OnInputDeviceChanged.RemoveListener(SetGamepadFocusOptionsMenu);
                GameInputManager.Instance.OnInputDeviceChanged.RemoveListener(SwapInputIcons);

                GameInputManager.Instance.OnDuplicateKeybindingFound.RemoveListener(GameInputManager_OnDuplicateKeybindingFound);
            }
        }
    }
}
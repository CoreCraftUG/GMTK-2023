using System;
using System.Collections.Generic;
using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
    public class GameSettingsFile : MonoBehaviour
    {
        #region Variables

        public static GameSettingsFile Instance { get; private set; }

        private readonly string _userSettingsFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\My Games\\House Always WINS!\\GameUserSettings.ini";

        public string UserSettingsFilePath { get => _userSettingsFilePath; }

        // Graphics Settings
        [SerializeField] private int _resolutionIndex;
        [SerializeField] private Resolution _lastSavedResolution;
        [SerializeField] private Resolution[] _supportedResolutions;
        [SerializeField] private int _displayIndex;
        [SerializeField] private DisplayInfo _lastSavedDisplay;
        [SerializeField] private List<DisplayInfo> _supportedDisplays = new List<DisplayInfo>();
        [SerializeField] private int _windowModeIndex;
        [SerializeField] private FullScreenMode _lastSavedWindowMode;
        [SerializeField] private bool _vSync;
        [SerializeField] private int _textureQualityIndex;
        [SerializeField] private int _shadowQualityIndex;
        [SerializeField] private bool _softShadows;
        //[SerializeField] private bool _hdr;
        //[SerializeField] private bool _ssao;
        [SerializeField] private float _cameraDistance;

        // Audio Settings
        [SerializeField] private float _mainVolume;
        [SerializeField] private float _musicVolume;
        [SerializeField] private float _sfxVolume;

        /// <summary>
        /// Saved index of the resolution dropdown.
        /// </summary>
        public int ResolutionIndex { get => _resolutionIndex; set => _resolutionIndex = value; }

        /// <summary>
        /// Last saved resolution.
        /// </summary>
        public Resolution LastSavedResolution { get => _lastSavedResolution; set => _lastSavedResolution = value; }

        /// <summary>
        /// Holds all supported resolutions of the current display.
        /// </summary>
        public Resolution[] SupportedResolutions { get => _supportedResolutions; set => _supportedResolutions = value; }

        /// <summary>
        /// Saved index of the display dropdown.
        /// </summary>
        public int DisplayIndex { get => _displayIndex; set => _displayIndex = value; }

        /// <summary>
        /// Last saved display.
        /// </summary>
        public DisplayInfo LastSavedDisplay { get => _lastSavedDisplay; set => _lastSavedDisplay = value; }

        /// <summary>
        /// Holds all connected displays.
        /// </summary>
        public List<DisplayInfo> SupportedDisplays { get => _supportedDisplays; set => _supportedDisplays = value; }

        /// <summary>
        /// Saved index of the window mode dropdown.
        /// </summary>
        public int WindowModeIndex { get => _windowModeIndex; set => _windowModeIndex = value; }

        /// <summary>
        /// Last saved window mode.
        /// </summary>
        public FullScreenMode LastSavedWindowMode { get => _lastSavedWindowMode; set => _lastSavedWindowMode = value; }

        /// <summary>
        /// Saved toggle state of the V-Sync toggle.
        /// </summary>
        public bool VSync { get => _vSync; set => _vSync = value; }

        /// <summary>
        /// Saved index of the texture quality dropdown.
        /// </summary>
        public int TextureQualityIndex { get => _textureQualityIndex; set => _textureQualityIndex = value; }

        /// <summary>
        /// Saved index of the shadow quality dropdown.
        /// </summary>
        public int ShadowQualityIndex { get => _shadowQualityIndex; set => _shadowQualityIndex = value; }

        /// <summary>
        /// Saved toggle state of the Soft Shadows toggle.
        /// </summary>
        public bool SoftShadows { get => _softShadows; set => _softShadows = value; }

        /// <summary>
        /// Saved toggle state of the HDR toggle.
        /// </summary>
        //public bool HDR { get => _hdr; set => _hdr = value; }

        /// <summary>
        /// Saved toggle state of the SSAO toggle.
        /// </summary>
        //public bool SSAO { get => _ssao; set => _ssao = value; }

        /// <summary>
        /// Saved value if the camera distance slider.
        /// </summary>
        public float CameraDistance { get => _cameraDistance; set => _cameraDistance = value; }

        /// <summary>
        /// Saved value of the main volume.
        /// </summary>
        public float MainVolume { get => _mainVolume; set => _mainVolume = value; }

        /// <summary>
        /// Saved value of the music volume.
        /// </summary>
        public float MusicVolume { get => _musicVolume; set => _musicVolume = value; }

        /// <summary>
        /// Saved value of the sfx volume.
        /// </summary>
        public float SfxVolume { get => _sfxVolume; set => _sfxVolume = value; }

        /// <summary>
        /// Default volume for sliders.
        /// </summary>
        /// <remarks>Value ensures that the sliders are centered.</remarks>
        public const float DEFAULT_VOLUME = 0.5000499f;

        #endregion

        #region UserSettingsDataStrings
        
        public const string USERSETTINGS_RESOLUTION = "UserSettings_ResolutionValue";
        public const string USERSETTINGS_LASTSAVEDRESOLUTION = "UserSettings_LastSavedResolution";
        public const string USERSETTINGS_DISPLAY = "UserSettings_DisplayValue";
        public const string USERSETTINGS_LASTSAVEDDISPLAY = "UserSettings_LastSavedDisplay";
        public const string USERSETTINGS_WINDOW_MODE = "UserSettings_WindowModeValue";
        public const string USERSETTINGS_LASTSAVEDWINDOWMODE = "UserSettings_LastSavedWindowMode";
        public const string USERSETTINGS_VSYNC = "UserSettings_VSyncValue";
        public const string USERSETTINGS_TEXTURE_QUALITY = "UserSettings_TextureQualityValue";
        public const string USERSETTINGS_SHADOW_QUALITY = "UserSettings_ShadowQualityValue";
        public const string USERSETTINGS_SOFT_SHADOWS = "UserSettings_SoftShadowsValue";
        //public const string USERSETTINGS_HDR = "UserSettings_HdrValue";
        //public const string USERSETTINGS_SSAO = "UserSettings_SsaoValue";
        public const string USERSETTINGS_CAMERA_DISTANCE = "UserSettings_CameraDistanceValue"; // TODO: Fix the implementation.
        public const string USERSETTINGS_MAIN_VOLUME = "UserSettings_MainVolume";
        public const string USERSETTINGS_MUSIC_VOLUME = "UserSettings_MusicVolume";
        public const string USERSETTINGS_SFX_VOLUME = "UserSettings_SfxVolume";

        public const string USERSETTINGS_INPUT_BINDINGS = "UserSettings_InputBindings";

        #endregion

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            DontDestroyOnLoad(this.gameObject);

            if (ES3.FileExists(UserSettingsFilePath))
            {
                LoadUserSettings();
            }
        }

        private void LoadUserSettings()
        {
            if (ES3.KeyExists(USERSETTINGS_RESOLUTION, UserSettingsFilePath))
            {
                _resolutionIndex = ES3.Load<int>(USERSETTINGS_RESOLUTION, 0);
            }

            if (ES3.KeyExists(USERSETTINGS_LASTSAVEDRESOLUTION, UserSettingsFilePath))
            {
                try
                {
                    _lastSavedResolution = ES3.Load<Resolution>(USERSETTINGS_RESOLUTION);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }
            }

            if (ES3.KeyExists(USERSETTINGS_DISPLAY, UserSettingsFilePath))
            {
                _displayIndex = ES3.Load<int>(USERSETTINGS_DISPLAY, 0);
            }

            if (ES3.KeyExists(USERSETTINGS_RESOLUTION, UserSettingsFilePath))
            {
                try
                {
                    _lastSavedDisplay = ES3.Load<DisplayInfo>(USERSETTINGS_LASTSAVEDDISPLAY);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }
            }

            if (ES3.KeyExists(USERSETTINGS_WINDOW_MODE, UserSettingsFilePath))
            {
                _windowModeIndex = ES3.Load<int>(USERSETTINGS_WINDOW_MODE, 0);
            }

            if (ES3.KeyExists(USERSETTINGS_LASTSAVEDWINDOWMODE, UserSettingsFilePath))
            {
                try
                {
                    _lastSavedWindowMode = ES3.Load<FullScreenMode>(USERSETTINGS_LASTSAVEDWINDOWMODE);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }
            }

            if (ES3.KeyExists(USERSETTINGS_VSYNC, UserSettingsFilePath))
            {
                _vSync = ES3.Load<bool>(USERSETTINGS_VSYNC, false);
            }

            if (ES3.KeyExists(USERSETTINGS_TEXTURE_QUALITY, UserSettingsFilePath))
            {
                _textureQualityIndex = ES3.Load<int>(USERSETTINGS_TEXTURE_QUALITY, 0);
            }

            if (ES3.KeyExists(USERSETTINGS_SHADOW_QUALITY, UserSettingsFilePath))
            {
                _shadowQualityIndex = ES3.Load<int>(USERSETTINGS_SHADOW_QUALITY, 0);
            }

            if (ES3.KeyExists(USERSETTINGS_SOFT_SHADOWS, UserSettingsFilePath))
            {
                _softShadows = ES3.Load<bool>(USERSETTINGS_SOFT_SHADOWS, false);
            }

            //if (ES3.KeyExists(USERSETTINGS_HDR, UserSettingsFilePath))
            //{
            //    _hdr = ES3.Load<bool>(USERSETTINGS_HDR, false);
            //}

            //if (ES3.KeyExists(USERSETTINGS_SSAO, UserSettingsFilePath))
            //{
            //    _ssao = ES3.Load<bool>(USERSETTINGS_SSAO, false);
            //}

            if (ES3.KeyExists(USERSETTINGS_CAMERA_DISTANCE, UserSettingsFilePath))
            {
                _cameraDistance = ES3.Load<float>(USERSETTINGS_CAMERA_DISTANCE, 1.5f);
            }

            if (ES3.KeyExists(USERSETTINGS_MAIN_VOLUME, UserSettingsFilePath))
            {
                _mainVolume = ES3.Load<float>(USERSETTINGS_MAIN_VOLUME, DEFAULT_VOLUME);
            }

            if (ES3.KeyExists(USERSETTINGS_MUSIC_VOLUME, UserSettingsFilePath))
            {
                _musicVolume = ES3.Load<float>(USERSETTINGS_MUSIC_VOLUME, DEFAULT_VOLUME);
            }

            if (ES3.KeyExists(USERSETTINGS_SFX_VOLUME, UserSettingsFilePath))
            {
                _sfxVolume = ES3.Load<float>(USERSETTINGS_SFX_VOLUME, DEFAULT_VOLUME);
            }
        }
    }
}
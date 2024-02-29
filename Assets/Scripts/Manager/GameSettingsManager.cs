using CoreCraft.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JamCraft.GMTK2023.Code
{
    public class GameSettingsManager : MonoBehaviour
    {
        public static GameSettingsManager Instance { get; private set; }

        public float CameraHeight { get; set; } = 2.3f;
        public int ResolutionIndex { get; set; }

        public Transform VirtualCameraTransform { get; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            // Get the saves values and set the properties accordingly.
            CameraHeight = PlayerPrefs.GetFloat(GameOptionsUI.PLAYER_PREFS_CAMERA_HEIGHT, 0.9f);
            ResolutionIndex = PlayerPrefs.GetInt(GameOptionsUI.PLAYER_PREFS_RESOLUTION);
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "game_scene")
            {
                ChangeCameraHeight(CameraHeight);
            }

            GameOptionsUI.Instance.SetResolution(ResolutionIndex);
        }

        public void ChangeCameraHeight(float value)
        {
            // Set CameraHeight to the slider value display the value.
            CameraHeight = value;

            if (VirtualCameraTransform != null)
            { 
                VirtualCameraTransform.localPosition = new Vector3(VirtualCameraTransform.localPosition.x, CameraHeight, VirtualCameraTransform.localPosition.z);
            }
        }
    }
}
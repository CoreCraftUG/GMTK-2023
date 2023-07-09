using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JamCraft.GMTK2023.Code
{
    public class GameSettingsManager : MonoBehaviour
    {
        public static GameSettingsManager Instance { get; private set; }

        public float CameraHeight = 2.3f;
        public int ResolutionIndex;

        public Transform VirtualCameraTransform;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            // Get the saves values and set the properties accordingly.
            CameraHeight = PlayerPrefs.GetFloat(GameOptionsUI.PLAYER_PREFS_CAMERA_HEIGHT, 2.3f);
            ResolutionIndex = PlayerPrefs.GetInt(GameOptionsUI.PLAYER_PREFS_RESOLUTION);
        }

        private void Start()
        {
            //VirtualCameraTransform = Camera.main.GetComponent<Transform>();

            if (SceneManager.GetActiveScene().ToString() == Loader.Scene.game_scene.ToString())
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
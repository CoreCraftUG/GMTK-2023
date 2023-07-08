using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
    public class GameSettingsManager : MonoBehaviour
    {
        public static GameSettingsManager Instance { get; private set; }

        public float CameraHeight = 2.3f;

        private Transform _mainCameraTransform;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            // Get the saves values and set the properties accordingly.
            CameraHeight = PlayerPrefs.GetFloat(GameOptionsUI.PLAYER_PREFS_CAMERA_HEIGHT, 2.3f);
        }

        private void Start()
        {
            _mainCameraTransform = Camera.main.GetComponent<Transform>();

            ChangeCameraHeight(CameraHeight);
        }

        public void ChangeCameraHeight(float value)
        {
            // Set CameraHeight to the slider value display the value.
            CameraHeight = value;

            if (_mainCameraTransform != null)
            { 
                _mainCameraTransform.localPosition = new Vector3(_mainCameraTransform.localPosition.x, CameraHeight, _mainCameraTransform.localPosition.z);
            }
        }
    }
}
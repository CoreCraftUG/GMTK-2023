using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        public float MainVolume = 5f;
        public float MusicVolume = 5f;
        public float SfxVolume = 5f;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;
        }

        public void ChangeMainVolume(float value)
        {
            MainVolume = value;
        }

        public void ChangeMusicVolume(float value)
        {
            MusicVolume = value;
        }

        public void ChangeSfxVolume(float value)
        {
            SfxVolume = value;
        }
    }
}
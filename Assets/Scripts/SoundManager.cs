using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        public float _volume;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;
        }

        public void ChangeVolume(float value)
        {
            _volume = value;
        }
    }
}
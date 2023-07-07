using UnityEngine;
using UnityEngine.Audio;

namespace JamCraft.GMTK2023.Code
{
    public class SoundManager : MonoBehaviour
    {
        private const string _mainVolume = "masterVolume";
        private const string _musicVolume = "musicVolume";
        private const string _sfxVolume = "sfxVolume";

        public static SoundManager Instance { get; private set; }

        public float MainVolume = 5f;
        public float MusicVolume = 5f;
        public float SfxVolume = 5f;

        [Header("Sound Mixer")] 
        [SerializeField] private AudioMixer _mainAudioMixer;

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
            _mainAudioMixer.SetFloat(_mainVolume, Mathf.Log10(MainVolume) * 20);
        }

        public void ChangeMusicVolume(float value)
        {
            MusicVolume = value;
            _mainAudioMixer.SetFloat(_musicVolume, Mathf.Log10(MusicVolume) * 20);

        }

        public void ChangeSfxVolume(float value)
        {
            SfxVolume = value;
            _mainAudioMixer.SetFloat(_sfxVolume, Mathf.Log10(SfxVolume) * 20);

        }
    }
}
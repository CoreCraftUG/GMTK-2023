using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using CoreCraft.Core;
using Sirenix.OdinInspector;
using System;

namespace JamCraft.GMTK2023.Code
{
    public class SoundManager : Singleton<SoundManager>
    {
        private const string MAIN_VOLUME = "masterVolume";
        private const string MUSIC_VOLUME = "musicVolume";
        private const string SFX_VOLUME = "sfxVolume";

        public float MainVolume = 0.5000499f;
        public float MusicVolume = 0.5000499f;
        public float SfxVolume = 0.5000499f;
        public bool EventManagerReady;

        [Header("Sound Mixer")] 
        [SerializeField] private AudioMixer _mainAudioMixer;
        [SerializeField] private List<AudioClip> _allClips = new List<AudioClip>();
        [SerializeField] private AudioSource _sfxSource;

        [FoldoutGroup("Music"), Header("Audio Sources"), SerializeField] private AudioSource _mainTrack;
        [FoldoutGroup("Music"), SerializeField] private AudioSource _track1;
        [FoldoutGroup("Music"), SerializeField] private AudioSource _track2;
        [FoldoutGroup("Music"), SerializeField] private AudioSource _track3;
        [FoldoutGroup("Music"), SerializeField] private AudioSource _track4;
        [FoldoutGroup("Music"), SerializeField] private AudioSource _track5;
        [FoldoutGroup("Music"), SerializeField] private AudioSource _track6;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        
        private void Start()
        {
            StartCoroutine(SetUpCoroutine());
        }

        private IEnumerator SetUpCoroutine()
        {
            yield return new WaitUntil(() =>
            {
                return EventManagerReady;
            });

            EventManager.Instance.OnGameOptionsUIInitialized.AddListener(() => SetUpOptionsUI());
            EventManager.Instance.PlayAudio.AddListener(PlaySFXDelayed);

            EventManager.Instance.PointMultiplyEvent.AddListener(MusicMultiply);
        }

        private void MusicMultiply(float value)
        {
        }

        private void SyncAudioSources()
        {

        }

        public void SetUpOptionsUI()
        {
            // Get the saves values and set the volumes accordingly.
            MainVolume = PlayerPrefs.GetFloat(GameOptionsUI.PLAYER_PREFS_MAIN_VOLUME, 0.5000499f);
            MusicVolume = PlayerPrefs.GetFloat(GameOptionsUI.PLAYER_PREFS_MUSIC_VOLUME, 0.5000499f);
            SfxVolume = PlayerPrefs.GetFloat(GameOptionsUI.PLAYER_PREFS_SFX_VOLUME, 0.5000499f);
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.PlayAudio.RemoveAllListeners();
            }
        }

        private void OnApplicationQuit()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.PlayAudio.RemoveAllListeners();
            }
        }

        public void PlaySFXDelayed(int clip, float delay)
        {
            _sfxSource.clip = _allClips[clip];

            _sfxSource.PlayDelayed(delay);
        }

        public void PlaySFX(int clip)
        {
            _sfxSource.clip = _allClips[clip];

            _sfxSource.PlayOneShot(_allClips[clip]);
        }

        public void ChangeMainVolume(float value)
        {
            // Set MainVolume to the slider value and do some magic fuckery to display a correct value in the text.
            MainVolume = value;
            _mainAudioMixer.SetFloat(MAIN_VOLUME, Mathf.Log10(MainVolume) * 20);
        }

        public void ChangeMusicVolume(float value)
        {
            // Set MusicVolume to the slider value and do some magic fuckery to display a correct value in the text.
            MusicVolume = value;
            _mainAudioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(MusicVolume) * 20);
        }

        public void ChangeSfxVolume(float value)
        {
            // Set SfxVolume to the slider value and do some magic fuckery to display a correct value in the text.
            SfxVolume = value;
            _mainAudioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(SfxVolume) * 20);
        }
    }
}
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using CoreCraft.Core;
using Sirenix.OdinInspector;

namespace JamCraft.GMTK2023.Code
{
    public class SoundManager : Singleton<SoundManager>
    {
        public bool EventManagerReady;

        [Header("Sound Mixer")] 
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

            EventManager.Instance.PlayAudio.AddListener(PlaySFXDelayed);

            EventManager.Instance.PointMultiplyEvent.AddListener(MusicMultiply);
        }

        private void MusicMultiply(float value)
        {

        }

        private void SyncAudioSources()
        {

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
    }
}
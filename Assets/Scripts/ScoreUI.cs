using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

namespace JamCraft.GMTK2023.Code
{
    public class ScoreUI : MonoBehaviour
    {
        public static ScoreUI Instance { get; private set; }

        [Header("UI Texts")] 
        [SerializeField] private TextMeshProUGUI _temporaryScoreText;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        [SerializeField] private TextMeshProUGUI _levelText;
        
        [SerializeField] private List<GameObject> _streakLamps = new List<GameObject>();

        [SerializeField] private VolumeProfile _postProcessing;
        private Beautify.Universal.Beautify _beautify;

        public int NewScore { get; private set; } = 0;
        public int NewTemporaryScore { get; private set; } = 0;
        public int Level { get; private set; } = 1;

        private float _timer = 5f;
        private float _timerTotal = 5f;
        private float _startingSepia;
        private Tween _temporaryScoreTween;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            GameStateManager.Instance.OnGamePaused += Instance_OnGamePaused;
            GameStateManager.Instance.OnGameUnpaused += Instance_OnGameUnpaused;
        }

        private void Instance_OnGameUnpaused(object sender, System.EventArgs e)
        {
            gameObject.SetActive(true);
        }

        private void Instance_OnGamePaused(object sender, System.EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            EventManager.Instance.PointsAddedEvent.AddListener(AddScore);
            EventManager.Instance.PointMultiplyEvent.AddListener(SetMultiplier);
            EventManager.Instance.TempPointsEvent.AddListener(AddTemporaryScore);
            EventManager.Instance.StreakEndEvent.AddListener(StreakEndEvent);
            EventManager.Instance.MissedMultiplyEvent.AddListener(MissedMultiplierEvent);
            EventManager.Instance.LevelUpEvent.AddListener(LevelUpEvent);

            if (_postProcessing.TryGet<Beautify.Universal.Beautify>(out _beautify))
            {
                _beautify.sepia.value = 0.746f;
            }

            _startingSepia = _beautify.sepia.value;

            _temporaryScoreTween.SetAutoKill(false).Pause();
        }

        private void LevelUpEvent(int value)
        {
            Level = value;
            _levelText.text = "Level - " + value.ToString();
        }
        
        private void AddScore(int value)
        { 
            NewScore = value;
        }

        public void SetMultiplier(float value)
        {
            _multiplierText.text = value.ToString("F1");

            foreach (GameObject streak in _streakLamps)
            {
                streak.GetComponent<Renderer>().material.SetFloat("_UseEmission", 1);
            }
        }

        public void AddTemporaryScore(int value)
        {
            int oldTemporaryScore = NewTemporaryScore;
            NewTemporaryScore = value;

            if (_postProcessing.TryGet<Beautify.Universal.Beautify>(out _beautify))
            {
                _timer = 5f;
                _timerTotal = 5f;
                _beautify.sepia.value -= (oldTemporaryScore + NewTemporaryScore) / 2000f;
                _startingSepia = _beautify.sepia.value;
            }

            _temporaryScoreTween = _temporaryScoreText.DOCounter(oldTemporaryScore, NewTemporaryScore, 2f, false, null);

            if (_temporaryScoreTween.playedOnce)
            {
                _temporaryScoreTween.Restart();
            }
            else
            {
                _temporaryScoreTween.Play();
            }
        }

        private void StreakEndEvent()
        {
            _multiplierText.text = "1";
        }

        private void MissedMultiplierEvent(int value)
        {
            if (value > 3) return;

            for (int i = 0; i < value; i++)
            {
                _streakLamps[i].GetComponent<Renderer>().material.SetFloat("_UseEmission", 0);
            }
        }

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;

                if (_postProcessing.TryGet<Beautify.Universal.Beautify>(out _beautify))
                {
                    _beautify.sepia.value = Mathf.Lerp(_startingSepia, 0.746f, 1 - (_timer / _timerTotal));
                }
            }
        }

        private void OnDestroy()
        {
            if (GameStateManager.Instance != null)
            {
                // Unsubscribe from events in case of destruction.
                GameStateManager.Instance.OnGamePaused -= Instance_OnGamePaused;
                GameStateManager.Instance.OnGameUnpaused -= Instance_OnGameUnpaused;
            }
        }

        private void OnApplicationQuit()
        {
            if (GameStateManager.Instance != null)
            {
                // Unsubscribe from events in case of destruction.
                GameStateManager.Instance.OnGamePaused -= Instance_OnGamePaused;
                GameStateManager.Instance.OnGameUnpaused -= Instance_OnGameUnpaused;
            }
        }
    }
}
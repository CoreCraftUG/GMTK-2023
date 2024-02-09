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
        //[SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _temporaryScoreText;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [SerializeField] private GameObject _streakHolder;
        [SerializeField] private GameObject _streakPrefab;
        [SerializeField] private List<GameObject> _streaks = new List<GameObject>();

        [SerializeField] private VolumeProfile _postProcessing;
        private Beautify.Universal.Beautify _beautify;

        private int _oldScore = 0;
        private int _oldTemporaryScore = 0;

        public int NewScore = 0;
        public int NewTemporaryScore = 0;

        public int Level = 1;

        private float _timer = 5f;
        private float _timerTotal = 5f;
        private float _startingSepia;
        private Tween _scoreTween;
        private Tween _temporaryScoreTween;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;
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

            for (int i = 0; i < 3; i++)
            {
                _streaks.Add(Instantiate(_streakPrefab, _streakHolder.transform));
            }

            _scoreTween.SetAutoKill(false).Pause();
            _temporaryScoreTween.SetAutoKill(false).Pause();
        }

        private void LevelUpEvent(int value)
        {
            Level = value;
            _levelText.text = "Level: " + value.ToString();
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.PointsAddedEvent.RemoveAllListeners();
                EventManager.Instance.PointMultiplyEvent.RemoveListener(SetMultiplier);
                EventManager.Instance.TempPointsEvent.RemoveAllListeners();
                EventManager.Instance.StreakEndEvent.RemoveAllListeners();
                EventManager.Instance.MissedMultiplyEvent.RemoveAllListeners();
            }
        }

        private void OnApplicationQuit()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.PointsAddedEvent.RemoveAllListeners();
                EventManager.Instance.PointMultiplyEvent.RemoveListener(SetMultiplier);
                EventManager.Instance.TempPointsEvent.RemoveAllListeners();
                EventManager.Instance.StreakEndEvent.RemoveAllListeners();
                EventManager.Instance.MissedMultiplyEvent.RemoveAllListeners();
            }
        }
        
        private void AddScore(int value)
        {
            _oldScore = NewScore;
            NewScore = value;

            //_scoreTween = _scoreText.DOCounter(_oldScore, NewScore, 2f, false, null);
            //_scoreText.DOCounter(_oldScore, NewScore, 2f, false, null);

            //if (_scoreTween.playedOnce)
            //{
            //    _scoreTween.Restart();
            //}
            //else
            //{
            //    _scoreTween.Play();
            //}
        }

        public void SetMultiplier(float value)
        {
            _multiplierText.text = value.ToString("F1");

            foreach (GameObject streak in _streaks)
            {
                streak.SetActive(true);
            }
        }

        public void AddTemporaryScore(int value)
        {
            _oldTemporaryScore = NewTemporaryScore;
            NewTemporaryScore = value;

            if (_postProcessing.TryGet<Beautify.Universal.Beautify>(out _beautify))
            {
                _timer = 5f;
                _timerTotal = 5f;
                _beautify.sepia.value -= (_oldTemporaryScore + NewTemporaryScore) / 2000f;
                _startingSepia = _beautify.sepia.value;
            }

            _temporaryScoreTween = _temporaryScoreText.DOCounter(_oldTemporaryScore, NewTemporaryScore, 2f, false, null);
            //_temporaryScoreText.DOCounter(_oldTemporaryScore, NewTemporaryScore, 2f, false, null);

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
                _streaks[i].SetActive(false);
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
    }
}
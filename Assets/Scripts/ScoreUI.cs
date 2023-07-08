using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace JamCraft.GMTK2023.Code
{
    public class ScoreUI : MonoBehaviour
    {
        [Header("UI Texts")] 
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _temporaryScoreText;
        [SerializeField] private TextMeshProUGUI _multiplierText;

        [SerializeField] private GameObject _streakHolder;
        [SerializeField] private GameObject _streakPrefab;
        [SerializeField] private List<GameObject> _streaks = new List<GameObject>();

        private int _oldScore = 0;
        private int _oldTemporaryScore = 0;

        private int _newScore = 0;
        private int _newTemporaryScore = 0;

        private Tween _scoreTween;
        private Tween _temporaryScoreTween;

        private void Start()
        {
            EventManager.Instance.PointsAddedEvent.AddListener(AddScore);
            EventManager.Instance.PointMultiplyEvent.AddListener(SetMultiplier);
            EventManager.Instance.TempPointsEvent.AddListener(AddTemporaryScore);
            EventManager.Instance.StreakEndEvent.AddListener(StreakEndEvent);
            EventManager.Instance.MissedMultiplyEvent.AddListener(MissedMultiplierEvent);
            
            for (int i = 0; i < 3; i++)
            {
                _streaks.Add(Instantiate(_streakPrefab, _streakHolder.transform));
            }

            _scoreTween.SetAutoKill(false).Pause();
            _temporaryScoreTween.SetAutoKill(false).Pause();
        }
        
        private void AddScore(int value)
        {
            _oldScore = _newScore;
            _newScore = value;

            _scoreTween = _scoreText.DOCounter(_oldScore, _newScore, 2f, false, null);
            //_scoreText.DOCounter(_oldScore, _newScore, 2f, false, null);

            if (_scoreTween.playedOnce)
            {
                _scoreTween.Restart();
            }
            else
            {
                _scoreTween.Play();
            }
        }

        public void SetMultiplier(float value)
        {
            _multiplierText.text = "Multiplier: " + value.ToString() + "x";

            foreach (GameObject streak in _streaks)
            {
                streak.SetActive(true);
            }
        }

        public void AddTemporaryScore(int value)
        {
            _oldTemporaryScore = _newTemporaryScore;
            _newTemporaryScore = value;

            _temporaryScoreTween = _temporaryScoreText.DOCounter(_oldTemporaryScore, _newTemporaryScore, 2f, false, null);
            //_temporaryScoreText.DOCounter(_oldTemporaryScore, _newTemporaryScore, 2f, false, null);

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
            SetMultiplier(1);
        }

        private void MissedMultiplierEvent(int value)
        {
            Debug.Log("Missed a multiplier!");

            for (int i = 0; i < value; i++)
            {
                _streaks[i].SetActive(false);
            }
        }
    }
}
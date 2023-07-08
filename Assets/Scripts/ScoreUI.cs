using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        }
        
        private void AddScore(int value)
        {
            _scoreText.text = "Score: " + value.ToString();
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
            _temporaryScoreText.text = "Temporary Score: " + value.ToString();
        }

        private void StreakEndEvent()
        {
            SetMultiplier(1);
        }

        private void MissedMultiplierEvent(int value)
        {
            for (int i = 0; i < value; i++)
            {
                _streaks[i].SetActive(false);
            }
        }
    }
}
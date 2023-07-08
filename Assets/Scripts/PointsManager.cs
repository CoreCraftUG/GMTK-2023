using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public int TotalPoints;
    public float PointMultiplyer;
    [SerializeField] private int _basePoints;
    [SerializeField] private int _match3Points;
    [SerializeField] private float _multiplierIncrease;
    [SerializeField] private int _maxMissedMultiplies;

    private bool _wasMultiplied;
    private int _missedMultiplies;

    private void Awake()
    {
        EventManager.Instance.MatchingCardsEvent.AddListener(PointMemory);
        EventManager.Instance.TurnEvent.AddListener(Multiply);
    }

    private void Multiply()
    {
        if (!_wasMultiplied)
        {
            if (_multiplierIncrease < _maxMissedMultiplies)
            {
                PointMultiplyer = 1;
                EventManager.Instance.StreakEndEvent.Invoke();
            }
            else
            {
                _missedMultiplies++;
                EventManager.Instance.MissedMultiplyEvent.Invoke(_missedMultiplies);
            }
        }
        _wasMultiplied = false;
    }

    private void PointMemory(bool match3)
    {
        if(match3)
            TotalPoints += Mathf.FloorToInt(_match3Points * PointMultiplyer);
        else
            TotalPoints += Mathf.FloorToInt(_basePoints * PointMultiplyer);

        PointMultiplyer += _multiplierIncrease;
        _wasMultiplied = true;
        _missedMultiplies = 0;
        EventManager.Instance.PointsAddedEvent.Invoke(TotalPoints);
    }

    public void ResetPoints()
    {
        TotalPoints = 0;
    }
}
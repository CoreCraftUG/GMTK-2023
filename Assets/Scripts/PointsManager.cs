using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public int TotalPoints;
    public int TempPoints;
    public float PointMultiplyer;
    [SerializeField] private int _basePoints;
    [SerializeField] private int _match3Points;
    [SerializeField] private float _multiplierIncrease;
    [SerializeField] private int _maxMissedMultiplies;

    private bool _wasMultiplied;
    private int _missedMultiplies;
    private int _turn;
    private int _previousTurn;

    private void Awake()
    {
        EventManager.Instance.MatchingCardsEvent.AddListener(PointMemory);
        EventManager.Instance.TurnEvent.AddListener(TurnEnd);
    }

    private void OnDestroy()
    {
        if (!EventManager.Instance)
            return;

        EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
        EventManager.Instance.TurnEvent.RemoveAllListeners();
    }

    private void OnApplicationQuit()
    {
        if (!EventManager.Instance)
            return;

        EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
        EventManager.Instance.TurnEvent.RemoveAllListeners();
    }

    private void Multiply()
    {
        if (TempPoints<=0)
            return;

        TotalPoints += Mathf.FloorToInt(TempPoints * PointMultiplyer);
        TempPoints = 0;

        EventManager.Instance.PointsAddedEvent.Invoke(TotalPoints);
    }

    private void TurnEnd()
    {
        Multiply();
        if (!_wasMultiplied)
        {
            if (_missedMultiplies >= _maxMissedMultiplies)
            {
                PointMultiplyer = 1;
                EventManager.Instance.MissedMultiplyEvent.Invoke(_missedMultiplies);
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
            TempPoints += _match3Points;
        else
            TempPoints += _basePoints;

        PointMultiplyer += _multiplierIncrease;
        EventManager.Instance.PointMultiplyEvent.Invoke(PointMultiplyer);
        _wasMultiplied = true;
        _missedMultiplies = 0;

        EventManager.Instance.TempPointsEvent.Invoke(TempPoints);
    }

    public void ResetPoints()
    {
        TotalPoints = 0;
    }
}
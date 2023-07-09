using JamCraft.GMTK2023.Code;
using System;
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
        EventManager.Instance.GameOverEvent.AddListener(GameOver);
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
            EventManager.Instance.TurnEvent.RemoveAllListeners();
            EventManager.Instance.GameOverEvent.RemoveAllListeners();
        }
    }

    private void OnApplicationQuit()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
            EventManager.Instance.TurnEvent.RemoveAllListeners();
            EventManager.Instance.GameOverEvent.RemoveAllListeners();
        }
    }

    private void GameOver()
    {
        Multiply();
    }

    private void Multiply()
    {
        if (TempPoints<=0)
            return;

        TotalPoints += Mathf.FloorToInt(TempPoints * PointMultiplyer);
        TempPoints = 0;

        EventManager.Instance.TempPointsEvent.Invoke(TempPoints);
        EventManager.Instance.PointsAddedEvent.Invoke(TotalPoints);
        SoundManager.Instance.PlaySFX(6);
    }

    private void TurnEnd()
    {
        if (!_wasMultiplied)
        {
            if (_missedMultiplies >= _maxMissedMultiplies)
            {
                Multiply();
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
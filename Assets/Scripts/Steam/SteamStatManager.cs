using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SteamStatManager : MonoBehaviour
{
    [SerializeField] private LeaderboardObject _leaderboardObject;
    [SerializeField] private IntStatObject _maxLevelObject;
    [SerializeField] private FloatStatObject _maxMultiplyObject;

    private int _tempScore;

    private void Start()
    {
        EventManager.Instance.FinalScoreEvent.AddListener((int score) =>
        {
            _tempScore = score;
            EventManager.Instance.MaxScoreAchievementEvent.Invoke(_tempScore);

            _leaderboardObject.KeepBestUploadScore(score);

            _leaderboardObject.GetUserEntry(HandleMaxScoreCallback);
        });

        EventManager.Instance.LevelUpEvent.AddListener((int level) =>
        {
            if(_maxLevelObject.GetIntValue() > level)
                EventManager.Instance.MaxLevelAchievementEvent.Invoke(_maxLevelObject.GetIntValue());
            else
            {
                _maxLevelObject.AddIntStat(level);
                EventManager.Instance.MaxLevelAchievementEvent.Invoke(level);
            }
        });

        EventManager.Instance.PointMultiplyEvent.AddListener((float value) =>
        {
            if (_maxMultiplyObject.GetFloatValue() > value)
                EventManager.Instance.MaxMultiplyAchievementEvent.Invoke(_maxMultiplyObject.GetFloatValue());
            else
            {
                _maxMultiplyObject.AddFloatStat(value);
                EventManager.Instance.MaxMultiplyAchievementEvent.Invoke(value);
            }
        });


    }

    void HandleMaxScoreCallback(LeaderboardEntry result, bool IOError)
    {
        if(IOError && result.entry.m_nScore > _tempScore)
        {
            EventManager.Instance.MaxScoreAchievementEvent.Invoke(result.entry.m_nScore);
        }
    }
}
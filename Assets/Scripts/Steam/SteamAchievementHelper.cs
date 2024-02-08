using CoreCraft.Core;
using HeathenEngineering.SteamworksIntegration;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteamAchievementHelper : Singleton<SteamAchievementHelper>
{
    public bool EventManagerReady;

    // Score Achievements
    [FoldoutGroup("Score"), Header("Score 1.000"), SerializeField] private AchievementObject _score1000Achievement;
    [FoldoutGroup("Score"), Header("Score 2.500"), SerializeField] private AchievementObject _score2500Achievement;
    [FoldoutGroup("Score"), Header("Score 10.000"), SerializeField] private AchievementObject _score10000Achievement;
    [FoldoutGroup("Score"), Header("Score 20.000"), SerializeField] private AchievementObject _score20000Achievement;
    [FoldoutGroup("Score"), Header("Score 100.000"), SerializeField] private AchievementObject _score100000Achievement;

    // Multiply Achievements
    [FoldoutGroup("Multiply"), Header("Multiply x2"), SerializeField] private AchievementObject _multiply2;
    [FoldoutGroup("Multiply"), Header("Multiply x3"), SerializeField] private AchievementObject _multiply3;
    [FoldoutGroup("Multiply"), Header("Multiply x4"), SerializeField] private AchievementObject _multiply4;
    [FoldoutGroup("Multiply"), Header("Multiply x5"), SerializeField] private AchievementObject _multiply5;
    [FoldoutGroup("Multiply"), Header("Multiply x10"), SerializeField] private AchievementObject _multiply10;

    // Levele Achievements
    [FoldoutGroup("Level"), Header("Level 5"), SerializeField] private AchievementObject _level5;
    [FoldoutGroup("Level"), Header("Level 10"), SerializeField] private AchievementObject _level10;
    [FoldoutGroup("Level"), Header("Level 15"), SerializeField] private AchievementObject _level15;
    [FoldoutGroup("Level"), Header("Level 20"), SerializeField] private AchievementObject _level20;

    // Row Streak Achievements
    [FoldoutGroup("Row Streak"), Header("Red Row"), SerializeField] private AchievementObject _redRow;
    [FoldoutGroup("Row Streak"), Header("Green Row"), SerializeField] private AchievementObject _greenRow;
    [FoldoutGroup("Row Streak"), Header("Blue Row"), SerializeField] private AchievementObject _blueRow;
    [FoldoutGroup("Row Streak"), Header("Yellow Row"), SerializeField] private AchievementObject _yellowRow;
    [FoldoutGroup("Row Streak"), Header("Purple Row"), SerializeField] private AchievementObject _purpleRow;

    // Clear Row with one Card Achievements
    [FoldoutGroup("Clear Row with one Card"), Header("2 Rows"), SerializeField] private AchievementObject _2Rows;
    [FoldoutGroup("Clear Row with one Card"), Header("3 Rows"), SerializeField] private AchievementObject _3Rows;
    [FoldoutGroup("Clear Row with one Card"), Header("5 Rows"), SerializeField] private AchievementObject _5Rows;

    // Center Symbols Achievements
    [FoldoutGroup("Center Symbols"), Header("Bronze Center"), SerializeField] private AchievementObject _bronzeCenter;
    [FoldoutGroup("Center Symbols"), Header("Silver Center"), SerializeField] private AchievementObject _silverCenter;
    [FoldoutGroup("Center Symbols"), Header("Gold Center"), SerializeField] private AchievementObject _goldCenter;
    [FoldoutGroup("Center Symbols"), Header("Success Center"), SerializeField] private AchievementObject _successCenter;

    [Header("Match Tree"), SerializeField] private AchievementObject _matchTree;

    [Header("Clear Tutorial"), SerializeField] private AchievementObject _tutorialCleared;

    [Header("Deko Slots"), SerializeField] private AchievementObject _decoSlots;

    [Header("Clear Neighbour Row"), SerializeField] private AchievementObject _clearNeighbourRow;

    [Header("Fill Grids"), SerializeField] private AchievementObject _fillGrids;

    [Header("Rim Explosion"), SerializeField] private AchievementObject _rimExplosion;

    private ECardColour? _lastCardColour;
    private int _clearRowStreakCount;
    private List<CardGrid> _fullCardGrids = new List<CardGrid>();
    private int _rimExplosionCounter;


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

#if UNITY_EDITOR
        EventManager.Instance.ClearAllAchievements.AddListener(() =>
        {
            _score1000Achievement.ClearAchievement();
            _score10000Achievement.ClearAchievement();
            _score100000Achievement.ClearAchievement();
            _score2500Achievement.ClearAchievement();
            _score20000Achievement.ClearAchievement();

            _multiply2.ClearAchievement();
            _multiply3.ClearAchievement();
            _multiply4.ClearAchievement();
            _multiply5.ClearAchievement();
            _multiply10.ClearAchievement();

            _level5.ClearAchievement();
            _level10.ClearAchievement();
            _level15.ClearAchievement();
            _level20.ClearAchievement();

            _redRow.ClearAchievement();
            _blueRow.ClearAchievement();
            _greenRow.ClearAchievement();
            _yellowRow.ClearAchievement();
            _purpleRow.ClearAchievement();

            _2Rows.ClearAchievement();
            _3Rows.ClearAchievement();
            _5Rows.ClearAchievement();

            _bronzeCenter.ClearAchievement();
            _silverCenter.ClearAchievement();
            _goldCenter.ClearAchievement();
            _successCenter.ClearAchievement();

            _matchTree.ClearAchievement();

            _tutorialCleared.ClearAchievement();

            _decoSlots.ClearAchievement();

            _clearNeighbourRow.ClearAchievement();

            _fillGrids.ClearAchievement();

            _rimExplosion.ClearAchievement();
        });
#endif

        Debug.Log($"Achievement Event is getting set!");
        EventManager.Instance.MaxScoreAchievementEvent.AddListener((int points) =>
        {
            if (points >= 1000)
                UnlockScore1000();
            if (points >= 2500)
                UnlockScore2500();
            if (points >= 10000)
                UnlockScore10000();
            if (points >= 20000)
                UnlockScore20000();
            if (points >= 100000)
                UnlockScore100000();
        });

        EventManager.Instance.MaxLevelAchievementEvent.AddListener((int level) =>
        {
            if (level >= 5)
                UnlockLevel5();
            if (level >= 10)
                UnlockLevel10();
            if (level >= 15)
                UnlockLevel15();
            if (level >= 20)
                UnlockLevel20();
        });

        EventManager.Instance.MaxMultiplyAchievementEvent.AddListener((float multiplier) =>
        {
            if (multiplier >= 2)
                UnlockMultiply2();
            if (multiplier >= 3)
                UnlockMultiply3();
            if (multiplier >= 4)
                UnlockMultiply4();
            if (multiplier >= 5)
                UnlockMultiply5();
            if (multiplier >= 10)
                UnlockMultiply10();
        });

        EventManager.Instance.TurnEvent.AddListener(() =>
        {
            _clearRowStreakCount = 0;
            _rimExplosionCounter = 0;
        });

        EventManager.Instance.RowStreakAchievementEvent.AddListener((colour) =>
        {
            if(_lastCardColour != null && _lastCardColour.Value == colour)
            {
                UnlockRowStreak(colour);
            }
            _lastCardColour = colour;
        });

        EventManager.Instance.RowStreakAchievementEvent.AddListener((colour) =>
        {
            _clearRowStreakCount++;

            if (_clearRowStreakCount >= 2)
                ClearRowStreak2();
            if (_clearRowStreakCount >= 3)
                ClearRowStreak3();
            if (_clearRowStreakCount >= 5)
                ClearRowStreak5();
        });

        EventManager.Instance.MatchingCardsEvent.AddListener((faceMatch) =>
        {
            if(faceMatch)
                UnlockMatchTree();
        });

        EventManager.Instance.TutorialClearedEvent.AddListener(() =>
        {
            UnlockTutorialCleared();
        });

        EventManager.Instance.StartGameWithDekoAchievementEvent.AddListener((decoCount) =>
        {
            if (decoCount >= 4)
                UnlockDecoSlots();
        });

        EventManager.Instance.GridFullEvent.AddListener((grid) =>
        {
            if(_fullCardGrids != null && !_fullCardGrids.Any(cg => cg == grid))
                _fullCardGrids.Add(grid);

            if (_fullCardGrids.Count >= 4)
                UnlockFillAllGrids();
        });

        EventManager.Instance.GridNoLongerFullEvent.AddListener((grid) =>
        {
            if (_fullCardGrids != null && _fullCardGrids.Any(cg => cg == grid))
                _fullCardGrids.Remove(grid);
        });

        EventManager.Instance.RimExplosionEvent.AddListener((CardGrid grid, int row) =>
        {
            _rimExplosionCounter++;
        });

        EventManager.Instance.RimExplosionExplodedCardsEvent.AddListener((count) =>
        {
            if(count >= 3)
                _rimExplosionCounter++;

            if (_rimExplosionCounter >= 4)
                UnlockRimExplosion();
        });

        EventManager.Instance.MatchFromNeighbourEvent.AddListener(() =>
        {
            UnlockClearNeighbourRow();
        });

        EventManager.Instance.CentreLevelUpEvent.AddListener((level) =>
        {
            switch (level)
            {
                case ECentreGridLevel.None:
                    UnlockBronzeCentre();
                    break;
                case ECentreGridLevel.Bronze:
                    UnlockSilverCentre();
                    break;
                case ECentreGridLevel.Silver:
                    UnlockGoldCentre();
                    break;
                case ECentreGridLevel.Gold:
                    UnlockSuccessCentre();
                    break;
            }
        });
    }

    #region Score
    public void UnlockScore1000()
    {
        if(_score1000Achievement != null && !_score1000Achievement.IsAchieved)
        {
            _score1000Achievement.Unlock();
        }
    }

    public void UnlockScore2500()
    {
        if(_score2500Achievement != null && !_score2500Achievement.IsAchieved)
        {
            _score2500Achievement.Unlock();
        }
    }

    public void UnlockScore10000()
    {
        if(_score10000Achievement != null && !_score10000Achievement.IsAchieved)
        {
            _score10000Achievement.Unlock();
        }
    }

    public void UnlockScore20000()
    {
        if(_score20000Achievement != null && !_score20000Achievement.IsAchieved)
        {
            _score20000Achievement.Unlock();
        }
    }

    public void UnlockScore100000()
    {
        if(_score100000Achievement != null && !_score100000Achievement.IsAchieved)
        {
            _score100000Achievement.Unlock();
        }
    }
    #endregion

    #region Level
    private void UnlockLevel5()
    {
        if (_level5 != null && !_level5.IsAchieved)
        {
            _level5.Unlock();
        }
    }
    private void UnlockLevel10()
    {
        if (_level10 != null && !_level10.IsAchieved)
        {
            _level10.Unlock();
        }
    }
    private void UnlockLevel15()
    {
        if (_level15 != null && !_level15.IsAchieved)
        {
            _level15.Unlock();
        }
    }
    private void UnlockLevel20()
    {
        if (_level20 != null && !_level20.IsAchieved)
        {
            _level20.Unlock();
        }
    }
    #endregion

    #region Multiply
    private void UnlockMultiply2()
    {
        if (_multiply2 != null && !_multiply2.IsAchieved)
        {
            _multiply2.Unlock();
        }
    }
    private void UnlockMultiply3()
    {
        if (_multiply3 != null && !_multiply3.IsAchieved)
        {
            _multiply3.Unlock();
        }
    }
    private void UnlockMultiply4()
    {
        if (_multiply4 != null && !_multiply4.IsAchieved)
        {
            _multiply4.Unlock();
        }
    }
    private void UnlockMultiply5()
    {
        if (_multiply5 != null && !_multiply5.IsAchieved)
        {
            _multiply5.Unlock();
        }
    }
    private void UnlockMultiply10()
    {
        if (_multiply10 != null && !_multiply10.IsAchieved)
        {
            _multiply10.Unlock();
        }
    }
    #endregion

    #region Row Streak
    private void UnlockRowStreak(ECardColour colour)
    {
        switch (colour)
        {
            case ECardColour.Red:
                if(_redRow != null && !_redRow.IsAchieved)
                    _redRow.Unlock();
                break;
            case ECardColour.Green:
                if (_greenRow != null && !_greenRow.IsAchieved)
                    _greenRow.Unlock();
                break;
            case ECardColour.Blue:
                if (_blueRow != null && !_blueRow.IsAchieved)
                    _blueRow.Unlock();
                break;
            case ECardColour.Yellow:
                if (_yellowRow != null && !_yellowRow.IsAchieved)
                    _yellowRow.Unlock();
                break;
            case ECardColour.White:
                if (_purpleRow != null && !_purpleRow.IsAchieved)
                    _purpleRow.Unlock();
                break;
        }
    }
    #endregion

    #region Clear Row Streak
    private void ClearRowStreak2()
    {
        if(_2Rows != null && !_2Rows.IsAchieved)
            _2Rows.Unlock();
    }
    private void ClearRowStreak3()
    {
        if (_3Rows != null && !_3Rows.IsAchieved)
            _3Rows.Unlock();
    }
    private void ClearRowStreak5()
    {
        if (_5Rows != null && !_5Rows.IsAchieved)
            _5Rows.Unlock();
    }
    #endregion

    #region Match Three
    private void UnlockMatchTree()
    {
        if (_matchTree != null && !_matchTree.IsAchieved)
            _matchTree.Unlock();
    }
    #endregion

    #region TutorialCleared
    private void UnlockTutorialCleared()
    {
        if (_tutorialCleared != null && !_tutorialCleared.IsAchieved)
            _tutorialCleared.Unlock();
    }
    #endregion

    #region Deco Slots
    private void UnlockDecoSlots()
    {
        if (_decoSlots != null && !_decoSlots.IsAchieved)
            _decoSlots.Unlock();
    }
    #endregion

    #region Fill All Grids
    private void UnlockFillAllGrids()
    {
        if (_fillGrids != null && !_fillGrids.IsAchieved)
            _fillGrids.Unlock();
    }
    #endregion

    #region Rim Explosion
    private void UnlockRimExplosion()
    {
        if (_rimExplosion != null && !_rimExplosion.IsAchieved)
            _rimExplosion.Unlock();
    }
    #endregion

    #region Clear Neighbour Row
    private void UnlockClearNeighbourRow()
    {
        if (_clearNeighbourRow != null && !_clearNeighbourRow.IsAchieved)
            _clearNeighbourRow.Unlock();
    }
    #endregion

    #region Clear Neighbour Row
    private void UnlockBronzeCentre()
    {
        if (_bronzeCenter != null && !_bronzeCenter.IsAchieved)
            _bronzeCenter.Unlock();
    }
    private void UnlockSilverCentre()
    {
        if (_silverCenter != null && !_silverCenter.IsAchieved)
            _silverCenter.Unlock();
    }
    private void UnlockGoldCentre()
    {
        if (_goldCenter != null && !_goldCenter.IsAchieved)
            _goldCenter.Unlock();
    }
    private void UnlockSuccessCentre()
    {
        if (_successCenter != null && !_successCenter.IsAchieved)
            _successCenter.Unlock();
    }
    #endregion
}
using CoreCraft.Core;
using HeathenEngineering.SteamworksIntegration;
using Sirenix.OdinInspector;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class SteamAchievementHelper : Singleton<SteamAchievementHelper>
{
    public bool EventManagerReady;
    public bool? SteamOnline;

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
    [FoldoutGroup("Clear Row with one Card"), Header("4 Rows"), SerializeField] private AchievementObject _4Rows;

    // Center Symbols Achievements
    [FoldoutGroup("Center Symbols"), Header("Bronze Center"), SerializeField] private AchievementObject _bronzeCenter;
    [FoldoutGroup("Center Symbols"), Header("Silver Center"), SerializeField] private AchievementObject _silverCenter;
    [FoldoutGroup("Center Symbols"), Header("Gold Center"), SerializeField] private AchievementObject _goldCenter;
    [FoldoutGroup("Center Symbols"), Header("Success Center"), SerializeField] private AchievementObject _successCenter;

    [Header("Match Three"), SerializeField] private AchievementObject _matchThree;

    [Header("Clear Tutorial"), SerializeField] private AchievementObject _tutorialCleared;

    [Header("Deko Slots"), SerializeField] private AchievementObject _decoSlots;

    [Header("Clear Neighbour Row"), SerializeField] private AchievementObject _clearNeighbourRow;

    [Header("Fill Grids"), SerializeField] private AchievementObject _fillGrids;

    [Header("Rim Explosion"), SerializeField] private AchievementObject _rimExplosion;

    private ECardColour? _lastCardColour;
    private int _clearRowStreakCount;
    private List<CardGrid> _fullCardGrids = new List<CardGrid>();
    private int _rimExplosionCounter;

    private string _savePath;
    private string _saveKey = "OfflineAchievementsSave"; //DO NOT CHANGE THIS
    private string _encryptionPassword = "CoreCraftsSuperSavePassword"; //DO NOT CHANGE THIS
    private ES3Settings _settings;

    [FoldoutGroup("Offline Save"), SerializeField] private string _saveLocationFolderName;
    [FoldoutGroup("Offline Save"), SerializeField, ReadOnly] private OfflineAchievementSave _offlineAchievementSave = new OfflineAchievementSave();

    public OfflineAchievementSave OfflineAchievementsSave { get { return _offlineAchievementSave; } }


    private void Start()
    {
        _savePath = $"{Application.dataPath}/{_saveLocationFolderName}/{_saveKey}";
        _settings = new ES3Settings() { encryptionType = ES3.EncryptionType.AES, encryptionPassword = _encryptionPassword, compressionType = ES3.CompressionType.Gzip, bufferSize = 250000};
        StartCoroutine(SetUpCoroutine());
    }

    private void OnDestroy()
    {
        ES3.Save(_saveLocationFolderName, _offlineAchievementSave, _savePath, _settings);
    }

    private void OnApplicationQuit()
    {
        ES3.Save(_saveLocationFolderName, _offlineAchievementSave, _savePath, _settings);
    }

    private IEnumerator SetUpCoroutine()
    {
        yield return new WaitUntil(() =>
        {
            return EventManagerReady && SteamOnline != null;
        });

        if (ES3.KeyExists(_saveKey,_savePath,_settings))
            _offlineAchievementSave = ES3.Load<OfflineAchievementSave>(_saveKey,_savePath,_settings);
        else
            _offlineAchievementSave = new OfflineAchievementSave();

        if(SteamOnline != null && SteamOnline.Value == true)
        {
            if(UserData.Me.SteamId != _offlineAchievementSave.UserSteamID)
                _offlineAchievementSave = new OfflineAchievementSave();

            GetAllSteamAchievements();
            UnlockOfflineAchievements();
        }

#if UNITY_EDITOR
        EventManager.Instance.ClearAllAchievements.AddListener(() =>
        {
            if (SteamOnline == null || SteamOnline.Value == false)
            {
                Debug.LogError($"Not connected to Steam could not clear Achievements");
                return;
            }

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
            _4Rows.ClearAchievement();

            _bronzeCenter.ClearAchievement();
            _silverCenter.ClearAchievement();
            _goldCenter.ClearAchievement();
            _successCenter.ClearAchievement();

            _matchThree.ClearAchievement();

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
                UnlockClearRowStreak2();
            if (_clearRowStreakCount >= 3)
                UnlockClearRowStreak3();
            if (_clearRowStreakCount >= 4)
                UnlockClearRowStreak4();
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

        EventManager.Instance.GameOverEvent.AddListener(() =>
        {
            ES3.Save(_saveLocationFolderName, _offlineAchievementSave, _savePath, _settings);
        });
    }

    #region Score
    public void UnlockScore1000()
    {
        _offlineAchievementSave.Score1000Achievement = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if(_score1000Achievement != null && !_score1000Achievement.IsAchieved)
        {
            _score1000Achievement.Unlock();
        }
    }

    public void UnlockScore2500()
    {
        _offlineAchievementSave.Score2500Achievement = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_score2500Achievement != null && !_score2500Achievement.IsAchieved)
        {
            _score2500Achievement.Unlock();
        }
    }

    public void UnlockScore10000()
    {
        _offlineAchievementSave.Score10000Achievement = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_score10000Achievement != null && !_score10000Achievement.IsAchieved)
        {
            _score10000Achievement.Unlock();
        }
    }

    public void UnlockScore20000()
    {
        _offlineAchievementSave.Score20000Achievement = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_score20000Achievement != null && !_score20000Achievement.IsAchieved)
        {
            _score20000Achievement.Unlock();
        }
    }

    public void UnlockScore100000()
    {
        _offlineAchievementSave.Score100000Achievement = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_score100000Achievement != null && !_score100000Achievement.IsAchieved)
        {
            _score100000Achievement.Unlock();
        }
    }
    #endregion

    #region Level
    private void UnlockLevel5()
    {
        _offlineAchievementSave.Level5 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_level5 != null && !_level5.IsAchieved)
        {
            _level5.Unlock();
        }
    }
    private void UnlockLevel10()
    {
        _offlineAchievementSave.Level10 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_level10 != null && !_level10.IsAchieved)
        {
            _level10.Unlock();
        }
    }
    private void UnlockLevel15()
    {
        _offlineAchievementSave.Level15 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_level15 != null && !_level15.IsAchieved)
        {
            _level15.Unlock();
        }
    }
    private void UnlockLevel20()
    {
        _offlineAchievementSave.Level20 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_level20 != null && !_level20.IsAchieved)
        {
            _level20.Unlock();
        }
    }
    #endregion

    #region Multiply
    private void UnlockMultiply2()
    {
        _offlineAchievementSave.Multiply2 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_multiply2 != null && !_multiply2.IsAchieved)
        {
            _multiply2.Unlock();
        }
    }
    private void UnlockMultiply3()
    {
        _offlineAchievementSave.Multiply3 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_multiply3 != null && !_multiply3.IsAchieved)
        {
            _multiply3.Unlock();
        }
    }
    private void UnlockMultiply4()
    {
        _offlineAchievementSave.Multiply4 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_multiply4 != null && !_multiply4.IsAchieved)
        {
            _multiply4.Unlock();
        }
    }
    private void UnlockMultiply5()
    {
        _offlineAchievementSave.Multiply5 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_multiply5 != null && !_multiply5.IsAchieved)
        {
            _multiply5.Unlock();
        }
    }
    private void UnlockMultiply10()
    {
        _offlineAchievementSave.Multiply10 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

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
                _offlineAchievementSave.RedRow = true;

                if (SteamOnline == null || SteamOnline.Value == false)
                    return;

                if (_redRow != null && !_redRow.IsAchieved)
                    _redRow.Unlock();
                break;
            case ECardColour.Green:
                _offlineAchievementSave.GreenRow = true;

                if (SteamOnline == null || SteamOnline.Value == false)
                    return;

                if (_greenRow != null && !_greenRow.IsAchieved)
                    _greenRow.Unlock();
                break;
            case ECardColour.Blue:
                _offlineAchievementSave.BlueRow = true;

                if (SteamOnline == null || SteamOnline.Value == false)
                    return;

                if (_blueRow != null && !_blueRow.IsAchieved)
                    _blueRow.Unlock();
                break;
            case ECardColour.Yellow:
                _offlineAchievementSave.YellowRow = true;

                if (SteamOnline == null || SteamOnline.Value == false)
                    return;

                if (_yellowRow != null && !_yellowRow.IsAchieved)
                    _yellowRow.Unlock();
                break;
            case ECardColour.Purple:
                _offlineAchievementSave.PurpleRow = true;

                if (SteamOnline == null || SteamOnline.Value == false)
                    return;

                if (_purpleRow != null && !_purpleRow.IsAchieved)
                    _purpleRow.Unlock();
                break;
        }
    }
    #endregion

    #region Clear Row Streak
    private void UnlockClearRowStreak2()
    {
        _offlineAchievementSave.Rows2 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_2Rows != null && !_2Rows.IsAchieved)
            _2Rows.Unlock();
    }
    private void UnlockClearRowStreak3()
    {
        _offlineAchievementSave.Rows3 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_3Rows != null && !_3Rows.IsAchieved)
            _3Rows.Unlock();
    }
    private void UnlockClearRowStreak4()
    {
        _offlineAchievementSave.Rows4 = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_4Rows != null && !_4Rows.IsAchieved)
            _4Rows.Unlock();
    }
    #endregion

    #region Match Three
    private void UnlockMatchTree()
    {
        _offlineAchievementSave.MatchTree = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_matchThree != null && !_matchThree.IsAchieved)
            _matchThree.Unlock();
    }
    #endregion

    #region TutorialCleared
    private void UnlockTutorialCleared()
    {
        _offlineAchievementSave.TutorialCleared = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_tutorialCleared != null && !_tutorialCleared.IsAchieved)
            _tutorialCleared.Unlock();
    }
    #endregion

    #region Deco Slots
    private void UnlockDecoSlots()
    {
        _offlineAchievementSave.DecoSlots = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_decoSlots != null && !_decoSlots.IsAchieved)
            _decoSlots.Unlock();
    }
    #endregion

    #region Fill All Grids
    private void UnlockFillAllGrids()
    {
        _offlineAchievementSave.FillGrids = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_fillGrids != null && !_fillGrids.IsAchieved)
            _fillGrids.Unlock();
    }
    #endregion

    #region Rim Explosion
    private void UnlockRimExplosion()
    {
        _offlineAchievementSave.RimExplosion = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_rimExplosion != null && !_rimExplosion.IsAchieved)
            _rimExplosion.Unlock();
    }
    #endregion

    #region Clear Neighbour Row
    private void UnlockClearNeighbourRow()
    {
        _offlineAchievementSave.ClearNeighbourRow = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_clearNeighbourRow != null && !_clearNeighbourRow.IsAchieved)
            _clearNeighbourRow.Unlock();
    }
    #endregion

    #region Centre Level
    private void UnlockBronzeCentre()
    {
        _offlineAchievementSave.BronzeCenter = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_bronzeCenter != null && !_bronzeCenter.IsAchieved)
            _bronzeCenter.Unlock();
    }
    private void UnlockSilverCentre()
    {
        _offlineAchievementSave.SilverCenter = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_silverCenter != null && !_silverCenter.IsAchieved)
            _silverCenter.Unlock();
    }
    private void UnlockGoldCentre()
    {
        _offlineAchievementSave.GoldCenter = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_goldCenter != null && !_goldCenter.IsAchieved)
            _goldCenter.Unlock();
    }
    private void UnlockSuccessCentre()
    {
        _offlineAchievementSave.SuccessCenter = true;

        if (SteamOnline == null || SteamOnline.Value == false)
            return;

        if (_successCenter != null && !_successCenter.IsAchieved)
            _successCenter.Unlock();
    }
    #endregion

    #region Unlock Offline Achievements
    private void UnlockOfflineAchievements()
    {
        // Score Achievements
        if (_offlineAchievementSave.Score1000Achievement)
            UnlockScore1000();
        if (_offlineAchievementSave.Score2500Achievement)
            UnlockScore2500();
        if (_offlineAchievementSave.Score10000Achievement)
            UnlockScore10000();
        if (_offlineAchievementSave.Score20000Achievement)
            UnlockScore20000();
        if (_offlineAchievementSave.Score100000Achievement)
            UnlockScore100000();

        // Multiply Achievements
        if (_offlineAchievementSave.Multiply2)
            UnlockMultiply2();
        if (_offlineAchievementSave.Multiply3)
            UnlockMultiply3();
        if (_offlineAchievementSave.Multiply4)
            UnlockMultiply4();
        if (_offlineAchievementSave.Multiply5)
            UnlockMultiply5();
        if (_offlineAchievementSave.Multiply10)
            UnlockMultiply10();

        // Levele Achievements
        if (_offlineAchievementSave.Level5)
            UnlockLevel5();
        if (_offlineAchievementSave.Level10)
            UnlockLevel10();
        if (_offlineAchievementSave.Level15)
            UnlockLevel15();
        if (_offlineAchievementSave.Level20)
            UnlockLevel20();

        // Row Streak Achievements
        if (_offlineAchievementSave.RedRow)
            UnlockRowStreak(ECardColour.Red);
        if (_offlineAchievementSave.GreenRow)
            UnlockRowStreak(ECardColour.Green);
        if (_offlineAchievementSave.BlueRow)
            UnlockRowStreak(ECardColour.Blue);
        if (_offlineAchievementSave.YellowRow)
            UnlockRowStreak(ECardColour.Yellow);
        if (_offlineAchievementSave.PurpleRow)
            UnlockRowStreak(ECardColour.Purple);

        // Clear Row with one Card Achievements
        if (_offlineAchievementSave.Rows2)
            UnlockClearRowStreak2();
        if (_offlineAchievementSave.Rows3)
            UnlockClearRowStreak3();
        if (_offlineAchievementSave.Rows4)
            UnlockClearRowStreak4();

        // Center Symbols Achievements
        if (_offlineAchievementSave.BronzeCenter)
            UnlockBronzeCentre();
        if (_offlineAchievementSave.SilverCenter)
            UnlockSilverCentre();
        if (_offlineAchievementSave.GoldCenter)
            UnlockGoldCentre();
        if (_offlineAchievementSave.SuccessCenter)
            UnlockSuccessCentre();

        if (_offlineAchievementSave.MatchTree)
            UnlockMatchTree();

        if (_offlineAchievementSave.TutorialCleared)
            UnlockTutorialCleared();

        if (_offlineAchievementSave.DecoSlots)
            UnlockDecoSlots();

        if (_offlineAchievementSave.ClearNeighbourRow)
            UnlockClearNeighbourRow();

        if (_offlineAchievementSave.FillGrids)
            UnlockFillAllGrids();

        if (_offlineAchievementSave.RimExplosion)
            UnlockRimExplosion();

        ES3.Save(_saveLocationFolderName, _offlineAchievementSave, _savePath, _settings);
    }
    #endregion

    #region Get All Steam Achievements
    private void GetAllSteamAchievements()
    {
        _offlineAchievementSave.UserSteamID = UserData.Me.SteamId;

        // Score Achievements
        _offlineAchievementSave.Score1000Achievement    = _score1000Achievement.IsAchieved      ? true : _offlineAchievementSave.Score1000Achievement;
        _offlineAchievementSave.Score2500Achievement    = _score2500Achievement.IsAchieved      ? true : _offlineAchievementSave.Score2500Achievement;
        _offlineAchievementSave.Score10000Achievement   = _score10000Achievement.IsAchieved     ? true : _offlineAchievementSave.Score10000Achievement;
        _offlineAchievementSave.Score20000Achievement   = _score20000Achievement.IsAchieved     ? true : _offlineAchievementSave.Score20000Achievement;
        _offlineAchievementSave.Score100000Achievement  = _score100000Achievement.IsAchieved    ? true : _offlineAchievementSave.Score100000Achievement;

        // Multiply Achievements
        _offlineAchievementSave.Multiply2               = _multiply2.IsAchieved                 ? true : _offlineAchievementSave.Multiply2;
        _offlineAchievementSave.Multiply3               = _multiply3.IsAchieved                 ? true : _offlineAchievementSave.Multiply3;
        _offlineAchievementSave.Multiply4               = _multiply4.IsAchieved                 ? true : _offlineAchievementSave.Multiply4;
        _offlineAchievementSave.Multiply5               = _multiply5.IsAchieved                 ? true : _offlineAchievementSave.Multiply5;
        _offlineAchievementSave.Multiply10              = _multiply10.IsAchieved                ? true : _offlineAchievementSave.Multiply10;

        // Levele Achievements
        _offlineAchievementSave.Level5                  = _level5.IsAchieved                    ? true : _offlineAchievementSave.Level5;
        _offlineAchievementSave.Level10                 = _level10.IsAchieved                   ? true : _offlineAchievementSave.Level10;
        _offlineAchievementSave.Level15                 = _level15.IsAchieved                   ? true : _offlineAchievementSave.Level15;
        _offlineAchievementSave.Level20                 = _level20.IsAchieved                   ? true : _offlineAchievementSave.Level20;

        // Row Streak Achievements
        _offlineAchievementSave.RedRow                  = _redRow.IsAchieved                    ? true : _offlineAchievementSave.RedRow;
        _offlineAchievementSave.GreenRow                = _greenRow.IsAchieved                  ? true : _offlineAchievementSave.GreenRow;
        _offlineAchievementSave.BlueRow                 = _blueRow.IsAchieved                   ? true : _offlineAchievementSave.BlueRow;
        _offlineAchievementSave.YellowRow               = _yellowRow.IsAchieved                 ? true : _offlineAchievementSave.YellowRow;
        _offlineAchievementSave.PurpleRow               = _purpleRow.IsAchieved                 ? true : _offlineAchievementSave.PurpleRow;

        // Clear Row with one Card Achievements
        _offlineAchievementSave.Rows2                   = _2Rows.IsAchieved                     ? true : _offlineAchievementSave.Rows2;
        _offlineAchievementSave.Rows3                   = _3Rows.IsAchieved                     ? true : _offlineAchievementSave.Rows3;
        _offlineAchievementSave.Rows4                   = _4Rows.IsAchieved                     ? true : _offlineAchievementSave.Rows4;

        // Center Symbols Achievements
        _offlineAchievementSave.BronzeCenter            = _bronzeCenter.IsAchieved              ? true : _offlineAchievementSave.BronzeCenter;
        _offlineAchievementSave.SilverCenter            = _silverCenter.IsAchieved              ? true : _offlineAchievementSave.SilverCenter;
        _offlineAchievementSave.GoldCenter              = _goldCenter.IsAchieved                ? true : _offlineAchievementSave.GoldCenter;
        _offlineAchievementSave.SuccessCenter           = _successCenter.IsAchieved             ? true : _offlineAchievementSave.SuccessCenter;

        _offlineAchievementSave.MatchTree               = _matchThree.IsAchieved                 ? true : _offlineAchievementSave.MatchTree;

        _offlineAchievementSave.TutorialCleared         = _tutorialCleared.IsAchieved           ? true : _offlineAchievementSave.TutorialCleared;

        _offlineAchievementSave.DecoSlots               = _decoSlots.IsAchieved                 ? true : _offlineAchievementSave.DecoSlots;

        _offlineAchievementSave.ClearNeighbourRow       = _clearNeighbourRow.IsAchieved         ? true : _offlineAchievementSave.ClearNeighbourRow;

        _offlineAchievementSave.FillGrids               = _fillGrids.IsAchieved                 ? true : _offlineAchievementSave.FillGrids;

        _offlineAchievementSave.RimExplosion            = _rimExplosion.IsAchieved              ? true : _offlineAchievementSave.RimExplosion;
    }
    #endregion

    [Serializable]
    public struct OfflineAchievementSave
    {
        [Header("Steam ID"), SerializeField] public ulong UserSteamID;

        // Score Achievements
        [FoldoutGroup("Score"), Header("Score 1.000"), SerializeField] public bool Score1000Achievement;
        [FoldoutGroup("Score"), Header("Score 2.500"), SerializeField] public bool Score2500Achievement;
        [FoldoutGroup("Score"), Header("Score 10.000"), SerializeField] public bool Score10000Achievement;
        [FoldoutGroup("Score"), Header("Score 20.000"), SerializeField] public bool Score20000Achievement;
        [FoldoutGroup("Score"), Header("Score 100.000"), SerializeField] public bool Score100000Achievement;

        // Multiply Achievements
        [FoldoutGroup("Multiply"), Header("Multiply x2"), SerializeField] public bool Multiply2;
        [FoldoutGroup("Multiply"), Header("Multiply x3"), SerializeField] public bool Multiply3;
        [FoldoutGroup("Multiply"), Header("Multiply x4"), SerializeField] public bool Multiply4;
        [FoldoutGroup("Multiply"), Header("Multiply x5"), SerializeField] public bool Multiply5;
        [FoldoutGroup("Multiply"), Header("Multiply x10"), SerializeField] public bool Multiply10;

        // Levele Achievements
        [FoldoutGroup("Level"), Header("Level 5"), SerializeField] public bool Level5;
        [FoldoutGroup("Level"), Header("Level 10"), SerializeField] public bool Level10;
        [FoldoutGroup("Level"), Header("Level 15"), SerializeField] public bool Level15;
        [FoldoutGroup("Level"), Header("Level 20"), SerializeField] public bool Level20;

        // Row Streak Achievements
        [FoldoutGroup("Row Streak"), Header("Red Row"), SerializeField] public bool RedRow;
        [FoldoutGroup("Row Streak"), Header("Green Row"), SerializeField] public bool GreenRow;
        [FoldoutGroup("Row Streak"), Header("Blue Row"), SerializeField] public bool BlueRow;
        [FoldoutGroup("Row Streak"), Header("Yellow Row"), SerializeField] public bool YellowRow;
        [FoldoutGroup("Row Streak"), Header("Purple Row"), SerializeField] public bool PurpleRow;

        // Clear Row with one Card Achievements
        [FoldoutGroup("Clear Row with one Card"), Header("2 Rows"), SerializeField] public bool Rows2;
        [FoldoutGroup("Clear Row with one Card"), Header("3 Rows"), SerializeField] public bool Rows3;
        [FoldoutGroup("Clear Row with one Card"), Header("5 Rows"), SerializeField] public bool Rows4;

        // Center Symbols Achievements
        [FoldoutGroup("Center Symbols"), Header("Bronze Center"), SerializeField] public bool BronzeCenter;
        [FoldoutGroup("Center Symbols"), Header("Silver Center"), SerializeField] public bool SilverCenter;
        [FoldoutGroup("Center Symbols"), Header("Gold Center"), SerializeField] public bool GoldCenter;
        [FoldoutGroup("Center Symbols"), Header("Success Center"), SerializeField] public bool SuccessCenter;

        [Header("Match Tree"), SerializeField] public bool MatchTree;

        [Header("Clear Tutorial"), SerializeField] public bool TutorialCleared;

        [Header("Deko Slots"), SerializeField] public bool DecoSlots;

        [Header("Clear Neighbour Row"), SerializeField] public bool ClearNeighbourRow;

        [Header("Fill Grids"), SerializeField] public bool FillGrids;

        [Header("Rim Explosion"), SerializeField] public bool RimExplosion;
    }
}
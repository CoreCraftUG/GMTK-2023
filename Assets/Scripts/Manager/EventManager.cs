using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreCraft.Core;
using UnityEngine.Events;
using UnityEditor;

public class EventManager : Singleton<EventManager>
{
    public UnityEvent<bool> MatchingCardsEvent = new UnityEvent<bool>();
    public UnityEvent GameOverEvent = new UnityEvent();
    public UnityEvent TurnEvent = new UnityEvent();
    public UnityEvent<int> PointsAddedEvent = new UnityEvent<int>();
    public UnityEvent OnPointsChanged = new UnityEvent();
    public UnityEvent<int> TempPointsEvent = new UnityEvent<int>();
    public UnityEvent<float> PointMultiplyEvent = new UnityEvent<float>();
    public UnityEvent<int, float> PlayAudio = new UnityEvent<int, float>();
    public UnityEvent TimeStartEvent = new UnityEvent();
    public UnityEvent TimeStopEvent = new UnityEvent();
    public UnityEvent<int> MissedMultiplyEvent = new UnityEvent<int>(); 
    public UnityEvent StreakEndEvent = new UnityEvent(); 
    public UnityEvent TurnRightEvent = new UnityEvent(); 
    public UnityEvent TurnLeftEvent = new UnityEvent(); 
    public UnityEvent<int> LevelUpEvent = new UnityEvent<int>(); 
    public UnityEvent<int> FinalScoreEvent = new UnityEvent<int>();
    public UnityEvent<int> MaxLevelAchievementEvent = new UnityEvent<int>();
    public UnityEvent<int> MaxScoreAchievementEvent = new UnityEvent<int>();
    public UnityEvent<float> MaxMultiplyAchievementEvent = new UnityEvent<float>();
    public UnityEvent<ECardColour> RowStreakAchievementEvent = new UnityEvent<ECardColour>();
    public UnityEvent TutorialClearedEvent = new UnityEvent();                                  //TODO: needs to be called
    public UnityEvent<int> StartGameWithDekoAchievementEvent = new UnityEvent<int>();           //TODO: needs to be called
    public UnityEvent<CardGrid> GridFullEvent = new UnityEvent<CardGrid>(); 
    public UnityEvent<CardGrid> GridNoLongerFullEvent = new UnityEvent<CardGrid>();
    public UnityEvent<ECentreGridLevel> CentreLevelUpEvent = new UnityEvent<ECentreGridLevel>();
    public UnityEvent RimExplosionCardDeletedEvent = new UnityEvent();
    public UnityEvent<CardGrid,int> RimExplosionEvent = new UnityEvent<CardGrid, int>();
    public UnityEvent<int> RimExplosionExplodedCardsEvent = new UnityEvent<int>();
    public UnityEvent MatchFromNeighbourEvent = new UnityEvent();
    public UnityEvent OnGameOptionsUIInitialized = new UnityEvent();

    //Tutorial Message
    public UnityEvent<bool> TutorialMessage01Event = new UnityEvent<bool>();
    public UnityEvent<bool> TutorialMessage02Event = new UnityEvent<bool>();
    public UnityEvent<bool> TutorialMessage03Event = new UnityEvent<bool>();
    public UnityEvent<bool> TutorialMessage04Event = new UnityEvent<bool>();
    public UnityEvent<bool> TutorialMessage05Event = new UnityEvent<bool>();
    public UnityEvent<bool> TutorialMessage06Event = new UnityEvent<bool>();
    public UnityEvent<bool> TutorialMessage07Event = new UnityEvent<bool>();
    public UnityEvent<bool> TutorialMessage08Event = new UnityEvent<bool>();
    public UnityEvent<bool> TutorialMessage09Event = new UnityEvent<bool>();
    public UnityEvent<bool> TutorialMessage10Event = new UnityEvent<bool>();

#if UNITY_EDITOR
    public UnityEvent ClearAllAchievements = new UnityEvent();
#endif
}
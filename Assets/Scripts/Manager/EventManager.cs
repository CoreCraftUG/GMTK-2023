using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreCraft.Core;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityEvent<bool> MatchingCardsEvent = new UnityEvent<bool>();
    public UnityEvent GameOverEvent = new UnityEvent();
    public UnityEvent TurnEvent = new UnityEvent();
    public UnityEvent<int> PointsAddedEvent = new UnityEvent<int>();
    public UnityEvent<int, float> PlayAudio = new UnityEvent<int, float>();
    public UnityEvent TimeStartEvent = new UnityEvent();
    public UnityEvent TimeStopEvent = new UnityEvent();
    public UnityEvent<int> MissedMultiplyEvent = new UnityEvent<int>(); 
    public UnityEvent StreakEndEvent = new UnityEvent(); 
}
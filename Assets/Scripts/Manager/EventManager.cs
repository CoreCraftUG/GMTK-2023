using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreCraft.Core;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityEvent<bool> MatchingCardsEvent = new UnityEvent<bool>();
}
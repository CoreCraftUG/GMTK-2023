using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreCraft.Core;

public class TimeManager : Singleton<TimeManager>
{
    private void Start()
    {
        TimeStop = false;
        EventManager.Instance.TimeStartEvent.AddListener(StartTime);       
        EventManager.Instance.TimeStopEvent.AddListener(StopTime);       
    }

    public bool TimeStop { get; internal set; }

    private void StartTime() => TimeStop = false;

    private void StopTime() => TimeStop = true;
}
using HeathenEngineering.SteamworksIntegration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashHandler : MonoBehaviour
{
    void Start()
    {
    }

    public void HandleSteamError()
    {
        Application.Quit();
    }
}
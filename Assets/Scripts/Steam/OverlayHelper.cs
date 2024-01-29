using CoreCraft.Core;
using HeathenEngineering.SteamworksIntegration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayHelper : Singleton<OverlayHelper>
{
    private OverlayManager _overlayManager => GetComponent<OverlayManager>();

    public OverlayManager GetOverlayManager() => _overlayManager;
}

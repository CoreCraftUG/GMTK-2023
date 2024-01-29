using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    private Button _button => GetComponent<Button>();
    private LeaderboardEntryUIRecord _entryRecord => GetComponent<LeaderboardEntryUIRecord>();
    

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            OverlayHelper.Instance.GetOverlayManager().OpenUser(FriendDialog.steamid,_entryRecord.Entry.User);
        });
    }
}
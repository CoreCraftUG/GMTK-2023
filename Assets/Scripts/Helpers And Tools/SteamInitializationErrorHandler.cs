using CoreCraft.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamInitializationErrorHandler : Singleton<SteamInitializationErrorHandler>
{
    [SerializeField] private Button _proceedOfflineButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Transform _panel;

    private bool _proceedOffline;

    private void Start()
    {
        _proceedOfflineButton.onClick.AddListener(() =>
        {
            _proceedOffline = true;
        });

        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public IEnumerator StartHasInitializationError()
    {
        _panel.gameObject.SetActive(true);

        yield return new WaitUntil(() => _proceedOffline);

        _panel.gameObject.SetActive(false);
    }
}
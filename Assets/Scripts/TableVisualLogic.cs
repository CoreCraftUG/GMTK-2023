using System.Collections;
using System.Collections.Generic;
using JamCraft.GMTK2023.Code;
using UnityEngine;

public class TableVisualLogic : MonoBehaviour
{
    [SerializeField] private GameObject _baseTop;
    [SerializeField] private GameObject _baseFrame;
    [SerializeField] private Transform _tableSpawnTransform;
    [SerializeField] private GameObject _middleGridDecalProjector;
    [SerializeField] private GameObject _middleGridDecals;

    private TableVisual _tableVisuals;

    private void Awake()
    {
        GameStateManager.Instance.OnGamePaused += Instance_OnGamePaused;
        GameStateManager.Instance.OnGameUnpaused += Instance_OnGameUnpaused;
    }

    private void Instance_OnGameUnpaused(object sender, System.EventArgs e)
    {
        _middleGridDecalProjector.SetActive(true);

        if (_middleGridDecals != null)
        {
            _middleGridDecals.SetActive(true);
        }
    }

    private void Instance_OnGamePaused(object sender, System.EventArgs e)
    {
        _middleGridDecalProjector.SetActive(false);

        if (_middleGridDecals != null)
        {
            _middleGridDecals.SetActive(false);
        }
    }

    void Start()
    {
        if(TableVisualManager.Instance == null || TableVisualManager.Instance.TableVisuals.TableTop == null || TableVisualManager.Instance.TableVisuals.TableFrame == null)
        {
            _baseTop.SetActive(true);
            _baseFrame.SetActive(true);
            return;
        }
        else
        {
            _baseTop.SetActive(false);
            _baseFrame.SetActive(false);
        }

        _tableVisuals = TableVisualManager.Instance.TableVisuals;

        Instantiate(_tableVisuals.TableTop, _tableSpawnTransform);
        Instantiate(_tableVisuals.TableFrame, _tableSpawnTransform);
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGamePaused -= Instance_OnGamePaused;
            GameStateManager.Instance.OnGameUnpaused -= Instance_OnGameUnpaused;
        }
    }

    private void OnApplicationQuit()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGamePaused -= Instance_OnGamePaused;
            GameStateManager.Instance.OnGameUnpaused -= Instance_OnGameUnpaused;
        }
    }
}
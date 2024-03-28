using System;
using JamCraft.GMTK2023.Code;
using UnityEngine;

public class TableVisualLogic : MonoBehaviour
{
    [SerializeField] private GameObject _baseTop;
    [SerializeField] private GameObject _baseFrame;
    [SerializeField] private Transform _tableSpawnTransform;
    [SerializeField] private GameObject _middleGridDecalProjector;
    [SerializeField] private GameObject _middleGridDecals;

    public GameObject TableTop;
    public GameObject TableFrame;

    private TableVisual _tableVisuals;
    private bool _middleGridDecalProjectorActiveOnStart;

    private TableTopSwitch _topSwitch;

    private void Awake()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGamePaused += Instance_OnGamePaused;
            GameStateManager.Instance.OnGameUnpaused += Instance_OnGameUnpaused;
        }

        //_middleGridDecalProjectorActiveOnStart = _middleGridDecalProjector.active;

        if (TableVisualManager.Instance != null)
        { 
            TableVisualManager.Instance.OnTableVisualsUpdated += TableVisualManager_OnTableVisualsUpdated;
        }
    }

    private void TableVisualManager_OnTableVisualsUpdated(object sender, EventArgs e)
    {
        if (TableVisualManager.Instance == null || TableVisualManager.Instance.TableVisuals.TableTop == null || TableVisualManager.Instance.TableVisuals.TableFrame == null)
        {
            _baseTop?.SetActive(true);
            _baseFrame?.SetActive(true);
            return;
        }
        else
        {
            _tableVisuals = TableVisualManager.Instance.TableVisuals;

            _baseTop?.SetActive(false);
            _baseFrame?.SetActive(false);

            Destroy(_baseTop);
            Destroy(_baseFrame);

            _baseTop = null;
            _baseFrame = null;

            Destroy(TableTop);
            Destroy(TableFrame);

            TableTop = Instantiate(_tableVisuals.TableTop, _tableSpawnTransform);
            TableFrame = Instantiate(_tableVisuals.TableFrame, _tableSpawnTransform);
        }
    }

    private void Instance_OnGameUnpaused(object sender, System.EventArgs e)
    {
        if (TableTop != null && TableTop.TryGetComponent<TableTopSwitch>(out _topSwitch))
        {
            _topSwitch.CenterfieldSwitch(_middleGridDecalProjectorActiveOnStart);
        }
        else
        {
            _middleGridDecalProjector.SetActive(_middleGridDecalProjectorActiveOnStart);
        }

        if (_middleGridDecals != null)
        {
            _middleGridDecals.SetActive(_middleGridDecalProjectorActiveOnStart);
        }
    }

    private void Instance_OnGamePaused(object sender, System.EventArgs e)
    {
        if (TableTop != null && TableTop.TryGetComponent<TableTopSwitch>(out _topSwitch))
        {
            _topSwitch.CenterfieldSwitch(false);
        }
        else
        {
            _middleGridDecalProjector.SetActive(false);
        }

        if (_middleGridDecals != null)
        {
            _middleGridDecals.SetActive(false);
        }
    }

    private void Start()
    {
        if (TableVisualManager.Instance != null)
        {
            _tableVisuals = TableVisualManager.Instance.TableVisuals;
        }

        TableVisualManager_OnTableVisualsUpdated(this, EventArgs.Empty);
        TableVisualManager.Instance.TableVisuals.TableTop.SetActive(false);
        TableVisualManager.Instance.TableVisuals.TableFrame.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGamePaused -= Instance_OnGamePaused;
            GameStateManager.Instance.OnGameUnpaused -= Instance_OnGameUnpaused;
        }

        if (TableVisualManager.Instance != null)
        {
            TableVisualManager.Instance.OnTableVisualsUpdated -= TableVisualManager_OnTableVisualsUpdated;
        }
    }

    private void OnApplicationQuit()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGamePaused -= Instance_OnGamePaused;
            GameStateManager.Instance.OnGameUnpaused -= Instance_OnGameUnpaused;
        }

        if (TableVisualManager.Instance != null)
        {
            TableVisualManager.Instance.OnTableVisualsUpdated -= TableVisualManager_OnTableVisualsUpdated;
        }
    }
}
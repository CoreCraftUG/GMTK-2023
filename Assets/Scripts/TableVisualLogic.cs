using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableVisualLogic : MonoBehaviour
{
    [SerializeField] private GameObject _baseTop;
    [SerializeField] private GameObject _baseFrame;
    [SerializeField] private Transform _tableSpawnTransform;

    private TableVisual _tableVisuals;

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
}
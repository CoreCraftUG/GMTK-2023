using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject _coinVFXObject;
    [SerializeField] private Transform _coinVFXSpawnTransform;

    private void Start()
    {
        EventManager.Instance.PointsAddedEvent.AddListener((int i) =>
        {
            Instantiate(_coinVFXObject, _coinVFXSpawnTransform);
        });
        EventManager.Instance.MatchingCardsEvent.AddListener((bool b) =>
        {
            if (b)
                Instantiate(_coinVFXObject, _coinVFXSpawnTransform);
        });
    }

    private void OnDestroy()
    {
        EventManager.Instance.PointsAddedEvent.RemoveAllListeners();
        EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
    }

    private void OnApplicationQuit()
    {
        EventManager.Instance.PointsAddedEvent.RemoveAllListeners();
        EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
    }
}
using JamCraft.GMTK2023.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject _coinVFXObject;
    [SerializeField] private GameObject _addTempPointsVFXObject;
    [SerializeField] private GameObject _coinMatch3VFXObject;
    [SerializeField] private Transform _coinVFXSpawnTransform;
    [SerializeField] private Transform _primedExplosionFinalPos;
    [SerializeField] private GameObject _primedExplposionFinal;
    [SerializeField] private GameObject _primedExplposionFinalInner;
    [SerializeField] private GameObject _primedExplposionFinalMiddle;
    [SerializeField] private GameObject _primedExplposionFinalOuter;


    private void Start()
    {
        EventManager.Instance.PointsAddedEvent.AddListener((int i) =>
        {
            Instantiate(_addTempPointsVFXObject, _coinVFXSpawnTransform);
        });
        EventManager.Instance.MatchingCardsEvent.AddListener((bool b) =>
        {
            if (b)
            {
                //Destroy(Instantiate(_primedExplposionFinal, _primedExplosionFinalPos), .5f);
                //Destroy(Instantiate(_primedExplposionFinalInner, _primedExplosionFinalPos), .5f);
                Instantiate(_coinMatch3VFXObject, _coinVFXSpawnTransform);
                SoundManager.Instance.PlaySFX(5);
            }
            else
            {
                Instantiate(_coinMatch3VFXObject, _coinVFXSpawnTransform);
                SoundManager.Instance.PlaySFX(5);
            }
        });
        EventManager.Instance.RimExplosionEvent.AddListener((CardGrid grid, int i) =>
        {
            switch (i)
            {
                case 0:
                    Destroy(Instantiate(_primedExplposionFinalOuter, _primedExplosionFinalPos), .5f);
                    break;
                case 1:
                    Destroy(Instantiate(_primedExplposionFinalMiddle, _primedExplosionFinalPos), .5f);
                    break;
                case 2:
                    Destroy(Instantiate(_primedExplposionFinalInner, _primedExplosionFinalPos), .5f);
                    break;
            }
        });
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.PointsAddedEvent.RemoveAllListeners();
            EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
        }
    }

    private void OnApplicationQuit()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.PointsAddedEvent.RemoveAllListeners();
            EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
        }
    }
}
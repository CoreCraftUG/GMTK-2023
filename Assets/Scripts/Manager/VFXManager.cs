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
                Destroy(Instantiate(_primedExplposionFinal, _primedExplosionFinalPos), _primedExplposionFinal.GetComponent<ParticleSystem>().main.duration);
                Destroy(Instantiate(_primedExplposionFinalInner, _primedExplosionFinalPos), _primedExplposionFinalInner.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                SoundManager.Instance.PlaySFX(5);
            }
            else
            {
                Instantiate(_coinMatch3VFXObject, _coinVFXSpawnTransform);
                SoundManager.Instance.PlaySFX(5);
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
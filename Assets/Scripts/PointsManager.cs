using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public int TotalPoints;
    public float PointMultiplyer;
    [SerializeField] int BasePoints;
    private bool _wasMultiplied;
    [SerializeField] private float _multiplierIncrease;

    private void Awake()
    {
        EventManager.Instance.MatchingCardsEvent.AddListener(PointMemory);
        EventManager.Instance.TurnEvent.AddListener(Multiply);
    }

    private void Multiply()
    {
        if (!_wasMultiplied)
        {
            PointMultiplyer = 1;
        }
        _wasMultiplied = false;
    }

    private void PointMemory(bool points)
    {      
        TotalPoints += Mathf.FloorToInt(BasePoints * PointMultiplyer);
        PointMultiplyer += _multiplierIncrease;
        _wasMultiplied = true;
        EventManager.Instance.PointsAddedEvent.Invoke(TotalPoints);
    }
    public void ResetPoints()
    {
        TotalPoints = 0;
    }
}

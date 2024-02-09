using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace JamCraft.GMTK2023.Code
{
    public class PointManager : MonoBehaviour
    {
        [SerializeField] private List<UI_PointsDisplay> _pointsDisplays = new List<UI_PointsDisplay>();

        [SerializeField] private int _currentPoints;

        private int _tmpPoint;
        private int _tmpCurrentPoints;

        [SerializeField] private PointsManager _pointsManager;

        private void Awake()
        {
            _pointsDisplays.Reverse();
            EventManager.Instance.OnPointsChanged.AddListener(OnPointsChanged);
        }

        private void Start()
        {
            
        }

        private void OnPointsChanged()
        {
            _currentPoints = _pointsManager.TotalPoints;
            _tmpCurrentPoints = _pointsManager.TotalPoints;

            foreach (UI_PointsDisplay pointsDisplay in _pointsDisplays)
            {
                pointsDisplay.AssignPoint(GetPoint());
            }
        }

        [Button("Add Points")]
        private void AddPoints()
        {
            _currentPoints += 100;

            //OnPointsChangedEvent.Invoke();
        }

        private UI_PointsDisplay.Points GetPoint()
        {
            _tmpPoint = _tmpCurrentPoints % 10;

            _tmpCurrentPoints /= 10;

            return (UI_PointsDisplay.Points)_tmpPoint;
        }
    }
}
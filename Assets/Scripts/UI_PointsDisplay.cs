using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
    [ExecuteInEditMode]
    public class UI_PointsDisplay : MonoBehaviour
    {
        public enum Points
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine
        }

        private Dictionary<Points, Vector3> _pointsToVector3 = new Dictionary<Points, Vector3>()
        {
            { Points.Zero, new Vector3(0, 90, 75) },
            { Points.One, new Vector3(0, 90, 110.75f) },
            { Points.Two, new Vector3(0, 90, 146.75f) },
            { Points.Three, new Vector3(0, 90, 182.5f) },
            { Points.Four, new Vector3(0, 90, 218.75f) },
            { Points.Five, new Vector3(0, 90, 254.5f) },
            { Points.Six, new Vector3(0, 90, 290.25f) },
            { Points.Seven, new Vector3(0, 90, 326.75f) },
            { Points.Eight, new Vector3(0, 90, 362.25f) },
            { Points.Nine, new Vector3(0, 90, 398.25f) }
        };

        [SerializeField] private Points _point;

        private Transform _transform => GetComponent<Transform>();
        private Vector3 _defaultRotation = new Vector3(0, 90, 75);

        private void RotateDisplay()
        {
            // InElastic / OutElastic / InOutElastic / OutBounce good
            _transform.DOLocalRotate(new Vector3(0, 0, 360), .5f, RotateMode.LocalAxisAdd).SetLoops(3, LoopType.Restart).SetEase(Ease.Linear).OnComplete(() => _transform.DOLocalRotate(_pointsToVector3[_point], .5f, RotateMode.FastBeyond360).SetEase(Ease.OutBounce));
        }

        [Button("Reset to default")]
        private void Reset()
        {
            Debug.Log("Resetting to default!");
            _transform.DOLocalRotate(_defaultRotation, .5f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad);
        }

        public void AssignPoint(Points point)
        {
            _point = point;
            RotateDisplay();
        }
    }
}
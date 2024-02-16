using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
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

        private readonly Dictionary<Points, Vector3> _pointsToVector3 = new Dictionary<Points, Vector3>()
        {
            { Points.Zero, new Vector3(360, 0, 180) },
            { Points.One, new Vector3(321.861f, 0, 180) },
            { Points.Two, new Vector3(287.251f, 0, 180) },
            { Points.Three, new Vector3(253.632f, 0, 180) },
            { Points.Four, new Vector3(216.634f, 0, 180) },
            { Points.Five, new Vector3(180.401f, 0, 180) },
            { Points.Six, new Vector3(144.139f, 0, 180) },
            { Points.Seven, new Vector3(109.792f, 0, 180) },
            { Points.Eight, new Vector3(72.815f, 0, 180) },
            { Points.Nine, new Vector3(36.407f, 0, 180) }
        };

        [SerializeField] private Points _point;

        private Transform _transform => GetComponent<Transform>();
        private Material _material => GetComponent<Renderer>().material;

        private readonly Vector3 _defaultRotation = new Vector3(0, 0, 180);

        private void RotateDisplay()
        {
            // InElastic / OutElastic / InOutElastic / OutBounce good
            _transform.DOLocalRotate(new Vector3(360, 0, 0), 1f, RotateMode.LocalAxisAdd).SetLoops(2, LoopType.Restart)
                .SetEase(Ease.Linear)
                .OnStart(() => _material.SetFloat("_InMovement", 1))
                .OnStepComplete(RotateDisplayOnStepCompleteCallback)
                .OnComplete(RotateDisplayOnCompleteCallback);
        }

        private void RotateDisplayOnStepCompleteCallback()
        {
            _material.SetFloat("_InMovementFast", 1);
            _material.SetFloat("_InMovement", 0);
        }

        private void RotateDisplayOnCompleteCallback()
        {
            _material.SetFloat("_InMovement", 0);
            _material.SetFloat("_InMovementFast", 0);
            _transform.DOLocalRotate(_pointsToVector3[_point], 1f).SetEase(Ease.OutBounce);
        }

        [Button("Reset to default")]
        private void Reset()
        {
            Debug.Log("Resetting to default!");
            _transform.DOLocalRotate(_defaultRotation, 1f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad);
        }

        public void AssignPoint(Points point)
        {
            _point = point;
            RotateDisplay();
        }
    }
}
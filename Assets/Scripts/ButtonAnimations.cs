using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JamCraft.GMTK2023.Code
{
    public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Sequence _buttonSequence;
        private Transform _buttonTransform;

        private void Start()
        {
            _buttonSequence = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause();

            _buttonTransform = GetComponent<Transform>();

            _buttonSequence.Append(_buttonTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f))
                .Join(_buttonTransform.DOPunchRotation(Vector3.one, 0.5f));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _buttonSequence.PlayForward();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _buttonSequence.SmoothRewind();
        }

        private void OnDestroy()
        {
            _buttonSequence.Kill();
        }

        private void OnDisable()
        {
            _buttonSequence.Rewind();
        }
    }
}
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
            // Setup the DOTween Sequence.
            _buttonSequence = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause();

            _buttonTransform = GetComponent<Transform>();

            // Function of the Sequence.
            _buttonSequence.Append(_buttonTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f))
                .Join(_buttonTransform.DOPunchRotation(Vector3.one, 0.5f));
        }

        // Play the animation forward if the cursor enters the button. Forward because "SmoothRewind" flips the animation.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _buttonSequence.PlayForward();
        }

        // Smoothly rewind the animation if the cursor leaves the button. Flips the animation direction.
        public void OnPointerExit(PointerEventData eventData)
        {
            _buttonSequence.SmoothRewind();
        }

        // Kill the animation if the object is destroyed.
        private void OnDestroy()
        {
            _buttonSequence.Kill();
        }

        // Rewind the animation - reset properties to initial values if the object is disabled.
        private void OnDisable()
        {
            _buttonSequence.Rewind();
        }
    }
}
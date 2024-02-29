using DG.Tweening;
using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
    public class Table : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] float _rotationTime;

        [SerializeField] private GameObject _table;

        private float _rotation;

        private void Start()
        {
            GameInputManager.Instance.OnTurnTableRightAction += InstanceOnTurnTableRightAction;
            GameInputManager.Instance.OnTurnTableLeftAction += InstanceOnTurnTableLeftAction;
        }

        private void InstanceOnTurnTableLeftAction(object sender, System.EventArgs e)
        {
            if (GameStateManager.Instance.IsGamePaused || GameStateManager.Instance.IsGameOver) return;

            _rotation -= 90f;
            _table.transform.DORotate(_rotation * Vector3.up, _rotationTime);
            EventManager.Instance.PlayAudio.Invoke(4, 0);
            EventManager.Instance.TurnLeftEvent.Invoke();
            PlayerManager.Instance.TurnRight();
        }

        private void InstanceOnTurnTableRightAction(object sender, System.EventArgs e)
        {
            if (GameStateManager.Instance.IsGamePaused || GameStateManager.Instance.IsGameOver) return;

            _rotation += 90f;
            _table.transform.DORotate(_rotation * Vector3.up, _rotationTime);
            EventManager.Instance.PlayAudio.Invoke(4, 0);
            EventManager.Instance.TurnRightEvent.Invoke();
            PlayerManager.Instance.TurnLeft();
        }

        private void OnDestroy()
        {
            if (GameInputManager.Instance != null)
            {
                // Unsubscribe from events in case of destruction.
                GameInputManager.Instance.OnTurnTableLeftAction-= InstanceOnTurnTableLeftAction;
                GameInputManager.Instance.OnTurnTableRightAction -= InstanceOnTurnTableRightAction;
            }
        }

        private void OnApplicationQuit()
        {
            if (GameInputManager.Instance != null)
            {
                // Unsubscribe from events in case of destruction.
                GameInputManager.Instance.OnTurnTableLeftAction -= InstanceOnTurnTableLeftAction;
                GameInputManager.Instance.OnTurnTableRightAction -= InstanceOnTurnTableRightAction;
            }
        }
    }
}
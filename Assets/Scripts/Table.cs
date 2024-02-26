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
            GameInputManager.Instance.OnTurnTableClockwiseAction += Instance_OnTurnTableClockwiseAction;
            GameInputManager.Instance.OnTurnTableCounterClockwiseAction += Instance_OnTurnTableCounterClockwiseAction;
        }

        private void Instance_OnTurnTableCounterClockwiseAction(object sender, System.EventArgs e)
        {
            if (GameStateManager.Instance.IsGamePaused || GameStateManager.Instance.IsGameOver) return;

            _rotation -= 90f;
            _table.transform.DORotate(_rotation * Vector3.up, _rotationTime);
            EventManager.Instance.PlayAudio.Invoke(4, 0);
            EventManager.Instance.TurnLeftEvent.Invoke();
            PlayerManager.Instance.TurnRight();
        }

        private void Instance_OnTurnTableClockwiseAction(object sender, System.EventArgs e)
        {
            if (GameStateManager.Instance.IsGamePaused || GameStateManager.Instance.IsGameOver) return;

            _rotation += 90f;
            _table.transform.DORotate(_rotation * Vector3.up, _rotationTime);
            EventManager.Instance.PlayAudio.Invoke(4, 0);
            EventManager.Instance.TurnRightEvent.Invoke();
            PlayerManager.Instance.TurnLeft();
        }
    }
}
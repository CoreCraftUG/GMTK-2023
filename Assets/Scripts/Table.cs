using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using JamCraft.GMTK2023.Code;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] float _rotationTime;
    [SerializeField] private GameObject _playTable;

    private float _rotation;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !GameStateManager.Instance.IsGamePaused && !GameStateManager.Instance.IsGameOver && PlayerManager.Instance.CanTurn)
        {
            _rotation -= 90f;
            _playTable.transform.DORotate(_rotation * Vector3.up, _rotationTime);
            EventManager.Instance.PlayAudio.Invoke(4, 0);
            EventManager.Instance.TurnLeftEvent.Invoke();
            PlayerManager.Instance.TurnRight();
        }
        if (Input.GetKeyDown(KeyCode.D) && !GameStateManager.Instance.IsGamePaused && !GameStateManager.Instance.IsGameOver && PlayerManager.Instance.CanTurn)
        {
            _rotation += 90f;
            _playTable.transform.DORotate(_rotation * Vector3.up, _rotationTime);
            EventManager.Instance.PlayAudio.Invoke(4, 0);
            EventManager.Instance.TurnRightEvent.Invoke();
            PlayerManager.Instance.TurnLeft();
        }
    }
}
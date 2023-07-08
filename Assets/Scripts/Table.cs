using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] float _rotationTime;
    [SerializeField] private GameObject _playTable;

    private float _rotation;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && Playermanager.Instance.CanTurn)
        {
            _rotation -= 90f;
            _playTable.transform.DORotate(_rotation * Vector3.up, _rotationTime);
            EventManager.Instance.PlayAudio.Invoke(4, 0);
            Playermanager.Instance.TurnRight();
        }
        if (Input.GetKeyDown(KeyCode.D) && Playermanager.Instance.CanTurn)
        {
            _rotation += 90f;
            _playTable.transform.DORotate(_rotation * Vector3.up, _rotationTime);
            EventManager.Instance.PlayAudio.Invoke(4, 0);
            Playermanager.Instance.TurnLeft();
        }
    }
}
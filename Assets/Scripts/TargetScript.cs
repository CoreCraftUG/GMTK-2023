using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{

    [SerializeField] private Player _player;

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.ReturnPresentedCard().position;
        transform.rotation = _player.ReturnPresentedCard().rotation;
    }
}

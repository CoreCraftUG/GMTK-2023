using JamCraft.GMTK2023.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WineSnifter : MonoBehaviour
{
    Animator animator => GetComponent<Animator>();

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && !GameStateManager.Instance.IsGamePaused && !GameStateManager.Instance.IsGameOver && PlayerManager.Instance.CanTurn)
        {
            animator.SetBool("sway",true);
        }
    }

    public void StopSwaying()
    {
        animator.SetBool("sway", false);
    }
}

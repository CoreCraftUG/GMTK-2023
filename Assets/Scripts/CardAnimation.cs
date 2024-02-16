using System.Collections;
using System.Collections.Generic;
using JamCraft.GMTK2023.Code;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    public Animator CardAnimator => GetComponent<Animator>();

    public void StartTimer(float timer)
    {



        CardAnimator.speed = 1/timer;

        CardAnimator.SetBool("Outer", true);

        //StartCoroutine(WaitForCards());
    }

    public void SetTimerProgress(float progress)
    {
        CardAnimator.Play("CardRotation", -1, progress);
    }

    IEnumerator WaitForCards()
    {
        yield return new WaitForSeconds(.2f);
        CardAnimator.SetBool("Outer", false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTimer : MonoBehaviour
{
    public Animator CardAnimator;

    public void StartTimer(float timer)
    {
        

        CardAnimator.speed = 1/timer;

        CardAnimator.SetBool("Outer", true);

        StartCoroutine(WaitForCards());
    }

    IEnumerator WaitForCards()
    {
        yield return new WaitForSeconds(.2f);
        CardAnimator.SetBool("Outer", false);
    }
}

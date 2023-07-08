using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;



public class IKController : MonoBehaviour
{
    private Animator animator;
    private bool _ikActive = true;
    [SerializeField] private GameObject _rightArm;
    
    [SerializeField] private Player _player;
    private Transform _handObj = null;
    private Transform _lookObj = null;


    private void Start()
    {
        animator = GetComponent<Animator>();   
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (_ikActive)
            {
                Debug.Log(_handObj);
                if(_lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(_lookObj.position);
                }
                if(_handObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, _handObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, _handObj.rotation);
                }
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }

}

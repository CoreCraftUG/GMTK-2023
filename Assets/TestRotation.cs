using DG.Tweening;
using UnityEngine;

namespace JamCraft.GMTK2023
{
    public class TestRotation : MonoBehaviour
    {
        private Transform m_Transform;
        private Tween m_Tween;
        
        private void Start()
        {
            m_Transform = GetComponent<Transform>();
            m_Tween = m_Transform.DORotate(new Vector3(180, 275, 140), 10, RotateMode.Fast).SetLoops(2000).SetAutoKill(false);
        }

        private void OnDestroy()
        {
            m_Tween.Kill();
        }
    }
}

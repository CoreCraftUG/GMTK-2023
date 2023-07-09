using UnityEngine;
using Cinemachine;

namespace JamCraft.GMTK2023.Code
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float _intensity;
        [SerializeField] private float _time;

        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineBasicMultiChannelPerlin _cinemachineMultiChannelPerlin;
        private float _timer;
        private float _timerTotal;
        private float _startingIntensity;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cinemachineMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Start()
        {
            EventManager.Instance.PointsAddedEvent.AddListener((int i) =>
            {
                //ShakeCamera(_intensity, _time);
            });
            EventManager.Instance.MatchingCardsEvent.AddListener((bool b) =>
            {
                if (b)
                    ShakeCamera(_intensity, _time);
                else
                    ShakeCamera(_intensity, _time);
            });
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.PointsAddedEvent.RemoveAllListeners();
                EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
            }
        }

        private void OnApplicationQuit()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.PointsAddedEvent.RemoveAllListeners();
                EventManager.Instance.MatchingCardsEvent.RemoveAllListeners();
            }
        }

        public void ShakeCamera(float intensity, float time)
        {
            _cinemachineMultiChannelPerlin.m_AmplitudeGain = intensity;
            _startingIntensity = intensity;
            _timerTotal = time;
            _timer = time;
        }

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;

                _cinemachineMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - (_timer / _timerTotal));
            }
        }
    }
}
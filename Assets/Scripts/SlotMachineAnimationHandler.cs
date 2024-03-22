using JamCraft.GMTK2023;
using UnityEngine;

public class SlotMachineAnimationHandler : AnimationHandler
{
    private bool _streakEnded;
    private bool _allLightsOut;

    private void Awake()
    {
        EventManager.Instance.OnPointsMultipliedTempPointsAdded.AddListener(() =>
        {
            int randomNumber = Random.Range(0, 10);

            PlayAnimation(randomNumber > 5 ? "BulbReset Layer.ANIM_MultiplierUp_B" : "BulbReset Layer.ANIM_UpScore", 1, 0f);

            _allLightsOut = false;

            //if (!_streakEnded)
            //{
            //    PlayAnimation("Base Layer.OnMultiplyTempPointsAdded", 0, 0f);
            //}
            //else
            //{
            //    PlayAnimation("Base Layer.OnMultiplyTempPointsAddedWithStreakEnd", 0, 0f);
            //    _streakEnded = false;
            //}

            if (!_streakEnded) return;

            PlayAnimation(randomNumber > 5 ? "BulbRest Layer.Test 1" : "BulbReset Layer.Test 2", 1, 0f);

            //PlayAnimation("BulbReset Layer.ANIM_BulbReset", 1, 0f);
            _streakEnded = false;
        });

        EventManager.Instance.OnPointsChanged.AddListener(() =>
        {
            PlayAnimation("Base Layer.ANIM_TempToScore", 0, 0f);
        });

        EventManager.Instance.OnTempPointsAdded.AddListener(() =>
        {
            PlayAnimation("Base Layer.ANIM_TempScoreUp", 0, 0f);
        });

        EventManager.Instance.MissedMultiplyEvent.AddListener(value =>
        {
            if (_allLightsOut) return;

            PlayAnimation($"Base Layer.ANIM_BulbOff_{value}", 0, 0f);

            if (value >= 3)
            {
                _allLightsOut = true;
            }
        });

        EventManager.Instance.StreakEndEvent.AddListener(() =>
        {
            _streakEnded = true;
        });
    }
}

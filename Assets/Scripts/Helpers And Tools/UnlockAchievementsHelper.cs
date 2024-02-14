using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockAchievementsHelper : MonoBehaviour
{
    [Header("Clear All Achievements")]
    [SerializeField] private Button _clearAllAchievements;

    [Header("Clear Row Streak")]
    [SerializeField] private Button _unlock2RowMatchButton;
    [SerializeField] private Button _unlock3RowMatchButton;
    [SerializeField] private Button _unlock5RowMatchButton;

    [Header("Row Streak")]
    [SerializeField] private Button _unlockRedRowButton;
    [SerializeField] private Button _unlockGreenRowButton;
    [SerializeField] private Button _unlockYellowRowButton;
    [SerializeField] private Button _unlockBlueRowButton;
    [SerializeField] private Button _unlockPurpleRowButton;


    void Start()
    {
        _unlock2RowMatchButton.onClick.AddListener(() =>
        {
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Red);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Purple);
        });
        _unlock3RowMatchButton.onClick.AddListener(() =>
        {
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Red);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Purple);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Blue);
        });
        _unlock5RowMatchButton.onClick.AddListener(() =>
        {
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Red);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Purple);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Yellow);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Blue);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Green);
        });

        _unlockRedRowButton.onClick.AddListener(() =>
        {
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Red);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Red);
        });
        _unlockGreenRowButton.onClick.AddListener(() =>
        {
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Green);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Green);
        });
        _unlockBlueRowButton.onClick.AddListener(() =>
        {
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Blue);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Blue);
        });
        _unlockYellowRowButton.onClick.AddListener(() =>
        {
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Yellow);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Yellow);
        });
        _unlockPurpleRowButton.onClick.AddListener(() =>
        {
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Purple);
            EventManager.Instance.RowStreakAchievementEvent.Invoke(ECardColour.Purple);
        });
    }

    void Update()
    {
        
    }
}

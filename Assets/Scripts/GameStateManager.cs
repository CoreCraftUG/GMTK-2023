using System;
using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public event EventHandler OnGamePaused;
        public event EventHandler OnGameUnpaused;

        private bool _isGamePaused = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;
        }

        public void TogglePauseGame()
        {
            // If game is paused set timeScale to 0f else 1f and notify OnGamePaused / OnGameUnpaused subscribers.

            _isGamePaused = !_isGamePaused;

            if (_isGamePaused)
            {
                Time.timeScale = 0;
                OnGamePaused?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Time.timeScale = 1f;
                OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
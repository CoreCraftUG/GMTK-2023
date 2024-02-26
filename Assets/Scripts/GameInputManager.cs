using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JamCraft.GMTK2023.Code
{
    public class GameInputManager : MonoBehaviour
    {
        public static GameInputManager Instance { get; private set; }

        private GameInput _gameInput;

        #region EventHandlers

        public event EventHandler OnTurnTableClockwiseAction;
        public event EventHandler OnTurnTableCounterClockwiseAction;
        public event EventHandler OnPlaceCardAction;

        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _gameInput = new GameInput();
            _gameInput.Player.Enable();

            RegisterInputActions();
        }

        private void PlaceCard_performed(InputAction.CallbackContext obj)
        {
            OnPlaceCardAction?.Invoke(this, EventArgs.Empty);
        }

        private void TurnTableCounterClockwise_performed(InputAction.CallbackContext obj)
        {
            OnTurnTableCounterClockwiseAction?.Invoke(this, EventArgs.Empty);
        }

        private void TurnTableClockwise_performed(InputAction.CallbackContext obj)
        {
            OnTurnTableClockwiseAction?.Invoke(this, EventArgs.Empty);
        }

        private void RegisterInputActions()
        {
            _gameInput.Player.TurnTableClockwise.performed += TurnTableClockwise_performed;
            _gameInput.Player.TurnTableCounterClockwise.performed += TurnTableCounterClockwise_performed;
            _gameInput.Player.PlaceCard.performed += PlaceCard_performed;
        }

        private void OnEnable()
        {
            _gameInput.Enable();
        }

        private void OnDisable()
        {
            _gameInput.Disable();
        }

        private void Update()
        {
            // Escape pauses the game. TODO: Refactor.
            if (Input.GetKeyDown(KeyCode.Escape) && !GameStateManager.Instance.IsGameOver)
            {
                GameStateManager.Instance.TogglePauseGame();
            }
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JamCraft.GMTK2023.Code
{
    public class GameInputManager : MonoBehaviour
    {
        public static GameInputManager Instance { get; private set; }

        private GameInput _gameInput;

        private const string PLAYER_INPUT_BINDINGS = "InputBindings";
        private readonly string _saveFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\My Games\\House Always WINS!\\SaveFile.ini";

        #region EventHandlers

        public event EventHandler OnTurnTableClockwiseAction;
        public event EventHandler OnTurnTableCounterClockwiseAction;
        public event EventHandler OnPlaceCardAction;
        public event EventHandler OnPauseAction;

        #endregion

        public enum Binding
        {
            TurnTableClockwise,
            TurnTableCounterClockwise,
            PlaceCard,
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            _gameInput = new GameInput();
            _gameInput.Player.Enable();

            if (ES3.KeyExists(PLAYER_INPUT_BINDINGS, _saveFilePath))
            {
                _gameInput.LoadBindingOverridesFromJson(ES3.Load<string>(PLAYER_INPUT_BINDINGS, _saveFilePath));
            }

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
            _gameInput.Player.Pause.performed += Pause_performed;
        }

        private void Pause_performed(InputAction.CallbackContext obj)
        {
            OnPauseAction?.Invoke(this, EventArgs.Empty);
        }

        public string GetBindingText(Binding binding)
        {
            switch (binding)
            {
                default:
                case Binding.TurnTableClockwise:
                    return _gameInput.Player.TurnTableClockwise.bindings[0].ToDisplayString();
                case Binding.TurnTableCounterClockwise:
                    return _gameInput.Player.TurnTableCounterClockwise.bindings[0].ToDisplayString();
                case Binding.PlaceCard:
                    return _gameInput.Player.PlaceCard.bindings[0].ToDisplayString();
            }
        }

        public void RebindBinding(Binding binding, Action onActionRebound)
        {
            _gameInput.Player.Disable();

            InputAction inputAction;
            int bindingIndex;

            switch (binding)
            {
                default:
                case Binding.TurnTableClockwise:
                    inputAction = _gameInput.Player.TurnTableClockwise;
                    bindingIndex = 0;
                    break;
                case Binding.TurnTableCounterClockwise:
                    inputAction = _gameInput.Player.TurnTableCounterClockwise;
                    bindingIndex = 0;
                    break;
                case Binding.PlaceCard:
                    inputAction = _gameInput.Player.PlaceCard;
                    bindingIndex = 0;
                    break;
            }

            inputAction.PerformInteractiveRebinding(bindingIndex)
                .OnComplete(callback =>
                {
                    callback.Dispose();
                    _gameInput.Player.Enable();
                    onActionRebound();
                    ES3.Save(PLAYER_INPUT_BINDINGS, _gameInput.SaveBindingOverridesAsJson(), _saveFilePath);
                })
                .Start();
        }
    }
}
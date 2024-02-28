using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace JamCraft.GMTK2023.Code
{
    public class GameInputManager : MonoBehaviour
    {
        public static GameInputManager Instance { get; private set; }

        private GameInput _gameInput;

        private const string PLAYER_INPUT_BINDINGS = "InputBindings";
        private readonly string _saveFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\My Games\\House Always WINS!\\SaveFile.ini";

        #region EventHandlers

        public event EventHandler OnTurnTableRightAction;
        public event EventHandler OnTurnTableLeftAction;
        public event EventHandler OnPlaceCardAction;
        public event EventHandler OnPauseAction;

        #endregion

        public enum Binding
        {
            TurnTableRight,
            TurnTableLeft,
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
            OnTurnTableLeftAction?.Invoke(this, EventArgs.Empty);
        }

        private void TurnTableClockwise_performed(InputAction.CallbackContext obj)
        {
            OnTurnTableRightAction?.Invoke(this, EventArgs.Empty);
        }

        private void Pause_performed(InputAction.CallbackContext obj)
        {
            OnPauseAction?.Invoke(this, EventArgs.Empty);
        }

        private void RegisterInputActions()
        {
            _gameInput.Player.TurnTableRight.performed += TurnTableClockwise_performed;
            _gameInput.Player.TurnTableLeft.performed += TurnTableCounterClockwise_performed;
            _gameInput.Player.PlaceCard.performed += PlaceCard_performed;
            _gameInput.Player.Pause.performed += Pause_performed;
            //InputSystem.onAnyButtonPress.CallOnce(ctrl => Debug.Log($"{ctrl} pressed"));
        }

        public string GetBindingText(Binding binding, int bindingIndex)
        {
            switch (binding)
            {
                default:
                case Binding.TurnTableRight:
                    return _gameInput.Player.TurnTableRight.bindings[bindingIndex].ToDisplayString();
                case Binding.TurnTableLeft:
                    return _gameInput.Player.TurnTableLeft.bindings[bindingIndex].ToDisplayString();
                case Binding.PlaceCard:
                    return _gameInput.Player.PlaceCard.bindings[bindingIndex].ToDisplayString();
            }
        }

        public void RebindBinding(Binding binding, Action onActionRebound, int bindingIndex)
        {
            _gameInput.Player.Disable();

            InputAction inputAction;

            switch (binding)
            {
                default:
                case Binding.TurnTableRight:
                    inputAction = _gameInput.Player.TurnTableRight;
                    break;
                case Binding.TurnTableLeft:
                    inputAction = _gameInput.Player.TurnTableLeft;
                    break;
                case Binding.PlaceCard:
                    inputAction = _gameInput.Player.PlaceCard;
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
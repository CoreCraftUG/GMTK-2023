using System;
using CoreCraft.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace JamCraft.GMTK2023.Code
{
    public class GameInputManager : MonoBehaviour
    {
        public static GameInputManager Instance;

        private GameInput _gameInput;
        private ControlScheme _currentControlScheme;
        private InputUser _currentUser;
        private string _currentCancelKey;

        private const string PLAYER_INPUT_BINDINGS = "InputBindings";
        private readonly string _saveFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\My Games\\House Always WINS!\\SaveFile.ini";

        #region EventHandlers

        public event EventHandler OnTurnTableRightAction;
        public event EventHandler OnTurnTableLeftAction;
        public event EventHandler OnPlaceCardAction;
        public event EventHandler OnPauseAction;

        #endregion

        public UnityEvent<InputBinding> OnDuplicateKeybindingFound;
        public UnityEvent<ControlScheme> OnInputDeviceChanged;

        public enum Binding
        {
            TurnTableRight,
            TurnTableLeft,
            PlaceCard
        }

        public enum ControlScheme
        {
            Keyboard,
            Gamepad
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            DontDestroyOnLoad(this.gameObject);

            _gameInput = new GameInput();
            _gameInput.Player.Enable();
            
            _currentUser = InputUser.PerformPairingWithDevice(Keyboard.current);
            _currentUser.AssociateActionsWithUser(_gameInput);
            _currentUser.ActivateControlScheme(_gameInput.KeyboardScheme);
            _currentControlScheme = ControlScheme.Keyboard;

            ++InputUser.listenForUnpairedDeviceActivity;
            InputUser.onUnpairedDeviceUsed += InputUser_onUnpairedDeviceUsed;

            if (ES3.KeyExists(PLAYER_INPUT_BINDINGS, _saveFilePath))
            {
                _gameInput.LoadBindingOverridesFromJson(ES3.Load<string>(PLAYER_INPUT_BINDINGS, _saveFilePath));
            }

            RegisterInputActions();
        }

        private void Start()
        {
            OnInputDeviceChanged.AddListener(SetKeybindBindingCancelKey);
        }

        private void SetKeybindBindingCancelKey(ControlScheme controlScheme)
        {
            switch (controlScheme)
            {
                default:
                case ControlScheme.Keyboard:
                    _currentCancelKey = "<Keyboard>/escape";
                    break;
                case ControlScheme.Gamepad:
                    _currentCancelKey = "<Gamepad>/eastButton";
                    break;
            }
        }

        private void InputUser_onUnpairedDeviceUsed(InputControl inputControl, UnityEngine.InputSystem.LowLevel.InputEventPtr inputEventPtr)
        {
            ControlScheme cacheControlScheme = _currentControlScheme;

            InputDevice inputDevice = inputControl.device;

            if (inputDevice is Gamepad)
            {
                _currentUser.UnpairDevices();

                InputUser.PerformPairingWithDevice(Gamepad.current, user: _currentUser);

                _currentUser.ActivateControlScheme(ControlScheme.Gamepad.ToString());

                _currentControlScheme = ControlScheme.Gamepad;
            }

            if (inputDevice is Keyboard || inputDevice is Mouse)
            {
                _currentUser.UnpairDevices();

                InputUser.PerformPairingWithDevice(Keyboard.current, user: _currentUser);
                InputUser.PerformPairingWithDevice(Mouse.current, user: _currentUser);

                _currentUser.ActivateControlScheme(ControlScheme.Keyboard.ToString());

                _currentControlScheme = ControlScheme.Keyboard;
            }

            if (cacheControlScheme != _currentControlScheme)
            {
                OnInputDeviceChanged?.Invoke(_currentControlScheme);
                Debug.Log($"Device changed to {_currentControlScheme}.");
            }
        }

        public void GameOptionsUI_OnResetToDefault()
        {
            foreach (InputActionMap map in _gameInput.asset.actionMaps)
            {
                foreach (InputAction inputAction in map.actions)
                {
                    inputAction.RemoveBindingOverride(InputBinding.MaskByGroup(_currentControlScheme.ToString()));
                    ES3.Save(PLAYER_INPUT_BINDINGS, _gameInput.SaveBindingOverridesAsJson(), _saveFilePath);
                }
            }
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
            //InputSystem.onAnyButtonPress.CallOnce(control => Debug.Log("Test"));
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

        public void RebindBinding(Binding binding, Action onActionRebound, int bindingIndex/*, bool allCompositeParts = false*/)
        {
            //_gameInput.Player.Disable();

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

            inputAction.Disable();

            inputAction.PerformInteractiveRebinding(bindingIndex)
                .WithCancelingThrough(_currentCancelKey)
                .WithControlsExcluding("<Mouse>")
                .WithControlsExcluding("<Keyboard>/anyKey")
                .OnCancel(operation =>
                {
                    ResetBinding(inputAction, bindingIndex);
                    inputAction.Enable();
                    onActionRebound?.Invoke();
                    ES3.Save(PLAYER_INPUT_BINDINGS, _gameInput.SaveBindingOverridesAsJson(), _saveFilePath);
                    operation.Dispose();
                })
                .OnComplete(operation =>
                {
                    if (CheckForDuplicateBindings(inputAction, bindingIndex/*, allCompositeParts*/))
                    {
                        inputAction.RemoveBindingOverride(bindingIndex);

                        operation.Dispose();

                        RebindBinding(binding, () =>
                        {
                            GameOptionsUI.Instance.HideRebindPanel();
                            GameOptionsUI.Instance.UpdateVisual();
                        }, bindingIndex);

                        return;
                    }

                    inputAction.Enable();
                    onActionRebound?.Invoke();
                    ES3.Save(PLAYER_INPUT_BINDINGS, _gameInput.SaveBindingOverridesAsJson(), _saveFilePath);
                    operation.Dispose();
                })
                .Start();
        }

        private void ResetBinding(InputAction inputAction, int bindingIndex)
        {
            InputBinding newBinding = inputAction.bindings[bindingIndex];
            string oldOverridePath = newBinding.overridePath;

            inputAction.RemoveBindingOverride(bindingIndex);

            foreach (InputAction otherInputAction in inputAction.actionMap.actions)
            {
                if (otherInputAction == inputAction)
                {
                    continue;
                }

                for (int i = 0; i < otherInputAction.bindings.Count; i++)
                {
                    InputBinding binding = otherInputAction.bindings[i];

                    if (binding.overridePath == newBinding.path)
                    {
                        otherInputAction.ApplyBindingOverride(i, oldOverridePath);
                    }
                }
            }
        }

        private bool CheckForDuplicateBindings(InputAction inputAction, int bindingIndex/*, bool allCompositeParts = false*/)
        {
            InputBinding newBinding = inputAction.bindings[bindingIndex];

            foreach (InputBinding binding in inputAction.actionMap.bindings)
            {
                if (binding.action == newBinding.action)
                {
                    continue;
                }

                if (binding.effectivePath == newBinding.effectivePath)
                {
                    OnDuplicateKeybindingFound?.Invoke(binding);
                    // TODO: Highlight keybinding?
                    return true;
                }
            }

            //if (allCompositeParts)
            //{
            //    for (int i = 1; i < bindingIndex; i++)
            //    {
            //        if (inputAction.bindings[i].effectivePath == newBinding.overridePath)
            //        {
            //            Debug.Log($"Duplicate binding found: {newBinding.effectivePath}!");
            //            return true;
            //        }
            //    }
            //}

            return false;
        }
    }
}
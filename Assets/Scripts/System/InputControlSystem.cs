using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using BS.GameObjects;
using BS.Common;
using UnityEngine.InputSystem.UI;
using UnityEngine.Events;
using BS.UI;

namespace BS.System
{
    public class InputControlSystem : ISystem
    {
        private static InputControlSystem _instance;
        public static InputControlSystem Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<InputControlSystem>();
                }

                return _instance;
            }
        }

        public Transform CurrentPlayableTransform => _currentPlayableCharacter.transform;

        private InputSystemUIInputModule _inputSystemUIInputModule;
        private InputActionAsset _currentInputAsset;
        private InputActionAsset _currentUIInputAsset;
        private AbstractCharacter _currentPlayableCharacter;

        private bool _leftPressed;
        private bool _rightPressed;
        private bool _upPressed;
        private bool _downPressed;

        public bool IsInput
        {
            get
            {
                return _currentInputAsset.enabled;
            }

            set
            {
                if(value)
                {
                    if(_currentInputAsset != null)
                    {
                        _currentInputAsset.Enable();
                    }
                }
                else
                {
                    if(_currentInputAsset != null)
                    {
                        _currentInputAsset.Disable();
                    }
                }
            }
        }

        public bool IsUIInput
        {
            get
            {
                return _currentUIInputAsset.enabled;
            }
            set
            {
                if(value)
                {
                    if(_currentUIInputAsset != null)
                    {
                        _currentUIInputAsset.Enable();
                    }
                }
                else
                {
                    if(_currentUIInputAsset != null)
                    {
                        _currentUIInputAsset.Disable();
                    }
                }
            }
        }

        private UnityEvent _uiCancelEvent = new UnityEvent();
        public UnityEvent UICancel => _uiCancelEvent;
        private UnityEvent _uiSubmitEvent = new UnityEvent();
        public UnityEvent UISubmit => _uiSubmitEvent;
        public void Load()
        {
            Initialize();
        }

        public void Unload()
        {
            Release();
        }

        private void Initialize()
        {
            _inputSystemUIInputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            _uiCancelEvent.AddListener(PauseCancelEvent);
        }

        private void PauseCancelEvent()
        {
            if(GameSequenceSystem.Instance.CurrentState == GameStepState.Playing)
            {
                GameSequenceSystem.Instance.SetGameStepState(GameStepState.Paused);
            }
            else if(GameSequenceSystem.Instance.CurrentState == GameStepState.Paused)
            {
                GameSequenceSystem.Instance.SetGameStepState(GameStepState.Playing);
            }
            else
            {
                return;
            }
        }

        private void Release()
        {

        }

        public void SetUIInputActionAsset(InputActionAsset inputActionAsset)
        {
            if(_currentUIInputAsset != null)
            {
                _currentUIInputAsset.Disable();
                _currentUIInputAsset = null;
            }
            _currentUIInputAsset = inputActionAsset;
            _inputSystemUIInputModule.actionsAsset = _currentUIInputAsset;

            var actionMap = _currentUIInputAsset.FindActionMap(Constrants.STR_UI_CONTROL);
            actionMap.FindAction(Constrants.STR_UIINPUT_ACTION_CANCEL).performed += CancelUIInput;
            actionMap.FindAction(Constrants.STR_UIINPUT_ACTION_SUBMIT).performed += SubmitUIInput;

            if (_currentUIInputAsset != null)
            {
                _currentUIInputAsset.Enable();
            }
        }

        private void SubmitUIInput(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                _uiSubmitEvent.Invoke();
            }
        }

        private void CancelUIInput(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                _uiCancelEvent.Invoke();
            }
        }

        public void SetInputActionAsset(InputActionAsset inputActionAsset)
        {
            if(_currentInputAsset != null)
            {
                _currentInputAsset.Disable();
                _currentInputAsset = null;
            }

            _currentInputAsset = inputActionAsset;
            
            if(_currentInputAsset != null)
            {
                var actionMap = _currentInputAsset.FindActionMap(Constrants.STR_CHARACTER_NIGHT);
                actionMap.FindAction(Constrants.STR_INPUT_ACTION_ATTACK).performed += AttackInput;
                actionMap.FindAction(Constrants.STR_INPUT_ACTION_DEFENSE).performed += DefenseInput;
                actionMap.FindAction(Constrants.STR_INPUT_ACTION_JUMP).performed += JumpInput;
                actionMap.FindAction(Constrants.STR_INPUT_ACTION_THROWING).performed += ThrowingInput;

                var actLeft = actionMap.FindAction(Constrants.STR_INPUT_ACTION_LEFT);
                var actRight = actionMap.FindAction(Constrants.STR_INPUT_ACTION_RIGHT);
                var actUp = actionMap.FindAction(Constrants.STR_INPUT_ACTION_UP);
                var actDown = actionMap.FindAction(Constrants.STR_INPUT_ACTION_DOWN);

                actLeft.started += MoveInput;
                actLeft.performed += TurnInput;
                actLeft.canceled += MoveInput;
                actRight.started+= MoveInput;
                actRight.performed += TurnInput;
                actRight.canceled += MoveInput;
                actUp.started += MoveInput;
                actUp.performed += TurnInput;
                actUp.canceled += MoveInput;
                actDown.started += MoveInput;
                actDown.performed += TurnInput;
                actDown.canceled += MoveInput;

                _currentInputAsset.Enable();
            }
        }

        public void SetPlayableCharacter(AbstractCharacter character)
        {
            _currentPlayableCharacter = character;
        }

        private void ThrowingInput(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                _currentPlayableCharacter.Throwing();
            }
        }

        private void AttackInput(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                _currentPlayableCharacter.Attack();
            }
        }

        private void DefenseInput(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                _currentPlayableCharacter.Defense();
            }
        }

        private void JumpInput(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                _currentPlayableCharacter.Jump();
            }
        }

        private void MoveInput(InputAction.CallbackContext context)
        {
            bool isPressed = !context.canceled; // performed/started => true, canceled => false
            switch(context.action.name)
            {
                case Constrants.STR_INPUT_ACTION_LEFT:
                    _leftPressed = isPressed;
                    break;
                case Constrants.STR_INPUT_ACTION_RIGHT:
                    _rightPressed = isPressed;
                    break;
                case Constrants.STR_INPUT_ACTION_UP:
                    _upPressed = isPressed;
                    break;
                case Constrants.STR_INPUT_ACTION_DOWN:
                    _downPressed = isPressed;
                    break;
            }

            Vector2 dir = new Vector2((_rightPressed ?1f :0f) + (_leftPressed ? -1f :0f),
                                     (_upPressed ?1f :0f) + (_downPressed ? -1f :0f));
            if (dir.sqrMagnitude >1f)
            {
                dir.Normalize();
            }

            _currentPlayableCharacter.Move(dir);
        }

        private void TurnInput(InputAction.CallbackContext context)
        {
            _leftPressed = false;
            _rightPressed = false;
            _upPressed = false;
            _downPressed = false;
            Vector2 dir = Vector2.zero;
            switch(context.action.name)
            {
                case Constrants.STR_INPUT_ACTION_LEFT:
                    dir = Vector2.left;
                    break;

                case Constrants.STR_INPUT_ACTION_RIGHT:
                    dir = Vector2.right;
                    break;

                case Constrants.STR_INPUT_ACTION_UP:
                    dir = Vector2.up;
                    break;

                case Constrants.STR_INPUT_ACTION_DOWN:
                    dir = Vector2.down;
                    break;
            }

            _currentPlayableCharacter.Turn(dir);
        }

        public void SetUISelectGameObjectSelected(GameObject gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
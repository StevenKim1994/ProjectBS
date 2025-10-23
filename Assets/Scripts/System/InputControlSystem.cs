using UnityEngine;
using UnityEngine.InputSystem;
using BS.GameObject;
using BS.Common;
using System.Net.Mime;

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

        private InputActionAsset _currentInputAsset;
        private AbstractCharacter _currentPlayableCharacter;

        // Track pressed states for directional inputs
        private bool _leftPressed;
        private bool _rightPressed;
        private bool _upPressed;
        private bool _downPressed;

        public void Load()
        {
            Initialize();
        }

        public void Unload()
        {
            Release();
        }

        protected void Initialize()
        {

        }

        protected void Release()
        {

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

                var actLeft = actionMap.FindAction(Constrants.STR_INPUT_ACTION_LEFT);
                var actRight = actionMap.FindAction(Constrants.STR_INPUT_ACTION_RIGHT);
                var actUp = actionMap.FindAction(Constrants.STR_INPUT_ACTION_UP);
                var actDown = actionMap.FindAction(Constrants.STR_INPUT_ACTION_DOWN);

                actLeft.performed += MoveInput;
                actLeft.canceled += MoveInput;
                actRight.performed += MoveInput;
                actRight.canceled += MoveInput;
                actUp.performed += MoveInput;
                actUp.canceled += MoveInput;
                actDown.performed += MoveInput;
                actDown.canceled += MoveInput;

                _currentInputAsset.Enable();
            }
        }

        public void SetPlayableCharacter(AbstractCharacter character)
        {
            _currentPlayableCharacter = character;
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

    }
}
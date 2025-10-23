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
           //     actionMap.FindAction(Constrants.STR_INPUT_ACTION_JUMP).performed += JumpInput;

                actionMap.FindAction(Constrants.STR_INPUT_ACTION_LEFT).performed += MoveInput;
                actionMap.FindAction(Constrants.STR_INPUT_ACTION_RIGHT).performed += MoveInput;
                actionMap.FindAction(Constrants.STR_INPUT_ACTION_UP).performed += MoveInput;
                actionMap.FindAction(Constrants.STR_INPUT_ACTION_DOWN).performed += MoveInput;

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
            if(context.performed)
            {
                switch(context.action.name)
                {
                    case Constrants.STR_INPUT_ACTION_LEFT:
                        _currentPlayableCharacter.Move(Vector2.left);
                        break;
                    case Constrants.STR_INPUT_ACTION_RIGHT:
                        _currentPlayableCharacter.Move(Vector2.right);
                        break;
                    case Constrants.STR_INPUT_ACTION_UP:
                        _currentPlayableCharacter.Move(Vector2.up);
                        break;
                    case Constrants.STR_INPUT_ACTION_DOWN:
                        _currentPlayableCharacter.Move(Vector2.down);
                        break;
                }
            }
        }

    }
}
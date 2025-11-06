using UnityEngine;
using BS.Common;
using BS.GameObjects;
using System;

namespace BS.System
{
    public class PlayerSystem : ISystem
    {
        private static PlayerSystem _instance;
        public static PlayerSystem Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<PlayerSystem>();
                }
                return _instance;
            }
        }

        private IPlayer _currentPlayer;
        public IPlayer CurrentPlayer => _currentPlayer;

        private Transform _currentPlayerTransform;
        public Transform CurrentPlayerTransform => _currentPlayerTransform;

        private IInteractable _interactableObject;

        public void Load()
        {

        }

        public void Unload()
        {

        }

        public void SetCurrentPlayer(IPlayer player, bool targetControl = true)
        {
            _currentPlayer = player;

            if(targetControl)
            {
                if(_currentPlayer is NightCharacter night)
                {
                    InputControlSystem.Instance.SetPlayableCharacter(night);
                    InputControlSystem.Instance.SetInputActionAsset(night.InputActionAsset);
                    _currentPlayerTransform = night.transform;
                }
            }
        }

        public void SetInteractObject(IInteractable interatableObject)
        {
            _interactableObject = interatableObject;
        }

        public bool InteractCurrentObject()
        {
            if(_interactableObject != null)
            {
                _interactableObject.Interact();
                return true;
            }

            return false;
        }
    }
}
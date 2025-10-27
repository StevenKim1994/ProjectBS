using UnityEngine;
using UnityEngine.Rendering.Universal;
using BS.System;

namespace BS.GameObjects
{
    public abstract class AbstractInGameScene : MonoBehaviour
    {
        [SerializeField]
        private PixelPerfectCamera _pixelPerfectCamera;
        public PixelPerfectCamera PixelPerfectCamera
        {
            get
            {
                if(_pixelPerfectCamera == null)
                {
                    _pixelPerfectCamera = FindFirstObjectByType<PixelPerfectCamera>();
                }

                return _pixelPerfectCamera;
            }
        }

        [SerializeField]
        private Canvas _mainCanvas;
        public Canvas MainCanvas
        {
            get
            {
                if(_mainCanvas == null)
                {
                    _mainCanvas = FindFirstObjectByType<Canvas>();
                }

                return _mainCanvas;
            }
        }

        protected virtual void Awake()
        {
            SystemGameObject.Instance.LoadAllSystems();
        }

        protected virtual void Start()
        {
            var player = FindFirstObjectByType<NightCharacter>();
            InputControlSystem.Instance.SetPlayableCharacter(player);
            InputControlSystem.Instance.SetInputActionAsset(player.InputActionAsset);
        }
    }
}
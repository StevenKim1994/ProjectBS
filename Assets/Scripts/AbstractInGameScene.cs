using UnityEngine;
using UnityEngine.Rendering.Universal;
using BS.System;
using BS.UI;
using BS.Common;
using UnityEngine.Tilemaps;

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

        [SerializeField] private Tilemap _groundTilemap;
        public Tilemap GroundTilemap
        {
            get
            {
                return _groundTilemap;
            }
        }

        [SerializeField]
        private Transform _spawnPoint;
        public Transform SpawnPoint
        {
            get
            {
                return _spawnPoint;
            }
        }

        protected virtual void Awake()
        {
            SystemGameObject.Instance.LoadAllSystems();
        }

        protected virtual void Start()
        {
            var playerObject = GameObject.Instantiate( ResourceSystem.Instance.GetLoadGameObject(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_NIGHT_PREFAB));
            if(playerObject.TryGetComponent<NightCharacter>(out var night))
            {
                night.transform.position = SpawnPoint.position;
                PlayerSystem.Instance.SetCurrentPlayer(night);
                InputControlSystem.Instance.SetUIInputActionAsset(ResourceSystem.Instance.GetLoadGameAsset<UnityEngine.InputSystem.InputActionAsset>(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_UI_UIINPUT_ACTION_ASSET_INPUTACTIONS));

                UISystem.Instance.Show<HUDUIPresenter>();
            }
        }
    }
}
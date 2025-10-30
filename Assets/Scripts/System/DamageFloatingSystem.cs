using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;
using BS.GameObjects;
using BS.Common;
using DG.Tweening;

namespace BS.System
{
    public class DamageFloatingSystem : ISystem
    {
        private static DamageFloatingSystem _instance;
        
        public static DamageFloatingSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<DamageFloatingSystem>();
                }

                return _instance;
            }
        }

        private ObjectPool<DamageFloating> _damageFloatingPool;
        private DamageFloating _floatingObjectOriginalPrefab;

        public void Load()
        {
            var loadGameObject = ResourceSystem.Instance.GetLoadGameObject(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_FLOATING_DAMAGE_FLOATING_PREFAB);
            if(loadGameObject != null)
            {
                if(loadGameObject.TryGetComponent<DamageFloating>(out var loadedGameObject))
                {
                    _floatingObjectOriginalPrefab = loadedGameObject;
                }
            }

            _damageFloatingPool = new ObjectPool<DamageFloating>(
                createFunc: () => GameObject.Instantiate(_floatingObjectOriginalPrefab),
                actionOnGet: (floating) => floating.gameObject.SetActive(true),
                actionOnRelease: (floating) => floating.gameObject.SetActive(false),
                actionOnDestroy: (floating) => GameObject.Destroy(floating.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 100
            );
        }

        public void Unload()
        {
            if (_damageFloatingPool != null)
            {
                _damageFloatingPool.Clear();
            }

            _floatingObjectOriginalPrefab = null;
        }

        public DamageFloating GetDamageFloating(float damageValue, Color color, float startSize, float endTweenSize, Vector3 position, float duration = 0.75f, Ease easeFunc = Ease.OutSine)
        {
            var floating = _damageFloatingPool.Get();
            
            floating
                .SetText(damageValue.ToString())
                .SetColor(color)
                .SetSize(startSize, endTweenSize)
                .SetPosition(position)
                .StartTween(duration, easeFunc);

            return floating;
        }

        public void ReleaseDamageFloating(DamageFloating damageFloating)
        {
            _damageFloatingPool.Release(damageFloating);
        }
    }
}
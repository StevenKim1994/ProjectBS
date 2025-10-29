using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;
using BS.GameObjects;
using BS.Common;

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

        }

        public DamageFloating GetDamageFloating(string text, Color color, float size, Vector3 position)
        {
            var floating = _damageFloatingPool.Get();
            
            floating
                .SetFloatingText(text, color, size)
                .SetPosition(position)
                .StartTween();

            return floating;
        }

        public void ReleaseDamageFloating(DamageFloating damageFloating)
        {
            _damageFloatingPool.Release(damageFloating);
        }
    }
}
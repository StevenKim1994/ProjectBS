using UnityEngine;
using UnityEngine.Pool;
using BS.GameObjects;
using BS.Common;

namespace BS.System
{
    public class DamageColiderSystem : ISystem
    {
        private static DamageColiderSystem _instance;
        public static DamageColiderSystem Instance
        {
            get
            {
                if(_instance == null )
                {
                    _instance = SystemGameObject.Instance.GetSystem<DamageColiderSystem>();
                }

                return _instance;
            }
        }

        private bool _isInitialize = false;
        private ObjectPool<DamageColider> _dmgColiderPool;

        public void Load()
        {
            Initialize();
        }

        public void Unload()
        {
            Release();
        }

        private DamageColider OnCreateObject()
        {
            var loadObject = ResourceSystem.Instance.GetLoadGameObject(Constrants.STR_DAMAGE_COLIDER);
            if (loadObject != null)
            {
                loadObject = UnityEngine.GameObject.Instantiate(loadObject);
                if(loadObject.TryGetComponent<DamageColider>(out var result))
                {
                    result.SetPool(_dmgColiderPool);
                    return result;
                }
            }

            return null;
        }

        private void OnGetObject(DamageColider damageColider)
        {
            damageColider.SetPool(_dmgColiderPool);
            //damageColider.gameObject.SetActive(true);
        }

        private void OnReleaseObject(DamageColider damageColider)
        {
            damageColider.SetDefault();
        }

        private void OnDestoryObject(DamageColider damageColider)
        {

        }

        private void Initialize()
        {
            if(!_isInitialize)
            {
                _dmgColiderPool = new ObjectPool<DamageColider>
                (
                    OnCreateObject,
                    OnGetObject,
                    OnReleaseObject,
                    OnDestoryObject
                );

                _isInitialize = true;
            }
        }

        private void Release()
        {
            if(_isInitialize)
            {
                _isInitialize = false;
            }
        }

        public DamageColider GetDamageCollider()
        {
            var damgeColider =  _dmgColiderPool.Get();
            damgeColider.SetPool(_dmgColiderPool);
            return damgeColider;
        }

    }
}
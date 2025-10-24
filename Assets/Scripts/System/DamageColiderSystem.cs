using UnityEngine;
using UnityEngine.Pool;
using BS.GameObject;

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

        private DamageColider CreateObject()
        {
            return new DamageColider(); // TODO :: 어드레서블 로드로 수정 필요
        }

        private void OnGetObject(DamageColider damageColider)
        {

        }

        private void OnReleaseObject(DamageColider damageColider)
        {

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
                    CreateObject,
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
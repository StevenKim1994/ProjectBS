using System;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Behavior;
using BS.System;

namespace BS.GameObjects
{
    public abstract class AbstractEnermy : AbstractCharacter
    {
        [SerializeField]
        private Transform _spawnPoint; // DESC :: 만약 스폰포인트를 통해서 스폰되었을 경우 리스폰을 위해 필요
        public Transform SpawnPoint => _spawnPoint;

        [SerializeField] 
        protected BehaviorGraphAgent _behaviorAgent; // DESC :: BehaviorGraphAgent 컴포넌트 참조
        public BehaviorGraphAgent BehaviorGraphAgent => _behaviorAgent;

        protected ObjectPool<AbstractEnermy> _parentPool;
        public ObjectPool<AbstractEnermy> ParentPool => _parentPool;

        protected override void Awake()
        {
            base.Awake();

            _currentHealth = Ability.Health;

            if(_behaviorAgent == null)
            {
                _behaviorAgent = TryGetComponent<BehaviorGraphAgent>(out var agent) ? agent : null;
            }
        }

        public override void Die()
        {
            base.Die();

            EnermySystem.Instance.KillEnermy(this);
        }

        public void SetParentPool(ObjectPool<AbstractEnermy> parentPool)
        {
            _parentPool = parentPool;
        }

        public virtual void OnSpawn()
        {

        }

        public virtual void SetDefault()
        {

        }
    }
}
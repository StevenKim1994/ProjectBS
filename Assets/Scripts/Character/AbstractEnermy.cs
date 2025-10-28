using UnityEngine;
using UnityEngine.Pool;
using Unity.Behavior;

namespace BS.GameObjects
{
    public abstract class AbstractEnermy : AbstractCharacter
    {
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

        public void SetParentPool(ObjectPool<AbstractEnermy> parentPool)
        {
            _parentPool = parentPool;
        }
    }
}
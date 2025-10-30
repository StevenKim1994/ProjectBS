using UnityEngine;
using BS.Common;

namespace BS.GameObjects
{
    public class AbstractPropObject : MonoBehaviour, IPropObject
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [SerializeField]
        private Animator _animator;
        public Animator Animator => _animator;

        [SerializeField]
        private Collider2D _colider;
        public Collider2D Colider => _colider;

        [SerializeField]
        protected bool _isDestructible = true;

        protected bool _isDestroyed = false;

        public bool IsDestructible => _isDestructible;
        public bool IsDestroyed => _isDestroyed;

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Initialize()
        {

        }

        public virtual void DestroyProp()
        {
            if(!_isDestructible || _isDestroyed)
            {
                return;
            }

            _isDestroyed = true;
        }

        public virtual void HitAnim()
        {

        }

        public virtual void TakeDamage(float amount)
        {

        }
    }
}
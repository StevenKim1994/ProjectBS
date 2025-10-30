using UnityEngine;
using UnityEngine.U2D;

namespace BS.GameObjects
{
    public class AbstractProjectileObject : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [SerializeField] AbstractProjectileMover _mover;
        public AbstractProjectileMover Mover => _mover;

        protected float _speed = 10f;
        public float Speed => _speed;

        [SerializeField]
        protected float _lifeTime = 5f;
        public float LifeTime => _lifeTime;

        [SerializeField]
        protected int _damage = 10;
        public int Damage => _damage;

        [SerializeField]
        protected Collider2D _colider;
        public Collider2D Colider => _colider;

        [SerializeField]
        protected bool _isGuided = false; // DESC :: 유도 투사체인지 여부, 이값으로 _targetTransform을 따라갈지 _targetPosition 위치로 갈지 결정
        public bool IsGuided => _isGuided;

        [SerializeField]
        protected Transform _targetTransform;
        public Transform TargetTransform => _targetTransform;

        [SerializeField]
        protected Vector3 _targetPosition;
        public Vector3 TargetPosition => _targetPosition;

        [SerializeField]
        protected AbstractCharacter _onwer;
        public AbstractCharacter Owner => _onwer;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        protected virtual void HitAnim()
        {

        }

        public virtual AbstractProjectileObject SetTargetTransform(Transform target)
        {
            _targetTransform = target;
            _targetPosition = target.position;

            return this;
        }

        public virtual AbstractProjectileObject SetTargetPosition(Vector3 position)
        {
            _targetTransform = null;
            _targetPosition = position;

            return this;
        }

        public virtual AbstractProjectileObject SetOwner(AbstractCharacter owner)
        {
            _onwer = owner;

            return this;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != _onwer)
            {
                _onwer.TakeDamage(_damage);
                HitAnim();
            }
        }
    }
}
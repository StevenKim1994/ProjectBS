using System;
using UnityEngine;
using BS.Common;
using UnityEngine.Pool;
using DG.Tweening;
using System.Linq;

namespace BS.GameObjects
{
    public class AbstractRewardableObject : MonoBehaviour, IRewardableObject
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [SerializeField]
        private Collider2D _colider;
        public Collider2D Colider => _colider;

        [SerializeField]
        private Rigidbody2D _rigidbody;
        public Rigidbody2D Rigidbody => _rigidbody;

        [SerializeField]
        private Animator _animator;
        public Animator Animator => _animator;

        [Header("Physics")]
        [Tooltip("바닥 물리 충돌용 콜라이더(Trigger=false)")]
        [SerializeField]
        private Collider2D _physicsCollider;
        public Collider2D PhysicsCollider => _physicsCollider;

        protected bool _isRewarded = false;

        protected Tweener _spawnTweener;

        protected Tweener _rewardGetTweener;

        public virtual bool IsRewarded => _isRewarded;

        protected ObjectPool<AbstractRewardableObject> _parentPool;

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Initialize()
        {
            _isRewarded = false;

            // 물리 콜라이더 기본 설정 (바닥 충돌 전용)
            if (_physicsCollider != null)
            {
                _physicsCollider.isTrigger = false;

                // 플레이어와의 물리 충돌은 무시 (플레이어는 트리거로만 감지)
                var playerGo = GameObject.FindWithTag(Constrants.TAG_PLAYER);
                if (playerGo != null)
                {
                    var playerColliders = playerGo.GetComponentsInChildren<Collider2D>();
                    foreach (var plc in playerColliders)
                    {
                        if (plc != null)
                            Physics2D.IgnoreCollision(_physicsCollider, plc, true);
                    }
                }

                // 다른 보상 아이템과의 물리 충돌 무시
                var rewardableObjects = GameObject.FindGameObjectsWithTag(Constrants.TAG_REWARDABLE).ToList();
                foreach (var rewardable in rewardableObjects)
                {
                    if (rewardable != null) 
                    {
                        if(rewardable.TryGetComponent<AbstractRewardableObject>(out var rewardableObject))
                        {
                            if(rewardableObject.PhysicsCollider != null)
                            {
                                Physics2D.IgnoreCollision(_physicsCollider, rewardableObject.PhysicsCollider, true);
                            }
                        }
                    }
                }

                // 다른 Enemy들과의 물리 충돌 무시
                var enemyObjects = GameObject.FindGameObjectsWithTag(Constrants.TAG_ENERMY);
                foreach (var enemy in enemyObjects)
                {
                    if (enemy != null)
                    {
                        if(enemy.TryGetComponent<AbstractEnermy>(out var enemyObject))
                        {
                            if (enemyObject.Collider != null)
                            {
                                Physics2D.IgnoreCollision(_physicsCollider, enemyObject.Collider, true);
                            }
                        }
                    }
                }
            }

            // Rigidbody2D 기본 물리 세팅
            if (_rigidbody != null)
            {
                _rigidbody.simulated = true;
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                _rigidbody.gravityScale = Mathf.Max(1f, _rigidbody.gravityScale);
                _rigidbody.freezeRotation = true;
                _rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
                _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }
        }

        public virtual void Reward(Action rewardCallback = null)
        {
            _isRewarded = true;

            if (rewardCallback != null)
            {
                rewardCallback();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Constrants.TAG_PLAYER))
            {
                if (collision.TryGetComponent<IPlayer>(out IPlayer player))
                {
                    player.GainRewardableObject(this);
                }
            }
        }

        public AbstractRewardableObject SetParentPool(ObjectPool<AbstractRewardableObject> objectPool)
        {
            _parentPool = objectPool;

            return this;
        }

        public AbstractRewardableObject SetPosition(Vector3 position)
        {
            transform.position = position;

            return this;
        }

        public AbstractRewardableObject SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;

            return this;
        }

        public AbstractRewardableObject SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);

            return this;
        }

        public virtual void Spawn()
        {
            if(_spawnTweener != null)
            {
                _spawnTweener.Kill(false);
                _spawnTweener = null;
            }
        }

        public virtual void Release()
        {
            if(_parentPool != null)
            {
                if(_spawnTweener != null)
                {
                    _spawnTweener.Kill(false);
                    _spawnTweener = null;
                }
                if(_rewardGetTweener != null)
                {
                    _rewardGetTweener.Kill(false);
                    _rewardGetTweener = null;
                }
                _isRewarded = false;
                _parentPool.Release(this);
            }
        }
    }
}

using System;
using UnityEngine;
using BS.Common;
using UnityEngine.Pool;
using DG.Tweening;

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

using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace BS.GameObject
{
    public class DamageColider : MonoBehaviour
    {
        [SerializeField] private Collider2D _colider;
        public Collider2D colider => _colider;

        private AbstractCharacter _damageOwner;
        public AbstractCharacter damageOwner => _damageOwner;

        private float _damage;
        public float Damage => _damage;

        private bool _enable = false;
        public bool Enable => _enable;

        private float _duration;

        private CancellationTokenSource _timeCTS;
        private ObjectPool<DamageColider> _parentPool;

        public void SetDamageInfo(AbstractCharacter owner, float damage, float duration = 0.33f)
        {
            _damageOwner = owner;
            _damage = damage;
            _enable = true;
            _duration = duration;

            if (_timeCTS != null)
            {
                _timeCTS.Cancel();
                _timeCTS.Dispose();
            }

            _timeCTS = new CancellationTokenSource();
            DurationTimer().Forget();
        }

        private async UniTask DurationTimer()
        {
            await UniTask.WaitForSeconds(_duration, cancelImmediately: true, cancellationToken: _timeCTS.Token);
            _parentPool.Release(this);
        }

        public void SetPool(ObjectPool<DamageColider> parentPool)
        {
            _parentPool = parentPool;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.TryGetComponent<AbstractCharacter>(out var character))
            {
                if (_damageOwner != character)
                {
                    Debug.Log("Attacker: " + _damageOwner.name);
                    character.TakeDamage(_damage);
                    _enable = false;

                    _parentPool.Release(this);
                }
            }
        }
        public void SetDefault()
        {
            transform.gameObject.SetActive(false);
            _timeCTS.Cancel();
            _timeCTS.Dispose();
            _timeCTS = null;
            _duration = 0f;
            _damageOwner = null;
            _damage = 0f;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            
        }

    }

}
using System.Threading;
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

        public void SetDamageInfo(AbstractCharacter owner, float damage, float duration = 0f)
        {
            _damageOwner = owner;
            _damage = damage;
            _enable = true;
            _duration = duration;
        }

        public void SetPool(ObjectPool<DamageColider> parentPool)
        {
            _parentPool = parentPool;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.TryGetComponent<AbstractCharacter>(out var character))
            {
                character.TakeDamage(_damage);
                _enable = false;

                _parentPool.Release(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            
        }
    }

}
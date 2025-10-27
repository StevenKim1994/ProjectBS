using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace BS.GameObjects
{
    public class DamageColider : MonoBehaviour
    {
        [SerializeField] private Collider2D _colider;
        public Collider2D colider => _colider;

        private AbstractCharacter _damageOwner;
        public AbstractCharacter damageOwner => _damageOwner;

        private float _damage;
        public float Damage => _damage;

        private bool _enable = false; // DESC :: 사용가능 여부 
        public bool Enable => _enable;

        private float _duration;

        private int _targetMaxCount; // DESC :: 한번에 공격 가능한 최대 타겟 수
        public int TargetMaxCount => _targetMaxCount;

        private int _currentTargetCount; // DESC :: 현재 적중한 타겟의 수이 값이 _targetMaxCount를 넘으면 더이상 공격 불가
        public int CurrentTargetCount => _currentTargetCount;

        private CancellationTokenSource _timeCTS;
        private ObjectPool<DamageColider> _parentPool;

        public void SetDamageInfo(AbstractCharacter owner, float damage, float duration = 0.33f, int targetMaxCount = 1)
        {
            _targetMaxCount = targetMaxCount;
            _currentTargetCount = 0;
            _damageOwner = owner;
            _damage = damage;
            _enable = true;
            _duration = duration;
            if (_timeCTS != null)
            {
                _timeCTS.Cancel();
                _timeCTS.Dispose();
                _timeCTS = null;
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
                if (_enable)
                {
                    if (_damageOwner != character)
                    {
                        Debug.Log("Attacker: " + _damageOwner.name);
                        character.TakeDamage(_damage);
                        _currentTargetCount++;

                        if(_currentTargetCount >= _targetMaxCount)
                        {
                            Debug.Log("타겟 최대 수 도달, 콜라이더 비활성화");
                            _parentPool.Release(this);
                            _enable = false;
                        }
                    }
                }
            }
        }

        public void SetDefault()
        {
            transform.gameObject.SetActive(false);
            _enable = false;
            _timeCTS.Cancel();
            _timeCTS.Dispose();
            _timeCTS = null;
            _duration = 0f;
            _damageOwner = null;
            _damage = 0f;
            _currentTargetCount = 0;   
            _targetMaxCount = 0;
        }
    }

}
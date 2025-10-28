using System;
using System.Collections.Generic;
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

        private bool _enable = false;
        public bool Enable => _enable;

        private float _duration;

        private int _targetMaxCount;
        public int TargetMaxCount => _targetMaxCount;

        private int _currentTargetCount;
        public int CurrentTargetCount => _currentTargetCount;

        private CancellationTokenSource _timeCTS;
        private ObjectPool<DamageColider> _parentPool;

        private HashSet<Collider2D> _hitTargets = new HashSet<Collider2D>();

        public void SetDamageInfo(AbstractCharacter owner, float damage, float duration = 0.33f, int targetMaxCount = 1)
        {
            _targetMaxCount = targetMaxCount;
            _currentTargetCount = 0;
            _damageOwner = owner;
            _damage = damage;
            _enable = true;
            _duration = duration;
            _hitTargets.Clear();

            if (_timeCTS != null)
            {
                _timeCTS.Cancel();
                _timeCTS.Dispose();
                _timeCTS = null;
            }

            _timeCTS = new CancellationTokenSource();

            DurationTimer().Forget();
            gameObject.SetActive(true);
        }

        private async UniTask DurationTimer()
        {
            try
            {
                await UniTask.WaitForSeconds(_duration, delayTiming: PlayerLoopTiming.FixedUpdate, cancelImmediately: true, cancellationToken: _timeCTS.Token);
                
                _parentPool.Release(this);
            }
            catch (OperationCanceledException)
            {
                // Cancel된 경우 무시
            }
        }

        public void SetPool(ObjectPool<DamageColider> parentPool)
        {
            _parentPool = parentPool;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_enable) return;

            if (_hitTargets.Contains(collision))
            {
                Debug.Log($"[DamageCollider] 이미 적중한 타겟 무시: {collision.gameObject.name}");
                return;
            }

            if (collision.gameObject == _damageOwner.gameObject)
            {
                return;
            }
                 
            Debug.Log($"[DamageCollider] OnTriggerEnter2D: {collision.gameObject.name}");

            // Player의 공격인 경우
            if (_damageOwner.TryGetComponent<NightCharacter>(out var nightCharacter))
            {
                HandlePlayerAttack(collision);
            }
            // Enemy의 공격인 경우
            else if (_damageOwner.TryGetComponent<AbstractEnermy>(out var enemyOwner))
            {
                HandleEnemyAttack(collision, enemyOwner);
            }

            _hitTargets.Add(collision);
        }

        /// <summary>
        /// Player의 공격 처리
        /// </summary>
        private void HandlePlayerAttack(Collider2D collision)
        {
            if (collision.TryGetComponent<AbstractEnermy>(out var enemy))
            {
                Debug.Log($"[DamageCollider] Player가 Enemy 공격: {enemy.name}");
                ApplyDamage(collision, enemy);
            }
        }

        /// <summary>
        /// Enemy의 공격 처리
        /// </summary>
        private void HandleEnemyAttack(Collider2D collision, AbstractEnermy enemyOwner)
        {
            if (collision.TryGetComponent<NightCharacter>(out var player))
            {
                Debug.Log($"[DamageCollider] Enemy가 Player 공격: {player.name}");
                ApplyDamage(collision, player);
            }
        }

        /// <summary>
        /// 실제 데미지 적용 및 타겟 카운트 관리
        /// </summary>
        private void ApplyDamage(Collider2D targetCollider, AbstractCharacter target)
        {
            if (target == null || !target.IsAlive) return;

            _hitTargets.Add(targetCollider);

            target.TakeDamage(_damage);
            _currentTargetCount++;

            Debug.Log($"[DamageCollider] 데미지 적용: {_damage} -> {target.name} (적중: {_currentTargetCount}/{_targetMaxCount})");

            if (_currentTargetCount >= _targetMaxCount)
            {
                Debug.Log($"[DamageCollider] 타겟 최대 수 도달, 콜라이더 비활성화");
                if (_parentPool != null)
                {
                    _parentPool.Release(this);
                }
            }
        }

        public void SetDefault()
        {
            gameObject.SetActive(false);
            _enable = false;

            if (_timeCTS != null)
            {
                _timeCTS.Cancel();
                _timeCTS.Dispose();
                _timeCTS = null;
            }

            _duration = 0f;
            _damageOwner = null;
            _damage = 0f;
            _currentTargetCount = 0;
            _targetMaxCount = 0;
            
            _hitTargets.Clear();
        }
    }
}
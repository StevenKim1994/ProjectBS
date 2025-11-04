using UnityEngine;
using BS.System;
using BS.Common;
using BS.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace BS.GameObjects
{
    public sealed class NightCharacter : AbstractCharacter, IPlayer
    {
        private NightAbility _castingAbility;

        private float _currentAttackRange;
        private float _currentAttackDamage;

        // DESC :: 콤보 시스템 관련 변수
        private int _currentComboCount =0;
        private float _lastAttackTime = -999f;

        [SerializeField]
        private float _comboWindowTime =1.0f;
        private const int MAX_COMBO_COUNT =2;

        // DESC :: 공격 시 전진 관련 변수
        [Header("Attack Movement Settings")]
        [SerializeField]
        private float _attackForwardDistance =0.5f; // 공격 시 전진 거리
        [SerializeField]
        private float _attackForwardDuration =0.15f; // 전진 시간 (애니메이션 초반에만)
        [SerializeField]
        private AnimationCurve _attackMovementCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f); // 전진 곡선

        // DESC :: 점프 애니메이션 관련 변수
        [Header("Jump Animation Settings")]
        [SerializeField]
        private float _jumpVelocityThreshold =0.1f;
        private bool _isJumping = false;
        private bool _isJumpRising = false;
        private bool _isHit = false;

        private CancellationTokenSource _hitInvincibleCTS;
        private CancellationTokenSource _attackForwardCTS;

        protected override void Awake()
        {
            base.Awake();
            _castingAbility = _ability as NightAbility;

            _currentAttackRange = _castingAbility.AttackRange;
            _currentAttackDamage = _castingAbility.AttackDamage;
            _animator.SetFloat(AnimParamConstants.ATTACK_SPEED,0.5f);
        }

        public override void SetAttackDamageColliderActive(bool isActive)
        {
            if (isActive)
            {
                MeleeAttackColider.SetMeleeDamage(_currentAttackDamage)
                .SetPosition(_spriteRenderer.transform.position + (Vector3)(_mover.ViewDirection.normalized * _currentAttackRange))
                .SetOwnerCharacter(this)
                .SetSize(new Vector2(_currentAttackRange, 1.0f))
                .SetActiveTime(0.33f)
                .SetActiveColider(true);
            }
            base.SetAttackDamageColliderActive(isActive);
        }

        private void Update()
        {
            if (_isJumping)
            {
                UpdateJumpAnimation();
            }
        }

        private void UpdateJumpAnimation()
        {
            float verticalVelocity = _rigidbody.linearVelocity.y;

            if (verticalVelocity > _jumpVelocityThreshold)
            {
                if (!_isJumpRising)
                {
                    _isJumpRising = true;
                    _animator.CrossFade(AnimStateConstants.JUMP_START,0.1f);
                    Debug.Log("Jump Rising - Playing JumpStart");
                }
            }
            else if (verticalVelocity < -_jumpVelocityThreshold)
            {
                if (_isJumpRising)
                {
                    _isJumpRising = false;
                    _animator.CrossFade(AnimStateConstants.JUMP_END,0.1f);
                    Debug.Log("Jump Falling - Playing JumpEnd");
                }
            }
        }

        public override void Jump()
        {
            if (!_isAlive) return;

            base.Jump();

            _isJumping = true;
            _isJumpRising = true;

            _animator.SetBool(AnimParamConstants.IS_JUMPING, _isJumping);

            Debug.Log("Jump Started - Playing JumpStart");
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);

            if (_isAlive && collision.gameObject.CompareTag(Constrants.TAG_GROUND))
            {
                _isJumping = false;
                _isJumpRising = false;
                _animator.SetBool(AnimParamConstants.IS_JUMPING, _isJumping);
                Debug.Log("Landed on ground");
            }
            else
            {
                Debug.Log("Enter Collision : " + collision.gameObject.name);

                if (_isAlive && collision.gameObject.CompareTag(Constrants.TAG_ENERMY))
                {
                    if (!_isHit)
                    {
                        if(collision.gameObject.TryGetComponent<AbstractEnermy>(out var enemyCharacter))
                        {
                            TakeDamage(enemyCharacter.CurrentDamage);
                            UISystem.Instance.GetPresenter<HUDUIPresenter>().TakeDamage(enemyCharacter.CurrentDamage);
                            StartHitInvincible(0.33f).Forget();
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (_hitInvincibleCTS != null)
            {
                _hitInvincibleCTS.Cancel();
                _hitInvincibleCTS.Dispose();
                _hitInvincibleCTS = null;
            }
        }

        public override void ResetAttackCombo()
        {
            base.ResetAttackCombo();
            _currentComboCount = 0;
            _attackMovementCurve.Evaluate(0f);
        }

        public override void Attack()
        {
            base.Attack();

            float currentTime = Time.time;

            if (currentTime - _lastAttackTime >= _comboWindowTime)
            {
                _currentComboCount = 0;
            }

            _currentComboCount++;

            if (_currentComboCount > MAX_COMBO_COUNT)
            {
                _currentComboCount = 0;
            }


            _lastAttackTime = currentTime;

            Debug.Log($"Attack Combo: {_currentComboCount}");

            // DESC :: 공격 콜라이더 설정
            Vector2 viewDirection = _mover.ViewDirection;
            MeleeAttackColider.SetMeleeDamage(_currentAttackDamage)
                .SetPosition(_spriteRenderer.transform.position + (Vector3)(viewDirection.normalized * _currentAttackRange))
                .SetOwnerCharacter(this)
                .SetSize(new Vector2(_currentAttackRange, 1.0f));

            _animator.SetInteger(AnimParamConstants.ATTACK_COUNT, _currentComboCount);
        }

        public void ForwardAttackMovement()
        {
            // DESC :: 공격 시 전진 효과 적용
            if(_attackForwardCTS != null)
            {
                _attackForwardCTS.Cancel();
                _attackForwardCTS.Dispose();
                _attackForwardCTS = null;
            }
            _attackForwardCTS = new CancellationTokenSource();
            ApplyAttackForwardMovement(_mover.ViewDirection).Forget();
        }
        /// <summary>
        /// 공격 시 앞으로 전진하는 효과 (UniTask 사용)
        /// </summary>
        private async UniTaskVoid ApplyAttackForwardMovement(Vector2 direction)
        {
            //if (_attackForwardDistance <= 0f || _attackForwardDuration <=0f)
            //    return;

            try
            {
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = startPosition + (Vector3)(direction.normalized * _attackForwardDistance);
                
                float elapsedTime =0f;

                while (elapsedTime < _attackForwardDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float normalizedTime = Mathf.Clamp01(elapsedTime / _attackForwardDuration);
                    
                    // DESC :: AnimationCurve를 사용하여 부드러운 이동
                    float curveValue = _attackMovementCurve.Evaluate(normalizedTime);
                    Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, curveValue);
                    
                    transform.position = newPosition;

                    await UniTask.Yield(PlayerLoopTiming.Update, _attackForwardCTS.Token, cancelImmediately: true);
                }

                // DESC :: 최종 위치 보정
                transform.position = targetPosition;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Attack forward movement cancelled: {e.Message}");
            }
        }

        public override void Die()
        {
            base.Die();
            GameSequenceSystem.Instance.SetGameStepState(GameStepState.GameOver);
        }

        public AbstractCharacter GetCharacterType()
        {
            return this;
        }

        public override void Throwing()
        {
            base.Throwing();
            if (_isAlive)
            {
                Debug.Log("투사체 던집니다!");
            }
        }

        public override void Move(Vector2 direction)
        {
            base.Move(direction);

            if (direction != Vector2.zero)
            {
                if (direction == Vector2.left)
                {
                    _spriteRenderer.flipX = true;
                }
                else
                {
                    _spriteRenderer.flipX = false;
                }

                Debug.Log("Night Character Move!");
            }

            _animator.SetFloat(AnimParamConstants.MOVE_SPEED, direction.magnitude);
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            ScreenSystem.Instance.ShakeCamera(0.25f,0.2f,1);
            UISystem.Instance.FlashScreen(Color.red,0.3f);
            Debug.Log("Night Character Take Damage!");
        }

        public void GainRewardableObject(AbstractRewardableObject rewardableObject)
        {
            if (!rewardableObject.IsRewarded)
            {
                DataSystem.Instance.AddReward(rewardableObject);
                var destScreenPos = UISystem.Instance.GetPresenter<HUDUIPresenter>().GetGoldImageScreenPos();
                RewardableObjectSystem.Instance.PlayGainRewardEffect(rewardableObject, destScreenPos);
            }
        }

        /// <summary>
        /// 피격 무적 시작 (duration 동안 추가 접촉 데미지 무시)
        /// </summary>
        private async UniTaskVoid StartHitInvincible(float duration)
        {
            if (_hitInvincibleCTS != null)
            {
                _hitInvincibleCTS.Cancel();
                _hitInvincibleCTS.Dispose();
                _hitInvincibleCTS = null;
            }

            _hitInvincibleCTS = new CancellationTokenSource();

            _isHit = true;

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _hitInvincibleCTS.Token, cancelImmediately: true);
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            finally
            {
                _isHit = false;
            }
        }
    }
}
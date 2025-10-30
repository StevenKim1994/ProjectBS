using UnityEngine;
using BS.System;
using BS.Common;
using BS.UI;

namespace BS.GameObjects
{
    public sealed class NightCharacter : AbstractCharacter, IPlayer
    {
        private NightAbility _castingAbility;

        private float _currentAttackRange;
        private float _currentAttackDamage;

        // DESC :: 콤보 시스템 관련 변수
        private int _currentComboCount = 0;
        private float _lastAttackTime = -999f;
        [SerializeField]
        private float _comboWindowTime = 1.0f; // 콤보 유지 시간 (초)
        private const int MAX_COMBO_COUNT = 2; // 최대 콤보 수 (Attack1, Attack2)

        protected override void Awake()
        {
            base.Awake();
            _castingAbility = _ability as NightAbility;

            _currentAttackRange = _castingAbility.AttackRange;
            _currentAttackDamage = _castingAbility.AttackDamage;
        }

        public override void Attack()
        {
            base.Attack();

            // DESC :: 콤보 시스템 처리
            float currentTime = Time.time;

            // DESC :: 마지막 공격으로부터 일정 시간이 지났으면 콤보 초기화
            if (currentTime - _lastAttackTime > _comboWindowTime)
            {
                _currentComboCount = 0;
            }

            // DESC :: 콤보 카운트 증가
            _currentComboCount++;

            // DESC :: 최대 콤보 수를 넘으면 다시 1로 리셋
            if (_currentComboCount > MAX_COMBO_COUNT)
            {
                _currentComboCount = 1;
            }

            // DESC :: 애니메이터에 콤보 카운트 전달
            _animator.SetInteger(AnimParamConstants.ATTACK_COUNT, _currentComboCount);

            // DESC :: 마지막 공격 시간 업데이트
            _lastAttackTime = currentTime;

            Debug.Log($"Attack Combo: {_currentComboCount}");

            // DESC :: ViewDirection 방향으로 공격 위치 설정
            Vector2 viewDirection = _mover.ViewDirection;

            // DESC :: ViewDirection이 0인 경우 기본 방향 설정 (오른쪽)
            if (viewDirection.sqrMagnitude < 0.001f)
            {
                viewDirection = Vector2.right;
            }

            MeleeAttackColider.SetMeleeDamage(_currentAttackDamage)
                .SetPosition(_spriteRenderer.transform.position + (Vector3)(viewDirection.normalized * _currentAttackRange))
                .SetOwnerCharacter(this)
                .SetSize(new Vector2(_currentAttackRange, 1.0f))
                .SetActiveTime(0.33f)
                .SetActiveColider(true);
        }

        public override void Die()
        {
            base.Die();

            Debug.Log("Night Character Die!");
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
            ScreenSystem.Instance.ShakeCamera(0.25f, 0.2f, 1);
            UISystem.Instance.FlashScreen(Color.red, 0.3f);
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
    }
}
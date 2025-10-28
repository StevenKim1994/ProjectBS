using UnityEngine;
using BS.System;
using BS.Common;

namespace BS.GameObjects
{
    public class NightCharacter : AbstractCharacter, IPlayer
    {
        private NightAbility _castingAbility;

        private float _currentAttackRange;
        private float _currentAttackDamage;

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

            /*
            var damageColider = DamageColiderSystem.Instance.GetDamageCollider();
            if (damageColider != null)
            {
                damageColider.SetDamageInfo(this, _currentAttackDamage);

                // DESC :: ViewDirection 방향으로 공격 위치 설정
                Vector2 viewDirection = _mover.ViewDirection;

                // DESC :: ViewDirection이 0인 경우 기본 방향 설정 (오른쪽)
                if (viewDirection.sqrMagnitude < 0.001f)
                {
                    viewDirection = Vector2.right;
                }

                // DESC :: 캐릭터 위치 + ViewDirection * 공격 범위
                Vector3 attackPosition = _spriteRenderer.transform.position + (Vector3)(viewDirection.normalized * _currentAttackRange);
                damageColider.transform.position = attackPosition;
            }
            */
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

        public override void Move(Vector2 direction)
        {
            base.Move(direction);

            if(direction != Vector2.zero)
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
    }
}
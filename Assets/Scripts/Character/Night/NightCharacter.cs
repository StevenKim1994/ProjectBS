using BS.System;
using UnityEngine;

namespace BS.GameObject
{
    public class NightCharacter : AbstractCharacter, IPlayer
    {
        private NightAbility _castingAbility;

        private void Awake()
        {
            _castingAbility = _ability as NightAbility;
        }

        public override void Attack()
        {
            base.Attack();
            Debug.Log("Night Character Attack!");
            var damageColider = DamageColiderSystem.Instance.GetDamageCollider();
            if (damageColider != null)
            {
                damageColider.SetDamageInfo(this, _castingAbility.AttackDamage);
                damageColider.transform.position = _spriteRenderer.transform.position;
                // TODO :: 현재 캐릭터 앞쪽에 생성
            }
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

        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);

            Debug.Log("Night Character Take Damage!");
        }
    }
}
using BS.System;
using UnityEngine;

namespace BS.GameObjects
{
    public class ScareCrow : AbstractCharacter
    {
        public override void Attack() 
        {
            // DESC :: 허수아비는 공격기능이 없으므로 base호출 안함.
        }
        public override void Move(Vector2 direction)
        {
            // DESC :: 허수아비는 이동기능이 없으므로 base 호출 안함
        }

        public override void Die()
        {
            base.Die();
        }

        public override void HitAnim()
        {
            base.HitAnim();
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
        }
    }
}
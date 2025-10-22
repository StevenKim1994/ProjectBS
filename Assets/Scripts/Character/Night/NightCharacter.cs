using UnityEngine;

namespace BS.GameObject
{
    public class NightCharacter : AbstractCharacter
    {
        public override void Attack()
        {
            base.Attack();
        }

        public override void Die()
        {
            base.Die();
        }

        public override void Move(Vector2 direction)
        {
            base.Move(direction);
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
        }
    }
}
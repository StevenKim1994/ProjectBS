using UnityEngine;

namespace BS.GameObject
{
    public class NightCharacter : AbstractCharacter, IPlayer
    {
        public override void Attack()
        {
            base.Attack();

            Debug.Log("Night Character Attack!");
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

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);

            Debug.Log("Night Character Take Damage!");
        }
    }
}
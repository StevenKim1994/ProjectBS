using UnityEditor.Tilemaps;
using UnityEngine;

namespace BS.GameObject
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

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
        }

        protected override void OnTriggerEnterCallback(Collider2D collision)
        {
            base.OnTriggerEnterCallback(collision);

            if(collision != _colider)
            {
                Debug.Log("허수아비는 아무 일도 일어나지 않는다.");
            }
        }

        protected override void OnTriggerStayCallback(Collider2D collision)
        {
            base.OnTriggerStayCallback(collision);
        }

        protected override void OnTriggerExitCallback(Collider2D collision)
        {
            base.OnTriggerExitCallback(collision);
        }
    }
}
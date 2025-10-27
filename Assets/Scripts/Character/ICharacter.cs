using UnityEngine;

namespace BS.GameObjects
{
    public interface ICharacter 
    {
        void Move(Vector2 direction);
        void Attack();
        void TakeDamage(float amount);
        void Die();
    }
}
using UnityEngine;

namespace BS.GameObject
{
    public interface ICharacter 
    {
        void Move(Vector2 direction);
        void Attack();
        void TakeDamage(int amount);
        void Die();
    }
}
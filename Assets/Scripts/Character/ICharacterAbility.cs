using UnityEngine;

namespace BS.GameObject
{
    public interface ICharacterAbility 
    {
        public float MoveSpeed { get; }
        public int Health { get; }
        public int Mana { get; }
        public int MaxHealth { get; }
        public int MaxMana { get; }

        public void SetHealth(int health);
        public void SetMana(int mana);
        public void SetMaxHealth(int maxHealth);
        public void SetMaxMana(int maxMana);

        public void SetMoveSpeed(float moveSpeed);

    }
}
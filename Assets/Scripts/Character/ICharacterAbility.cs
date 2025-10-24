using UnityEngine;

namespace BS.GameObject
{
    public interface ICharacterAbility 
    {
        public float MoveSpeed { get; }
        public float Health { get; }
        public float Mana { get; }
        public float MaxHealth { get; }
        public float MaxMana { get; }

        public void SetHealth(float health);
        public void SetMana(float mana);
        public void SetMaxHealth(float maxHealth);
        public void SetMaxMana(float maxMana);

        public void SetMoveSpeed(float moveSpeed);

    }
}
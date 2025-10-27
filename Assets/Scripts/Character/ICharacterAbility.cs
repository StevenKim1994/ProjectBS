using UnityEngine;

namespace BS.GameObjects
{
    public interface ICharacterAbility 
    {
        public float MoveSpeed { get; }
        public float Health { get; }
        public float Mana { get; }
        public float MaxHealth { get; }
        public float MaxMana { get; }
        public float JumpForce { get; }

        public ICharacterAbility SetHealth(float health);
        public ICharacterAbility SetMana(float mana);
        public ICharacterAbility SetMaxHealth(float maxHealth);
        public ICharacterAbility SetMaxMana(float maxMana);
        public ICharacterAbility SetMoveSpeed(float moveSpeed);
        public ICharacterAbility SetJumpForce(float jumpForce);

    }
}
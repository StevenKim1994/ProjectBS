using UnityEngine;

namespace BS.GameObjects
{
    public class AbstractCharacterAbility : ScriptableObject, ICharacterAbility
    {
        [SerializeField]
        private float _health;
        public float Health => _health;

        [SerializeField]
        private float _damage;
        public float Damage => _damage;

        [SerializeField]
        private float _mana;
        public float Mana => _mana;

        [SerializeField]
        private float _maxHealth;
        public float MaxHealth => _maxHealth;

        [SerializeField]
        private float _maxMana;
        public float MaxMana => _maxMana;

        [SerializeField]
        private float _moveSpeed;
        public float MoveSpeed => _moveSpeed;

        [SerializeField]
        private float _attackRange;
        public float AttackRange => _attackRange;

        [SerializeField]
        private float _jumpForce;
        public float JumpForce => _jumpForce;

        public ICharacterAbility SetHealth(float health)
        {
            _health = health;

            return this;
        }

        public ICharacterAbility SetMana(float mana)
        {
            _mana = mana;

            return this;
        }

        public ICharacterAbility SetMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;

            return this;
        }

        public ICharacterAbility SetMaxMana(float maxMana)
        {
            _maxMana = maxMana;

            return this;
        }

        public ICharacterAbility SetMoveSpeed(float moveSpeed)
        {
            _moveSpeed = moveSpeed;

            return this;
        }

        public ICharacterAbility SetJumpForce(float jumpForce)
        {
            _jumpForce = jumpForce;
         
            return this;
        }

    }
}
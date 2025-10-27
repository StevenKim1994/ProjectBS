using UnityEngine;

namespace BS.GameObjects
{
    public class AbstractCharacterAbility : ScriptableObject, ICharacterAbility
    {
        [SerializeField]
        private float _health;
        public float Health => _health;

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
        public void SetHealth(float health)
        {
            _health = health;
        }

        public void SetMana(float mana)
        {
            _mana = mana;
        }

        public void SetMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void SetMaxMana(float maxMana)
        {
            _maxMana = maxMana;
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }

    }
}
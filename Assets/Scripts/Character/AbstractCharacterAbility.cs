using UnityEngine;

namespace BS.GameObject
{
    public class AbstractCharacterAbility : ScriptableObject, ICharacterAbility
    {
        [SerializeField]
        private int _health;
        public int Health => _health;

        [SerializeField]
        private int _mana;
        public int Mana => _mana;

        [SerializeField]
        private int _maxHealth;
        public int MaxHealth => _maxHealth;

        [SerializeField]
        private int _maxMana;
        public int MaxMana => _maxMana;

        [SerializeField]
        private float _moveSpeed;
        public float MoveSpeed => _moveSpeed;
        public void SetHealth(int health)
        {
            _health = health;
        }

        public void SetMana(int mana)
        {
            _mana = mana;
        }

        public void SetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void SetMaxMana(int maxMana)
        {
            _maxMana = maxMana;
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }

    }
}
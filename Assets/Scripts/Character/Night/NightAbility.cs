using UnityEngine;
using BS.Common;

namespace BS.GameObject
{
    [CreateAssetMenu(fileName = "NightAbilitySO", menuName = "Ability/Player/Night")]
    public class NightAbility : AbstractCharacterAbility
    {
        [SerializeField]
        private float _attackDamage;
        public float AttackDamage
        {
            get { return _attackDamage; }
        }

        [SerializeField] 
        private float _attackRange = 1.0f; 
        // DESC :: 공격 범위 (캐릭터로부터의 거리)
        public float AttackRange
        {
            get { return _attackRange; }
        }
    }
}
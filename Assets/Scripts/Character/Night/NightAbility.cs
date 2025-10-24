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
    }
}
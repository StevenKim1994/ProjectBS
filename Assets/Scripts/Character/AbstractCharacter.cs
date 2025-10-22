using UnityEngine;

namespace BS.GameObject
{
    public abstract class AbstractCharacter : MonoBehaviour, ICharacter
    {
        [SerializeField]
        private ICharacterAbility _ability;
        public ICharacterAbility Ability => _ability;

        [SerializeField]
        private ICharacterMover _mover;
        public ICharacterMover Mover => _mover;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [SerializeField]
        private Animator _animator;
        public Animator Animator => _animator;

        public virtual void Attack()
        {
            // TODO :: 애니메이션 처리 
        }

        public virtual void Die()
        {
            // TODO :: 애니메이션 처리
        }

        public virtual void Move(Vector2 direction)
        {
            if(_mover != null)
            {
                Mover.Move(direction, Ability.MoveSpeed);
                // TODO :: 애니메이션 처리 Mover.ViewDirection 활용
            }
            else
            {
                // DESC :: 이캐릭터는 움직일 수 없음.
                Debug.Log("이 캐릭터는 움직일 수 없음.");
            }
        }

        public virtual void TakeDamage(int amount)
        {
            Ability.SetHealth(Ability.Health - amount);

            // TODO :: 애니메이션 처리
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BS.GameObject
{
    public abstract class AbstractCharacter : MonoBehaviour, ICharacter
    {
        [SerializeField]
        protected AbstractCharacterAbility _ability;
        public AbstractCharacterAbility Ability => _ability;

        [SerializeField]
        protected InputActionAsset _inputActionAsset;
        public InputActionAsset InputActionAsset => _inputActionAsset;

        [SerializeField]
        protected AbstractCharacterMover _mover;
        public AbstractCharacterMover Mover => _mover;

        [SerializeField]
        protected SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [SerializeField]
        protected Animator _animator;
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
            if (_mover != null)
            {
                if(direction == Vector2.zero)
                {
                    Mover.Stop();
                }
                else
                {
                    Debug.Log($"Move Direction : {direction}");
                    Mover.Move(direction, Ability.MoveSpeed);
                    // TODO :: 애니메이션 처리 Mover.ViewDirection 활용
                }
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

        public virtual void Defense()
        {
            // TODO :: 애니메이션 처리
        }

        public virtual void Jump()
        {
            if(_mover != null)
            {
                // TODO :: 애니메이션 처리
            }
            else
            {
                Debug.Log("이 캐릭터는 점프할 수 없음.");
            }
        }
    }
}
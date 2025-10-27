using BS.Common;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

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

        [SerializeField]
        protected Rigidbody2D _rigidbody;
        public Rigidbody2D Rigidbody => _rigidbody;

        [SerializeField]
        protected Collider2D _colider;
        public Collider2D Collider => _colider;


        public virtual void Attack()
        {
            if(_mover != null)
            {
                _mover.Stop();
            }

            _animator.CrossFade(Constrants.STR_INPUT_ACTION_ATTACK, 0.3f);
            // TODO :: 콜라이더 처리
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

        public virtual void TakeDamage(float amount)
        {
            Ability.SetHealth(Ability.Health - amount);
            
            if(_mover != null)
            {
                _mover.Slow(0.9f);
            }

            // TODO :: 피격 애니메이션 처리
        }

        public virtual void Defense()
        {
            // TODO :: 방어 애니메이션 처리
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

        public virtual void Turn(Vector2 dir)
        {
            Mover.Turn(dir);
        }

        protected virtual void OnTriggerEnterCallback(Collider2D collision)
        {
            Debug.Log("Enter Trigger : " + collision.name);
        }

        protected virtual void OnTriggerStayCallback(Collider2D collision)
        {
            Debug.Log("Stay Trigger : " + collision.name);
        }

        protected virtual void OnTriggerExitCallback(Collider2D collision)
        {
            Debug.Log("Exit Trigger : " + collision.name);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerEnterCallback(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            OnTriggerStayCallback(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            OnTriggerExitCallback(collision);
        }

    }
}
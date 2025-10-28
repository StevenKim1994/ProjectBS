using BS.Common;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BS.GameObjects
{
    public abstract class AbstractCharacter : MonoBehaviour, ICharacter
    {
        [SerializeField]
        protected AbstractCharacterAbility _ability;
        public AbstractCharacterAbility Ability => _ability;

        protected float _currentHealth;
        public float CurrentHealth => _currentHealth;

        protected float _currentMana;
        public float CurrentMana => _currentMana;

        protected float _currentSpeed;
        public float CurrentSpeed => _currentSpeed;

        protected float _currentJumpForce;
        public float CurrentJumpForce => _currentJumpForce;

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

        [SerializeField]
        protected AttachedDamageColider _attachedDamageColider;

        public AttachedDamageColider MeleeAttackColider => _attachedDamageColider;

        protected bool _isAlive = true;
        public bool IsAlive => _isAlive;

        protected virtual void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            _currentHealth = Ability.Health;
            _currentMana = Ability.Mana;
            _currentSpeed = Ability.MoveSpeed;
            _currentJumpForce = Ability.JumpForce;
        }

        public virtual void Attack()
        {
            Stop();

            _animator.CrossFade(Constrants.STR_INPUT_ACTION_ATTACK, 0.3f);
            // TODO :: 콜라이더 처리
        }

        public virtual void Die()
        {
            if (_isAlive)
            {
                _isAlive = false;
                _animator.SetBool(AnimParamConstants.IS_DIE, true);
            }
        }

        public virtual void Move(Vector2 direction)
        {
            if (_isAlive)
            {
                if (_mover != null)
                {
                    if (direction == Vector2.zero)
                    {
                        Stop();
                    }
                    else
                    {
                        Debug.Log($"Move Direction : {direction}");
                        _mover.Move(direction, _currentSpeed);
                    }
                }
                else
                {
                    // DESC :: 이캐릭터는 움직일 수 없음.
                    Debug.Log("이 캐릭터는 움직일 수 없음.");
                }
            }
        }

        public virtual void Stop()
        {
            if (_isAlive)
            {
                if (_mover != null)
                {
                    _mover.Stop();
                }
            }
        }


        public virtual void TakeDamage(float amount)
        {
            if (_isAlive)
            {
                _currentHealth = _currentHealth - amount;

                if (_currentHealth <= 0)
                {
                    Die();
                }
                else
                {
                    HitAnim();
                    if (_mover != null)
                    {
                        _mover.Knockback(2);
                        //_mover.Slow(0.9f);
                    }
                }
            }
        }

        public virtual void HitAnim()
        {
            if (_isAlive)
            {
                _animator.CrossFade(AnimStateConstants.HIT, 0.1f);
            }
            }

        public virtual void Defense()
        {
            if (_isAlive)
            {
                // TODO :: 방어 애니메이션 처리
                _animator.CrossFade(Constrants.STR_INPUT_ACTION_DEFENSE, 0.3f);
            }
        }

        public virtual void Jump()
        {
            if (_isAlive)
            {
                if (_mover != null)
                {
                    _mover.Jump(_currentJumpForce);
                }
                else
                {
                    Debug.Log("이 캐릭터는 점프할 수 없음.");
                }
            }
        }

        public virtual void Turn(Vector2 dir)
        {
            if (_isAlive)
            {
                if (_mover != null)
                {
                    _mover.Turn(dir);
                }
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Enter Collision : " + collision.gameObject.name);

            if (_isAlive)
            {
                if (gameObject.CompareTag(Constrants.TAG_PLAYER))
                {
                    _mover.ResetJumpCount();
                }
            }
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("Stay Collision : " + collision.gameObject.name);
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            Debug.Log("Exit Collision : " + collision.gameObject.name);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using BS.Common;

namespace BS.GameObjects
{
    public class AttachedDamageColider : MonoBehaviour
    {
        [SerializeField]
        private Collider2D _colider;
        public Collider2D Colider => _colider;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        private HashSet<AbstractCharacter> _colisionTargets = new HashSet<AbstractCharacter>();

        private CancellationTokenSource _timeCTS;

        private AbstractCharacter _ownerCharacter;
        public AbstractCharacter OwnerCharacter => _ownerCharacter;

        private float _meleeDamage;
        public float MeleeDamage => _meleeDamage;

        public AttachedDamageColider SetOwnerCharacter(AbstractCharacter character)
        {
            _ownerCharacter = character;
            return this;
        }

        public AttachedDamageColider SetMeleeDamage(float damage)
        {
            _meleeDamage = damage;
            return this;
        }

        public AttachedDamageColider SetActiveColider(bool isActive)
        {
            _colisionTargets.Clear();
            gameObject.SetActive(isActive);

            return this;
        }

        public AttachedDamageColider SetPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }

        public AttachedDamageColider SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
            return this;
        }

        public AttachedDamageColider SetSize(Vector2 size)
        {
            if (_colider is BoxCollider2D boxColider)
            {
                boxColider.size = size;
            }
            return this;
        }

        public AttachedDamageColider SetActiveTime(float time)
        {
            if (_timeCTS != null)
            {
                _timeCTS.Cancel();
                _timeCTS.Dispose();
                _timeCTS = null;
            }
            _timeCTS = new CancellationTokenSource();
            DeactiveTimer(time, _timeCTS.Token).Forget();
            return this;
        }

        public AttachedDamageColider SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            return this;
        }

        private UniTask DeactiveTimer(float time, CancellationToken token)
        {
            return UniTask.Delay(TimeSpan.FromSeconds(time), cancelImmediately: true, cancellationToken: token).ContinueWith(() =>
            {
                gameObject.SetActive(false);
            });
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Constrants.TAG_ENERMY))
            {
                if (collision.TryGetComponent<AbstractCharacter>(out var character))
                {
                    if (!_colisionTargets.Contains(character))
                    {
                        _colisionTargets.Add(character);
                        character.TakeDamage(_meleeDamage);

                        if (_timeCTS != null)
                        {
                            _timeCTS.Cancel();
                            _timeCTS.Dispose();
                            _timeCTS = null;
                        }
                    }
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            
        }
    }
}
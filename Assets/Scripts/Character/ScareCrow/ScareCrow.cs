using BS.System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

namespace BS.GameObjects
{
    public class ScareCrow : AbstractEnermy
    {
        private Tweener _twinkleTweener;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Attack() 
        {
            // DESC :: 허수아비는 공격기능이 없으므로 base호출 안함.
        }
        public override void Move(Vector2 direction)
        {
            base.Move(direction);
        }

        public override void Die()
        {
            //base.Die();
            if(_twinkleTweener != null && _twinkleTweener.IsActive())
            {
                _twinkleTweener.Kill(true);
                _spriteRenderer.color = Color.white;
            }
             _twinkleTweener = _spriteRenderer.DOColor(Color.clear, 0.1f)
            .SetLoops(3, LoopType.Yoyo)
            .OnComplete(() =>
            {
                UniTask.Create(async () =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
                    EnermySystem.Instance.GetEnemy("ScareCrow", this.transform.position);
                }).Forget();
                if (ParentPool != null)
                {
                     ParentPool.Release(this);
                }
                _twinkleTweener.Kill(true);
            });
        }

        public override void HitAnim()
        {
            //base.HitAnim(); DESC :: 허수아비는 별도의 애니메이션이 존재하지 않으므로 색상만 변경하므로 base호출 안함.

            if (_twinkleTweener != null && _twinkleTweener.IsActive())
            {
                _twinkleTweener.Kill(true);
            }
            _spriteRenderer.color = Color.white;
            TwinkleColor(Color.red);
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
        }

        public override void SetDefault()
        {
            base.SetDefault();

            if(_twinkleTweener != null && _twinkleTweener.IsActive())
            {
                _twinkleTweener.Kill(true);
            }
            _spriteRenderer.color = Color.white;
        }

        private void TwinkleColor(Color color, int repeatCount = 3)
        {
            _twinkleTweener = _spriteRenderer.DOColor(color, 0.1f)
                .SetLoops(repeatCount * 2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _spriteRenderer.color = Color.white;
                });
        }
    }
}
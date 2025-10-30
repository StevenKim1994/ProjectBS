using System;
using UnityEngine;
using UnityEngine.U2D;
using BS.Common;
using DG.Tweening;

namespace BS.GameObjects
{
    public class GoldCoinObject : AbstractRewardableObject
    {
        public override void Reward(Action rewardCallback = null)
        {
            base.Reward(rewardCallback);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
        }

        public override void Spawn()
        {
            base.Spawn();
            _spawnTweener = transform.DOMoveY(transform.position.y + 0.5f, 0.5f).SetEase(Ease.InOutSine)
                .OnStart(()=>
                {
                    _isRewarded = true; // DESC :: 스폰 연출중에는 획득안되도록 기믹 처리를 해야하므로
                })
                .OnComplete(()=>
                {
                    _isRewarded = false; // DESC :: 스폰 연출이 끝나면 획득 가능하도록 기믹 처리를 해제
                });
        }
    }
}
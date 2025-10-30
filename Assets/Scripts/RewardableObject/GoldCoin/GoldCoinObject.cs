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

        // 플레이어와의 트리거 감지만 처리 (스폰 연출 중에는 무시)
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isRewarded)
                return;

            base.OnTriggerEnter2D(collision);
        }

        public override void Spawn()
        {
            base.Spawn();

            // 스폰 연출 동안엔 물리 영향 제거(Kinematic), 끝나면 Dynamic으로 전환
            if (Rigidbody != null)
            {
                Rigidbody.linearVelocity = Vector2.zero;
                Rigidbody.angularVelocity = 0f;
                Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            }

            _spawnTweener = transform.DOMoveY(transform.position.y + 0.5f, 0.5f).SetEase(Ease.InOutSine)
                .OnStart(() =>
                {
                    _isRewarded = true;   // 스폰 연출 중 획득 방지
                })
                .OnComplete(() =>
                {
                    _isRewarded = false;  // 스폰 연출 종료 후 획득 가능
                    if (Rigidbody != null)
                    {
                        Rigidbody.bodyType = RigidbodyType2D.Dynamic; // 중력/바닥 물리 재개
                    }
                });
        }
    }
}
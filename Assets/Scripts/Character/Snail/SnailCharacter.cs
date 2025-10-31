using System;
using UnityEngine;
using BS.Common;
using BS.System;
using BS.UI;
using DG.Tweening;

namespace BS.GameObjects
{
    public class SnailCharacter : AbstractEnermy
    {
        public override void Move(Vector2 direction)
        {
            base.Move(direction);

            if (direction == Vector2.left)
            {
                _spriteRenderer.flipX = false;
            }
            else if (direction == Vector2.right)
            {
                _spriteRenderer.flipX = true;
            }
        }

        public override void Die()
        {
            base.Die();
        }
    }
}
using UnityEngine;
using UnityEngine.U2D;
using TMPro;
using DG.Tweening;
using BS.System;
using System;

namespace BS.GameObjects
{
    public class DamageFloating : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _floatingText;
        private Sequence _performTweenSeqence;

        private void Awake()
        {
            _floatingText.renderer.sortingOrder = 5000;
        }

        public DamageFloating SetText(string text)
        {
            _floatingText.SetText(text);

            return this;
        }

        public DamageFloating SetColor(Color color)
        {
            _floatingText.color = color;
            return this;
        }

        public DamageFloating SetSize(float size, float tweenSize)
        {
            _floatingText.fontSizeMax = tweenSize;
            _floatingText.fontSize = size;
            return this;
        }

        public DamageFloating SetPosition(Vector3 position)
        {
            this.transform.position = position;
         
            return this;
        }

        public DamageFloating StartTween(float duration = 1.5f, Ease easeFunc = Ease.OutCubic)
        {
            if(_performTweenSeqence != null)
            {
                _performTweenSeqence.Kill();
                _performTweenSeqence = null;
            }

            float targetY = transform.position.y + 1.5f;
            var moveTween = transform.DOMoveY(targetY, duration)
                .SetEase(easeFunc);

            var fontSizeTween = _floatingText.DOFontSize(_floatingText.fontSizeMax, duration)
                .SetEase(easeFunc);

            _performTweenSeqence = DOTween.Sequence()
                .Join(moveTween)
                .Join(fontSizeTween)
                .OnComplete(() =>
                {
                    DamageFloatingSystem.Instance.ReleaseDamageFloating(this);
                });

            return this;
        }
    }
}
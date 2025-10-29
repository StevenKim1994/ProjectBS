using UnityEngine;
using UnityEngine.U2D;
using TMPro;
using DG.Tweening;
using BS.System;

namespace BS.GameObjects
{
    public class DamageFloating : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _floatingText;

        private Tweener _performTweener;

        private void Awake()
        {
            _floatingText.renderer.sortingOrder = 5000;
        }

        public DamageFloating SetFloatingText(string text, Color color, float size)
        {
            _floatingText.text = text;
            _floatingText.color = color;
            _floatingText.fontSize = size;

            return this;
        }

        public DamageFloating SetPosition(Vector3 position)
        {
            this.transform.position = position;
         
            return this;
        }

        public DamageFloating StartTween()
        {
            _performTweener = this.transform.DOMoveY(this.transform.position.y + 1.5f, 0.5f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                DamageFloatingSystem.Instance.ReleaseDamageFloating(this);
            });
            return this;
        }
    }
}
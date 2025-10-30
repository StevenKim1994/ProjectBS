using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using BS.System;

namespace BS.UI
{
    public class TitlePresenter : AbstractUIPresenter<TitleView>
    {
        private CancellationTokenSource _delayCTS;

        protected override void PreShow()
        {
            View.LogoCanvasGroup.alpha = 0f;
            base.PreShow();
        }

        public override void Show()
        {
            PreShow();
            _viewShowTweener = View.LogoCanvasGroup.DOFade(1f, 1.5f)
                .SetEase(Ease.InOutSine)
                .SetUpdate(true)
                .OnComplete(()=>
                {
                    PostShow();
                });
        }

        protected override void PostShow()
        {
            base.PostShow();
        }

        public override void Hide()
        {
            TimeSystem.Instance.TimeSpeedUp(1.0f);
            _viewHideTweener = View.CanvasGroup.DOFade(0f, 0.5f).SetEase(View.HideEaseType).OnComplete(() =>
            {
                base.Hide();
            });
        }

        protected override void BindEvents()
        {
            base.BindEvents();
            View.StartButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Coffee.UIEffects;
using BS.System;
using UnityEngine.EventSystems;

namespace BS.UI
{
    public class TitlePresenter : AbstractUIPresenter<TitleView>
    {
        protected override void PreShow()
        {
            View.CanvasGroup.interactable = true;
            View.BackgroundUIEffect.SetRate(0f, UIEffectTweener.CullingMask.Transition);
            View.BackgroundUIEffectTweener.SetPause(true);
            View.BackgroundUIEffectTweener.ResetTime(UIEffectTweener.Direction.Forward);
            base.PreShow();
        }

        public override void Show()
        {
            PreShow();
            EventSystem.current.SetSelectedGameObject(View.StartButton.gameObject);
            PostShow();
        }

        protected override void PostShow()
        {
            base.PostShow();
        }

        public override void Hide()
        {
            View.CanvasGroup.interactable = false;
            TimeSystem.Instance.TimeSpeedUp(1.0f);
       
            View.BackgroundUIEffectTweener.PlayForward(true);
        }

        protected override void BindEvents()
        {
            base.BindEvents();
            View.BackgroundUIEffectTweener.onComplete.AddListener(() =>
            {
                View.gameObject.SetActive(false);
            });
            View.StartButton.onClick.AddListener(() =>
            {
                GameSequenceSystem.Instance.SetGameStepState(GameStepState.Playing);
                Hide();
            });
            View.ExitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}

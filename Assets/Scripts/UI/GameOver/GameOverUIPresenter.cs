using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Coffee.UIEffects;

namespace BS.UI
{
    public class GameOverUIPresenter : AbstractUIPresenter<GameOverUIView>
    {
        protected override void BindEvents()
        {
            base.BindEvents();
            _view.BackgroundUIEffectTweener.onComplete.AddListener(() =>
            {
                _view.GameOverText.gameObject.SetActive(true);
                _view.RestartButton.gameObject.SetActive(true);
            });
            _view.RestartButton.onClick.AddListener(OnClickRestart);
        }

        protected override void PreShow()
        {
            base.PreShow();
            _view.GameOverText.gameObject.SetActive(false);
            _view.RestartButton.gameObject.SetActive(false);

            View.BackgroundUIEffect.SetRate(0f, UIEffectTweener.CullingMask.Transition);
            View.BackgroundUIEffectTweener.SetPause(true);
            View.BackgroundUIEffectTweener.ResetTime(UIEffectTweener.Direction.Forward);
        }

        private void OnClickRestart()
        {

        }

        public override void Show()
        {
            base.Show();
 
            _view.BackgroundUIEffectTweener.Play(true);
        }

        public override void Hide()
        {
            base.Hide();

        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Coffee.UIEffects;
using BS.System;
using Unity.VisualScripting.FullSerializer;

namespace BS.UI
{
    public class TitlePresenter : AbstractUIPresenter<TitleView>
    {
        private const float SLIDE_TWEEN_DURATION = 0.35f;

        private bool isInputGuideOn = false;

        private Vector2 _buttonDefaultPos;
        private Vector2 _inputGuideDefaultPos;

        private float OffscreenX => Mathf.Max(Screen.width, Screen.height) + 200f;

        public override void Init(AbstractUIView bindView)
        {
            base.Init(bindView);
            _buttonDefaultPos = View.ButtonRect.anchoredPosition;
            _inputGuideDefaultPos = View.InputGuideRect.anchoredPosition;
        }

        protected override void PreShow()
        {
            InputControlSystem.Instance.UIInputMode = true;
            View.CanvasGroup.interactable = true;
            View.BackgroundUIEffect.SetRate(0f, UIEffectTweener.CullingMask.Transition);
            View.BackgroundUIEffectTweener.SetPause(true);
            View.BackgroundUIEffectTweener.ResetTime(UIEffectTweener.Direction.Forward);

            View.ButtonRect.DOKill();
            View.ButtonRect.anchoredPosition = isInputGuideOn
                ? _buttonDefaultPos + Vector2.left * OffscreenX
                : _buttonDefaultPos;
            View.ButtonRect.gameObject.SetActive(true);

            View.InputGuideRect.DOKill();
            View.InputGuideRect.anchoredPosition = isInputGuideOn
                ? _inputGuideDefaultPos
                : _inputGuideDefaultPos + Vector2.left * OffscreenX;
            View.InputGuideRect.gameObject.SetActive(isInputGuideOn);

            base.PreShow();
        }

        public override void Show()
        {
            PreShow();
            InputControlSystem.Instance.SetUISelectGameObjectSelected(View.StartButton.gameObject);
            InputControlSystem.Instance.UICancel.AddListener(() =>
            {
                OnOffInputGuide(false);
            });
            PostShow();
        }

        protected override void PostShow()
        {
            base.PostShow();
        }

        protected override void PreHide()
        {
            InputControlSystem.Instance.UICancel.RemoveAllListeners();
            base.PreHide();
        }

        public override void Hide()
        {
            View.CanvasGroup.interactable = false;
            TimeSystem.Instance.TimeSpeedUp(1.0f);
       
            View.BackgroundUIEffectTweener.PlayForward(true);
        }

        protected override void PostHide()
        {
            base.PostHide();
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
                InputControlSystem.Instance.UIInputMode = false;
                GameSequenceSystem.Instance.SetGameStepState(GameStepState.Playing);
                Hide();
            });
            View.InputGuideButton.onClick.AddListener(() =>
            {
                OnOffInputGuide(!isInputGuideOn);
            });
            View.ExitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }

        private void OnOffInputGuide(bool isOn)
        {
            View.ButtonRect.DOKill();
            View.InputGuideRect.DOKill();

            float offX = OffscreenX;

            if (isOn)
            {
                View.InputGuideRect.gameObject.SetActive(true);
                View.InputGuideRect.anchoredPosition = _inputGuideDefaultPos + Vector2.left * offX;
                View.InputGuideRect
                    .DOAnchorPos(_inputGuideDefaultPos, SLIDE_TWEEN_DURATION)
                    .SetUpdate(true)
                    .SetEase(Ease.OutCubic);

                View.ButtonRect
                    .DOAnchorPos(_buttonDefaultPos + Vector2.left * offX, SLIDE_TWEEN_DURATION)
                    .SetUpdate(true)
                    .SetEase(Ease.InCubic);
            }
            else
            {
                View.ButtonRect.gameObject.SetActive(true);
                View.ButtonRect
                    .DOAnchorPos(_buttonDefaultPos, SLIDE_TWEEN_DURATION)
                    .SetUpdate(true)
                    .SetEase(Ease.OutCubic);

                View.InputGuideRect
                    .DOAnchorPos(_inputGuideDefaultPos + Vector2.left * offX, SLIDE_TWEEN_DURATION)
                    .SetEase(Ease.InCubic)
                    .SetUpdate(true)
                    .OnComplete(() => View.InputGuideRect.gameObject.SetActive(false));
            }

            isInputGuideOn = isOn;
        }
    }
}

using BS.GameObjects;
using BS.System;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BS.UI
{
    public class HUDUIPresenter : AbstractUIPresenter<HUDUIView>
    {
        private const string KILL_COUNT_TEXT_FORMAT = "Kills: {0}";
        private const float KILL_FONT_DELTA = 8f;
        private const float KILL_FONT_UP_DURATION = 0.12f;
        private const float KILL_FONT_DOWN_DURATION = 0.12f;
        private const float HP_MINUS_DELAY = 0.15f;     // 지연 시간
        private const float HP_MINUS_DURATION = 0.35f;  // 따라오는 시간

        private Sequence _killCountFontSequence;     
        private Sequence _goldTextSequence;
        private Vector2 _goldIconImageBasicSize;
        private Tweener _hpMinusTweener;


        public override void Init(AbstractUIView bindView)
        {
            base.Init(bindView);

            _view.KillCountText.SetText(string.Format(KILL_COUNT_TEXT_FORMAT, DataSystem.Instance.PlayerHighScore));
            _goldIconImageBasicSize = _view.GoldIconImage.rectTransform.sizeDelta;

            var player = PlayerSystem.Instance.CurrentPlayer as AbstractCharacter;
            var currentHealth = player.CurrentHealth;
            _view.HPStateSlider.maxValue = player.Ability.MaxHealth;
            _view.HPStateSlider.value = currentHealth;
        }

        protected override void BindEvents()
        {
            base.BindEvents();

            _view.HPStateSlider.onValueChanged.AddListener(OnChangeHPStateSlider);
        }

        private void OnChangeHPStateSlider(float value)
        {
            var diffSize = 1f - _view.HPStateSlider.normalizedValue;
            if (_hpMinusTweener != null && _hpMinusTweener.IsActive())
            {
                _hpMinusTweener.Kill(true);
            }

            _view.HPStateMinusPerformImage.gameObject.SetActive(true);
            _hpMinusTweener = _view.HPStateMinusPerformImage.DOFillAmount(1f - diffSize, HP_MINUS_DURATION)
                .SetDelay(HP_MINUS_DELAY)
                .SetEase(Ease.OutQuad)
                .SetUpdate(false)
                .OnComplete(() =>
                 {
                     _view.HPStateMinusPerformImage.gameObject.SetActive(false);
                 });
        }

        public override void Show()
        {
            _view.HPStateMinusPerformImage.gameObject.SetActive(false);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void AddKillCount(int addCount)
        {
            if (_killCountFontSequence != null && _killCountFontSequence.IsActive())
            {
                _killCountFontSequence.Kill(true);
                _killCountFontSequence = null;
            }

            DataSystem.Instance.PlayerHighScore = DataSystem.Instance.PlayerHighScore + addCount;
            _view.KillCountText.SetText(string.Format(KILL_COUNT_TEXT_FORMAT, DataSystem.Instance.PlayerHighScore));

            float baseSize = _view.KillCountText.fontSize;
            float targetSize = baseSize + KILL_FONT_DELTA;

            _killCountFontSequence = DOTween.Sequence();
            _killCountFontSequence
                .Append(_view.KillCountText.DOFontSize(targetSize, KILL_FONT_UP_DURATION).SetEase(Ease.OutQuad))
                .Append(_view.KillCountText.DOFontSize(baseSize, KILL_FONT_DOWN_DURATION).SetEase(Ease.InQuad))
                .SetUpdate(false);
        }

        public Vector2 GetGoldImageScreenPos()
        {
            return RectTransformUtility.WorldToScreenPoint(null, _view.GoldIconImage.rectTransform.TransformPoint(_view.GoldIconImage.rectTransform.rect.center));
        }

        public void UpdateGoldText(int playerGold)
        {
            var rect = _view.GoldIconImage.rectTransform;

            if (_goldTextSequence != null && _goldTextSequence.IsActive())
            {
                _goldTextSequence.Kill(true); // 완료로 Kill하여 사이즈 원복 보장
                _goldTextSequence = null;
            }

            var targetSize = _goldIconImageBasicSize * 1.2f;

            _goldTextSequence = DOTween.Sequence()
                .Append(rect.DOSizeDelta(targetSize, 0.1f).SetEase(Ease.OutQuad))
                .Append(rect.DOSizeDelta(_goldIconImageBasicSize , 0.1f).SetEase(Ease.InQuad))
                .OnComplete(() =>
                {
                    _view.GoldIconImage.rectTransform.sizeDelta = _goldIconImageBasicSize;
                })
                .SetUpdate(false);

            _view.GoldAmountText.SetText(playerGold.ToString());
        }

        public void TakeDamage(float damage)
        {
            _view.HPStateSlider.value = _view.HPStateSlider.value - damage;
        }
    }
}
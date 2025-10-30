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

        private Sequence _killCountFontSequence;

        public override void Init(AbstractUIView bindView)
        {
            base.Init(bindView);

            _view.KillCountText.SetText(string.Format(KILL_COUNT_TEXT_FORMAT, DataSystem.Instance.PlayerHighScore));
        }

        public override void Show()
        {
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
            return RectTransformUtility.WorldToScreenPoint(null, _view.GoldIconImage.rectTransform.position);
        }
    }
}
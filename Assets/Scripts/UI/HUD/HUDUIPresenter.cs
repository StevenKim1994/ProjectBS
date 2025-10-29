using BS.System;
using UnityEngine;
using UnityEngine.UI;

namespace BS.UI
{
    public class HUDUIPresenter : AbstractUIPresenter<HUDUIView>
    {
        private const string KILL_COUNT_TEXT_FORMAT = "Kills: {0}";

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
            DataSystem.Instance.PlayerHighScore = DataSystem.Instance.PlayerHighScore+ addCount;
            _view.KillCountText.SetText(string.Format(KILL_COUNT_TEXT_FORMAT, DataSystem.Instance.PlayerHighScore));
        }
    }
}
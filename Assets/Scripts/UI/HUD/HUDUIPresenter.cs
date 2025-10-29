using UnityEngine;
using UnityEngine.UI;

namespace BS.UI
{
    public class HUDUIPresenter : AbstractUIPresenter<HUDUIView>
    {
        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void SetKillCount(int count)
        {
            _view.KillCountText.SetText($"Kills: {count}");
        }
    }
}
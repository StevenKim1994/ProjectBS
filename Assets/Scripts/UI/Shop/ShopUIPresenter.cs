using UnityEngine;
using BS.System;

namespace BS.UI
{
    public class ShopUIPresenter : AbstractUIPresenter<ShopUIView>
    {
        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            InputControlSystem.Instance.IsInput = true;
        }
    }
}
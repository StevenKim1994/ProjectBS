using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BS.Common;

namespace BS.UI
{
    [UIView(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_UI_HUD_PREFAB, false)]
    public class HUDUIView : AbstractUIView
    {
        [SerializeField]
        private TextMeshProUGUI _killCountText;
        public TextMeshProUGUI KillCountText => _killCountText;

        [SerializeField]
        private TextMeshProUGUI _goldAmountText;
        public TextMeshProUGUI GoldAmountText => _goldAmountText;

        [SerializeField]
        private Image _goldIconImage;
        public Image GoldIconImage => _goldIconImage;

        [SerializeField]
        private Slider _hpStateSlider;
        public Slider HPStateSlider => _hpStateSlider;

        [SerializeField]
        private Image _hpStateMinusPerformImage;
        public Image HPStateMinusPerformImage => _hpStateMinusPerformImage;
    }
}
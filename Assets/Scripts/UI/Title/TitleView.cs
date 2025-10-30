using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BS.Common;

namespace BS.UI
{
    [UIView(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_UI_TITLE_PREFAB)]
    public class TitleView : AbstractUIView
    {
        [SerializeField]
        private CanvasGroup _logoCanvasGroup;
        public CanvasGroup LogoCanvasGroup => _logoCanvasGroup;

        [SerializeField]
        private TextMeshProUGUI _titleText;
        public TextMeshProUGUI TitleText => _titleText;

        [SerializeField]
        private Button _startButton;
        public Button StartButton => _startButton;

    }
}
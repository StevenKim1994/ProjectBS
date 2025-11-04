using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Coffee.UIEffects;
using BS.Common;

namespace BS.UI
{
    [UIView(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_UI_TITLE_PREFAB)]
    public class TitleView : AbstractUIView
    {
        [SerializeField]
        private TextMeshProUGUI _titleText;
        public TextMeshProUGUI TitleText => _titleText;

        [SerializeField]
        private Button _startButton;
        public Button StartButton => _startButton;

        [SerializeField]
        private Button _inputGuideButton;
        public Button InputGuideButton => _inputGuideButton;

        [SerializeField]
        private Button _exitButton;
        public Button ExitButton => _exitButton;

        [SerializeField]
        private UIEffect _bgUIEffect;
        public UIEffect BackgroundUIEffect => _bgUIEffect;

        [SerializeField]
        private UIEffectTweener _bgTweener;
        public UIEffectTweener BackgroundUIEffectTweener => _bgTweener;

        [SerializeField]
        private RectTransform _buttonRect;
        public RectTransform ButtonRect => _buttonRect;

        [SerializeField]
        private RectTransform _inputGuideRect;
        public RectTransform InputGuideRect => _inputGuideRect;

    }
}
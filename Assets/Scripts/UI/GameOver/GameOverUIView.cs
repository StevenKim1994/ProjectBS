using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BS.Common;
using Coffee.UIEffects;

namespace BS.UI
{
    [UIView(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_UI_GAME_OVER_PREFAB)]
    public class GameOverUIView : AbstractUIView
    {
        [SerializeField]
        private TextMeshProUGUI _gameOverText;
        public TextMeshProUGUI GameOverText => _gameOverText;

        [SerializeField]
        private Button _restartButton;
        public Button RestartButton => _restartButton;

        [SerializeField]
        private UIEffect _backgroundUIEffect;
        public UIEffect BackgroundUIEffect => _backgroundUIEffect;

        [SerializeField]
        private UIEffectTweener _backgroundUIEffectTweener;
        public UIEffectTweener BackgroundUIEffectTweener => _backgroundUIEffectTweener;
    }
}
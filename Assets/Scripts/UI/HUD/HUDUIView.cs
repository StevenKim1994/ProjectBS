using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BS.Common;

namespace BS.UI
{
    [UIView(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_UI_HUD_PREFAB)]
    public class HUDUIView : AbstractUIView
    {
        [SerializeField]
        private TextMeshProUGUI _killCountText;
        public TextMeshProUGUI KillCountText => _killCountText;
    }
}
using UnityEngine;
using UnityEngine.UI;
using BS.Common;

namespace BS.UI
{
    [UIView(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_PAUSE_UI_PREFAB)]
    public class PauseUIView : AbstractUIView
    {
        [SerializeField]
        private RectTransform _markRectTransform;
        public RectTransform MarkRectTransform => _markRectTransform;

        [SerializeField]
        private UIButton _resumeButton;
        public UIButton ResumeButton => _resumeButton;

    }
}
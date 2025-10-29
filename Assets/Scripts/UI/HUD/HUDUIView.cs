using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BS.UI
{
    public class HUDUIView : AbstractUIView
    {
        [SerializeField]
        private TextMeshProUGUI _killCountText;
        public TextMeshProUGUI KillCountText => _killCountText;

    }
}
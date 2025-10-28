using UnityEngine;
using UnityEngine.UI;

namespace BS.UI
{
    public class AbstractUIView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rootRect;

        public RectTransform RootRect => _rootRect;
    }
}
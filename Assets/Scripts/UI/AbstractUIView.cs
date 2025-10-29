using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Reflection;

namespace BS.UI
{
    public abstract class AbstractUIView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rootRect;

        public RectTransform RootRect => _rootRect;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup => _canvasGroup;

        [SerializeField]
        private Ease _showEaseType = Ease.OutBack;
        public Ease ShowEaseType => _showEaseType;

        [SerializeField]
        private Ease _hideEaseType = Ease.InBack;
        public Ease HideEaseType => _hideEaseType;
    }
}
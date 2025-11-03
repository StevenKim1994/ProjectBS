using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace BS.UI
{
    public interface IUIPresenter
    {
        void Init(AbstractUIView bindView);
        void Show();
        void Hide();
        bool IsInit();
        bool IsShowing();
        IUIPresenter SetParentCanvas(Canvas mainCanvas);

        string GetViewPrefabPath();
        void SetHiearchy(int v);
    }

    public abstract class AbstractUIPresenter<T> : IUIPresenter where T : AbstractUIView
    {
        protected T _view;
        public T View => _view;

        protected Tweener _viewShowTweener;
        protected Tweener  _viewHideTweener;
        protected bool _isInit = false;
        protected Canvas _parentCanvas;
        protected string _viewPrefabPath;

        public virtual IUIPresenter SetParentCanvas(Canvas canvas)
        {
            _parentCanvas = canvas;
            _view.transform.SetParent(_parentCanvas.transform);

            if(_view.gameObject.TryGetComponent<RectTransform>(out var rectTransform))
            {
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = Vector2.zero;
                rectTransform.localScale = Vector2.one;
            }

            return this;
        }

        public string GetViewPrefabPath()
        {
            if (string.IsNullOrEmpty(_viewPrefabPath))
            {
                var viewType = typeof(T);
                var custumAttribute = viewType.GetCustomAttribute<UIViewAttribute>();
                if (custumAttribute != null)
                {
                    _viewPrefabPath = custumAttribute.AddressablePath;
                }
            }

            return _viewPrefabPath;
        }

        public bool IsInit()
        {
            return _isInit;
        }


        public virtual void Init(AbstractUIView bindView)
        {
            _view = bindView as T;
            BindEvents();
            _isInit = true;
        }

        protected virtual void BindEvents()
        {

        }

        public virtual bool IsShowing()
        {
            return _view.gameObject.activeSelf;
        }

        public virtual void Show()
        {
            if (_view.CanvasGroup != null)
            {
                _viewShowTweener = DOTween.To(() => _view.CanvasGroup.alpha, x => _view.CanvasGroup.alpha = x, 1f, 0.5f)
                    .OnStart(() =>
                    {
                        PreShow();
                    })
                    .SetEase(_view.ShowEaseType)
                    .OnComplete(() =>
                    {
                        PostShow();
                    });
            }
            else
            {
                PreShow();
                PostShow();
            }
        }

        protected virtual void PreShow()
        {
            View.gameObject.SetActive(true);
            if(_viewHideTweener != null && _viewHideTweener.IsActive())
            {
                _viewHideTweener.Kill(false);
                _viewHideTweener = null;
            }

            if (_viewShowTweener != null && _viewShowTweener.IsActive())
            {
                _viewShowTweener.Kill();
            }
        }

        protected virtual void PostShow()
        {
            _viewShowTweener.Kill();
            _viewShowTweener = null;
        }

        public virtual void Hide()
        {
            if (_view.CanvasGroup != null)
            {
                _viewHideTweener = DOTween.To(() => _view.CanvasGroup.alpha, x => _view.CanvasGroup.alpha = x, 0f, 0.5f)
                    .OnStart(() =>
                    {
                        PreHide();
                    })
                    .SetEase(_view.HideEaseType)
                    .OnComplete(() =>
                    {
                        PostHide();
                    });
            }
            else
            {
                PreHide();
                PostHide();
            }
        }

        protected virtual void PreHide()
        {
            if(_viewShowTweener != null && _viewShowTweener.IsActive())
            {
                _viewShowTweener.Kill(false);
                _viewShowTweener = null;
            }

            if (_viewHideTweener != null && _viewHideTweener.IsActive())
            {
                _viewHideTweener.Kill();
            }
        }

        protected virtual void PostHide()
        {
            if (_viewShowTweener != null && _viewShowTweener.IsActive())
            {
                _viewShowTweener.Kill(false);
                _viewShowTweener = null;
            }

            if (_viewHideTweener != null && _viewHideTweener.IsActive())
            {
                _viewHideTweener.Kill();
            }

            View.gameObject.SetActive(false);
        }

        public void SetHiearchy(int index)
        {
            View.transform.SetSiblingIndex(index);
        }
    }
}
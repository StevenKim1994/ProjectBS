using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using BS.Common;
using BS.UI;
using BS.GameObjects;
using System.Reflection;
using Unity.VisualScripting;

namespace BS.System
{
    public class UISystem : ISystem
    {
        private static UISystem _instance;
        private Canvas _mainCanvas;
        public Canvas MainCanvas => _mainCanvas;

        private Image _flashImage;

        private Tweener _flashScreenTweener;

        private Dictionary<Type, List<IUIPresenter>> _presenters = new Dictionary<Type, List<IUIPresenter>>();

        public static UISystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<UISystem>();
                }

                return _instance;
            }
        }

        public void Load()
        {
            _mainCanvas = SystemGameObject.Instance.CurrentGameScene.MainCanvas;
            _flashImage = _mainCanvas.transform.Find(Constrants.NAME_FLASHIMAGE).GetComponent<Image>();
        }

        public void Unload()
        {

        }

        public void FlashScreen(Color color, float duration = 3f)
        {
            if (_flashScreenTweener != null && _flashScreenTweener.IsActive())
            {
                return;
            }

            _flashImage.gameObject.SetActive(true);
            _flashScreenTweener = DOTween.ToAlpha(
                () => color,
                x => color = x,
                0f,
                duration
            ).OnUpdate(() =>
            {
                _flashImage.color = color;
            }).OnComplete(() =>
            {
                _flashImage.color = Color.white;
                _flashImage.gameObject.SetActive(false);
            });
        }

        public TPresenter Show<TPresenter>() where TPresenter : class, IUIPresenter, new()
        {
            TPresenter presenter = null;

            if(_presenters.ContainsKey(typeof(TPresenter)) == false)
            {
                Type type = typeof(TPresenter);
                var attributes = type.GetCustomAttribute<UIViewAttribute>();

                GameObject viewObject = ResourceSystem.Instance.GetLoadGameObject(attributes.AddressablePath);
                if (viewObject != null)
                {
                    var clone = GameObject.Instantiate(viewObject);
                    if(clone.TryGetComponent<AbstractUIView>(out var existingView))
                    {
                        presenter.Init(existingView);
                    }
                }
                _presenters.Add(type, new List<IUIPresenter>() { presenter });
            }
            else
            {
                presenter = _presenters[typeof(TPresenter)][0] as TPresenter;
            }

            if(!presenter.IsInit())
            {
                Debug.LogWarning("초기화가 되어 있지 않음. 해당 View의 Attribute 체크 필요");
            }

            presenter.SetParentCanvas(_mainCanvas);
            presenter.Show();

            return presenter;
        }

        public TPresenter Hide<TPresenter>() where TPresenter : class, IUIPresenter
        {
            TPresenter presenter = null;
            Type type = typeof(TPresenter);

            if (_presenters.TryGetValue(type, out var result))
            {
                presenter = result[0] as TPresenter;
            }

            if(presenter != null)
            {
                presenter.Hide();
            }
            else
            {
                Debug.LogWarning("현재 열려있지 않음");
            }
            return presenter;
        }

        public bool IsShowing<TPresenter>() where TPresenter : class, IUIPresenter
        {
            Type type = typeof(TPresenter);
            if (_presenters.TryGetValue(type, out var result))
            {
                if (result[0] is TPresenter presenter)
                {
                    return presenter.IsShowing();
                }
            }

            return false;
        }
    }
}
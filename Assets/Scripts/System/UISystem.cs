using BS.GameObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using BS.Common;
using BS.UI;
//using BS.UI;

namespace BS.System
{
    public class UISystem : ISystem
    {
        private static UISystem _instance;
        private Canvas _mainCanvas;
        public Canvas MainCanvas => _mainCanvas;

        private Image _flashImage;

        private Tweener _flashScreenTweener;

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
            if(_flashScreenTweener != null && _flashScreenTweener.IsActive())
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
            }).OnComplete(()=>
            {
                _flashImage.color = Color.white;
                _flashImage.gameObject.SetActive(false);
            });
        }

        public T Show<T>() 
        {
            T presenter = default(T);

            return presenter;
        }

        public T Hide<T>()
        {
            T presenter = default(T);

            return presenter;
        }

        public bool IsShowing<T>()
        {
            return false;
        }
    }
}
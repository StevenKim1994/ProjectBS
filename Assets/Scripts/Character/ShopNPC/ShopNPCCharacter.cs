using UnityEngine;
using DG.Tweening;
using R3;
using BS.Common;
using BS.System;
using System;
using BS.UI;
using UnityEngine.InputSystem;

namespace BS.GameObjects
{
    public class ShopNPCCharacter : AbstractNPC
    {
        private Tweener _arrowTweener;
        private Vector2 _originArrowPos;

        protected override void Awake()
        {
            base.Awake();
            _originArrowPos = _notiArrowRender.transform.localPosition;
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            if(_notiArrowRender != null && _isInPlayerRange)
            {
                _notiArrowRender.gameObject.SetActive(true);
                _notiArrowRender.transform.localPosition = _originArrowPos;
                _arrowTweener = _notiArrowRender.transform.DOLocalMoveY(0.3f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                PlayerSystem.Instance.SetInteractObject(this);
            }
        }

        protected override void OnTriggerStay2D(Collider2D collision)
        {
            base.OnTriggerStay2D(collision);
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);

            if(_notiArrowRender != null)
            {
                _arrowTweener.Kill();
                _notiArrowRender.gameObject.SetActive(false);
            }

            PlayerSystem.Instance.SetInteractObject(null);
            UISystem.Instance.Hide<ShopUIPresenter>();
        }

        public override void Interact()
        {
            base.Interact();

            if (_isInPlayerRange)
            {
                InputControlSystem.Instance.IsInput = false;
                UISystem.Instance.Show<ShopUIPresenter>();
            }
        }
    }
}
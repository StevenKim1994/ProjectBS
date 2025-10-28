using UnityEngine;
using UnityEngine.UI;

namespace BS.UI
{
    public class AbstractUIPresenter
    {
        public virtual void Show()
        {

        }

        protected virtual void PreShow()
        {

        }

        protected virtual void PostShow()
        {

        }

        public virtual void Hide()
        {

        }

        protected virtual void PreHide()
        {

        }

        protected virtual void PostHide()
        {

        }
    }
}
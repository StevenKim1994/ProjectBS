using System;
using UnityEngine;
using UnityEngine.UI;
using BS.System;

namespace BS.UI
{
    public class PauseUIPresenter : AbstractUIPresenter<PauseUIView>
    {
        protected override void BindEvents()
        {
            base.BindEvents();
            View.ResumeButton.onClick.AddListener(OnResumeButtonClicked);
        }

        private void OnResumeButtonClicked()
        {
            GameSequenceSystem.Instance.SetGameStepState(GameStepState.Playing);
        }

        protected override void PreShow()
        {
            base.PreShow();
            InputControlSystem.Instance.SetUISelectGameObjectSelected(View.ResumeButton.gameObject);
        }
    }
}
using UnityEngine;
using BS.GameObjects;
using BS.System;
using BS.Common;
using BS.UI;

namespace BS
{
    public class InGameScene : AbstractInGameScene
    {
        protected override void Start()
        {
            base.Start();
            Application.targetFrameRate = 60;
            TimeSystem.Instance.TimeSpeedUp(0.0f);

            SoundSystem.Instance.SetUIAudioSource(UISystem.Instance.MainCanvas.GetComponent<AudioSource>());
            UISystem.Instance.Show<TitlePresenter>();

            ScreenSystem.Instance.CinemachineCamera.Target.TrackingTarget = InputControlSystem.Instance.CurrentPlayableTransform;
        }
    }
}
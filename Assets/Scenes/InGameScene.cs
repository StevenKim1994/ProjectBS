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

            EnermySystem.Instance.GetEnemy(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_SNAIL_PREFAB, new Vector3(4.19999981f, -4.03000021f, 0f));

            TimeSystem.Instance.TimeSpeedUp(0.0f);

            SoundSystem.Instance.SetUIAudioSource(UISystem.Instance.MainCanvas.GetComponent<AudioSource>());
            UISystem.Instance.Show<TitlePresenter>();
        }
    }
}
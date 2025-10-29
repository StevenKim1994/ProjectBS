using BS.GameObjects;
using BS.System;
using BS.Common;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InGameScene : AbstractInGameScene
{
    protected override void Start()
    {
        base.Start();

        EnermySystem.Instance.GetEnemy(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_SCARE_CROW_PREFAB, new Vector3(4.19999981f, -4.03000021f, 0f));

        EnermySystem.Instance.GetEnemy(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_SCARE_CROW_PREFAB, new Vector3(3.9f, -4.03000021f, 0f));

        EnermySystem.Instance.GetEnemy(AddressablePathConstants.DefaultLocalGroup.ASSETS_ADDRESS_RESOURCE_SCARE_CROW_PREFAB, new Vector3(4.29999981f, -4.03000021f, 0f));
    }
}

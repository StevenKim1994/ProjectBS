using BS.GameObject;
using BS.System;
using BS.Common;
using UnityEngine;

public class InGameScene : MonoBehaviour
{
    private void Awake()
    {
        SystemGameObject.Instance.LoadAllSystems();
    }

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Constrants.LAYER_ACTOR), LayerMask.NameToLayer(Constrants.LAYER_ACTOR), true);

        var player = FindFirstObjectByType<NightCharacter>();
        InputControlSystem.Instance.SetPlayableCharacter(player);
        InputControlSystem.Instance.SetInputActionAsset(player.InputActionAsset);
    }
}

using BS.GameObject;
using BS.System;
using UnityEngine;

public class InGameScene : MonoBehaviour
{
    private void Awake()
    {
        SystemGameObject.Instance.LoadAllSystems();
    }

    private void Start()
    {
        var player = FindFirstObjectByType<AbstractCharacter>();
        InputControlSystem.Instance.SetPlayableCharacter(player);
        InputControlSystem.Instance.SetInputActionAsset(player.InputActionAsset);
    }
}

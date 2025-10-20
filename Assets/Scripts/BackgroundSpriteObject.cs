using System;
using System.Collections.Generic;   
using UnityEngine;
using BS.System;

public class BackgroundSpriteObject : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _backgroundSprite;

    [SerializeField]
    private List<SpriteRenderer> _layerSpriteList = new List<SpriteRenderer>();

    public Sprite BackgroundSprite
    {
        get => _backgroundSprite.sprite;
    }

    public List<SpriteRenderer> LayerSpriteList
    {
        get => _layerSpriteList;
    }

    private void Awake()
    {
        _backgroundSprite = TryGetComponent<SpriteRenderer>(out var spriteRenderer) ? spriteRenderer : null;
    }

    private void Start()
    {
        if(_backgroundSprite != null)
        {
            _backgroundSprite.size = SystemGameObject.Instance.GetSystem<ScreenSystem>().GetScreenSize();
        }
    }


}


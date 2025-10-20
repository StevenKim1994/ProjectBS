using UnityEngine;
using UnityEngine.Sprites;

public class SpriteObject : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    public Sprite Sprite
    {
        get => _spriteRenderer.sprite;
    }
}

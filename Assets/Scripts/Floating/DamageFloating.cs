using UnityEngine;
using UnityEngine.U2D;
using TMPro;

namespace BS.GameObjects
{
    public class DamageFloating : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _floatingText;

        public DamageFloating SetFloatingText(string text, Color color, float size)
        {
            _floatingText.text = text;
            _floatingText.color = color;
            _floatingText.fontSize = size;

            return this;
        }

        public DamageFloating SetPosition(Vector3 position)
        {
            this.transform.position = position;
         
            return this;
        }
    }
}
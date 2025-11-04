using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace BS.UI
{
    public class UIButton : Button
    {
        private UnityEvent<float> _buttonPressEvent = new UnityEvent<float>();
        public UnityEvent<float> ButtonPressEvent => _buttonPressEvent;

        [SerializeField, Min(0f)]
        private float _pressEventInterval = 0f; // 초 단위. 0이면 매 프레임 전송

        private bool _isPressed;
        private float _pressStartUnscaledTime;
        private float _lastInvokeUnscaledTime;

        public float CurrentPressDurationUnscaled => _isPressed ? Time.unscaledTime - _pressStartUnscaledTime : 0f;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (_isPressed)
            {
                float now = Time.unscaledTime;

                if (_pressEventInterval <= 0f || now - _lastInvokeUnscaledTime >= _pressEventInterval)
                {
                    _lastInvokeUnscaledTime = now;
                    float duration = now - _pressStartUnscaledTime;
                    _buttonPressEvent.Invoke(duration);
                }
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _isPressed = false;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _isPressed = true;
            _pressStartUnscaledTime = Time.unscaledTime;
            _lastInvokeUnscaledTime = _pressStartUnscaledTime; 
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (_isPressed)
            {
                float duration = Time.unscaledTime - _pressStartUnscaledTime;
                _buttonPressEvent.Invoke(duration);
                _isPressed = false;
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }
    }
}
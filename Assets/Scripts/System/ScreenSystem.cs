using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using BS.GameObjects;
using System;


namespace BS.System
{
    public class ScreenSystem : ISystem
    {
        private static ScreenSystem _instance;
        public static ScreenSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<ScreenSystem>();
                }
                return _instance;
            }
        }

        private PixelPerfectCamera _pixelPerfectCamera;
        private Camera _camera;
        private Sequence _cameraZoomInSeq;

        public void Load()
        {
            _pixelPerfectCamera = SystemGameObject.Instance.CurrentGameScene.PixelPerfectCamera;
            _camera = _pixelPerfectCamera.TryGetComponent<Camera>(out var cam) ? cam : null;
        }

        public void Unload()
        {

        }

        public Vector2 GetScreenSize()
        {
            float width = Screen.width;
            float height = Screen.height;

            float currentAspect = width / height;
            float targetAspect = 16f / 9f;

            Vector2 screenSize;

            if (currentAspect > targetAspect)
            {
                // 화면이 더 넓은 경우: 높이 기준으로 폭 조정
                screenSize = new Vector2(height * targetAspect, height);
            }
            else
            {
                // 화면이 더 좁거나 같은 경우: 폭 기준으로 높이 조정
                screenSize = new Vector2(width, width / targetAspect);
            }

            return screenSize;
        }

        public void ShakeCamera(float duration, float strength, int vibrato)
        {
            if (_camera != null)
            {
                var originalPosition = _camera.transform.position;
                _camera.transform.DOShakePosition(duration, strength, vibrato).OnComplete(() =>
                {
                    _camera.transform.position = originalPosition;
                });
            }
        }

        public void ZoomInCamera(Vector3 position, float zoomValue, float duration)
        {
            // TODO :: 수정필요
            if(_cameraZoomInSeq != null && _cameraZoomInSeq.IsActive())
            {
                _cameraZoomInSeq.Kill(true);
                _cameraZoomInSeq = null;
            }

            if (_camera != null && _pixelPerfectCamera != null)
            {
                var originalOrthoSize = _camera.orthographicSize;
                var targetOrthoSize = originalOrthoSize * zoomValue;
                var originalPosition = _camera.transform.position;

                _cameraZoomInSeq = DOTween.Sequence()
                    .Append(_camera.transform.DOMove(new Vector3(position.x, position.y, originalPosition.z), duration))
                    .Join(DOTween.To(() => _camera.orthographicSize, x => _camera.orthographicSize = x, targetOrthoSize, duration))
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        _camera.transform.position = originalPosition;
                        _camera.orthographicSize = originalOrthoSize;
                    });
            }
        }
    }
}
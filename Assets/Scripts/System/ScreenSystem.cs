using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace BS.System
{
    public class ScreenSystem : ISystem
    {
        private PixelPerfectCamera _pixelPerfectCamera;
        private Camera _camera;

        public void Load()
        {
            _pixelPerfectCamera = SystemGameObject.Instance.PixelPerfectCamera;
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

    }
}
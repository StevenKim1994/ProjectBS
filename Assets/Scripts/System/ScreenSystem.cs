using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;
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


        public Camera WorldCamera => _camera;

        public CinemachineCamera CinemachineCamera => _cinemachineCamera;

        private PixelPerfectCamera _pixelPerfectCamera;
        private Camera _camera;
        private Sequence _cameraZoomInSeq;
        private CinemachineCamera _cinemachineCamera;
        private CinemachineConfiner2D _cinemachineConfiner2D;
        private CinemachinePositionComposer _cinemachinePositionComposer;
        private CinemachineBasicMultiChannelPerlin _cinemachineNoise;
        private Sequence _cameraShakeSeq;

        public void Load()
        {
            _pixelPerfectCamera = SystemGameObject.Instance.CurrentGameScene.PixelPerfectCamera;
            _camera = _pixelPerfectCamera.TryGetComponent<Camera>(out var cam) ? cam : null;
            _cinemachineCamera = SystemGameObject.Instance.CurrentGameScene.CinemachineCamera;
            _cinemachineConfiner2D = _cinemachineCamera.GetComponent<CinemachineConfiner2D>();
            _cinemachinePositionComposer = _cinemachineCamera.GetComponent<CinemachinePositionComposer>();
            _cinemachineNoise = _cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Unload()
        {

        }

        public void ShakeCamera(float duration, float strength, int vibrato)
        {
            if (_cameraShakeSeq != null && _cameraShakeSeq.IsActive())
            {
                _cameraShakeSeq.Kill();
                _cameraShakeSeq = null;
            }

            float originalAmp = _cinemachineNoise.AmplitudeGain;
            float originalFreq = _cinemachineNoise.FrequencyGain;

            float targetAmp = Mathf.Max(0f, strength);
            float targetFreq = Mathf.Max(0.1f, vibrato);

            _cinemachineNoise.AmplitudeGain = targetAmp;
            _cinemachineNoise.FrequencyGain = targetFreq;

            _cinemachineNoise.enabled = true;
            _cameraShakeSeq = DOTween.Sequence();
            _cameraShakeSeq.Append(
                DOTween.To(
                    () => _cinemachineNoise.AmplitudeGain,
                    v => _cinemachineNoise.AmplitudeGain = v,
                    0f,
                    Mathf.Max(0.01f, duration)
                ).SetEase(Ease.OutSine)
            );
            _cameraShakeSeq.Join(
                DOTween.To(
                    () => _cinemachineNoise.FrequencyGain,
                    v => _cinemachineNoise.FrequencyGain = v,
                    Mathf.Max(0.05f, originalFreq),
                    Mathf.Max(0.01f, duration)
                ).SetEase(Ease.OutSine)
            );

            _cameraShakeSeq.OnComplete(() =>
            {
                _cinemachineNoise.AmplitudeGain = originalAmp;
                _cinemachineNoise.FrequencyGain = originalFreq;
                _cameraShakeSeq = null;
                _cinemachineNoise.enabled = false;
            });
        }
    }
}
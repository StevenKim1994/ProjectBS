using System;
using System.Collections.Generic;   
using UnityEngine;
using BS.System;

namespace BS.GameObjects
{
    public class BackgroundSpriteObject : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _backgroundSprite;

        [SerializeField]
        private List<SpriteRenderer> _layerSpriteList = new List<SpriteRenderer>();

        [SerializeField]
        private Transform _targetTransform;

        [SerializeField]
        private bool _isParallaxEnabled = true;

        [Header("Parallax Settings")]
        [SerializeField]
        [Tooltip("Parallax factor for each layer (0 = no movement, 1 = full movement)")]
        private List<float> _parallaxFactors = new List<float>();

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Overall parallax intensity")]
        private float _parallaxIntensity = 0.3f;

        [SerializeField]
        [Tooltip("Smooth damping time for layer movement")]
        private float _smoothTime = 0.3f;

        private Vector3 _previousTargetPosition;
        private List<Vector3> _layerVelocities = new List<Vector3>();
        private List<Vector3> _layerTargetPositions = new List<Vector3>();
        private List<Vector3> _layerInitialPositions = new List<Vector3>();

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
            InitializeParallax();
        }

        private void Start()
        {
            _targetTransform = InputControlSystem.Instance.CurrentPlayableTransform;

            if (_targetTransform != null)
            {
                _previousTargetPosition = _targetTransform.position;
            }
        }

        private void Update()
        {
            if (_isParallaxEnabled)
            {
                if (_targetTransform != null)
                {
                    ApplyParallaxEffect();
                }
            }
        }

        private void InitializeParallax()
        {
            for (int i = 0; i < _layerSpriteList.Count; ++i)
            {
                _layerVelocities.Add(Vector3.zero);
                _layerTargetPositions.Add(Vector3.zero);
   
                if (_layerSpriteList[i] != null)
                {
                    _layerInitialPositions.Add(_layerSpriteList[i].transform.localPosition);
                }
                else
                {
                    _layerInitialPositions.Add(Vector3.zero);
                }

                if (_parallaxFactors.Count <= i)
                {
                    float factor = (float)(i + 1) / (_layerSpriteList.Count + 1);
                    _parallaxFactors.Add(factor);
                }
            }
        }

        private void ApplyParallaxEffect()
        {
            Vector3 currentTargetPosition = _targetTransform.position;
            Vector3 deltaMovement = currentTargetPosition - _previousTargetPosition;

            for (int i = 0; i < _layerSpriteList.Count; i++)
            {
                if (_layerSpriteList[i] == null) continue;

                // Calculate parallax offset based on layer's parallax factor
                float parallaxFactor = _parallaxFactors[i] * _parallaxIntensity;
                Vector3 parallaxOffset = new Vector3(
                    deltaMovement.x * parallaxFactor,
                    deltaMovement.y * parallaxFactor,
                    0f
                );

                // Update target position for this layer
                _layerTargetPositions[i] = _layerInitialPositions[i] - parallaxOffset;

                // Smooth damp to target position for arcade-style smooth movement
                Vector3 currentPos = _layerSpriteList[i].transform.localPosition;
                Vector3 velocity = _layerVelocities[i];
                Vector3 newPos = Vector3.SmoothDamp(
                    currentPos,
                    _layerTargetPositions[i],
                    ref velocity,
                    _smoothTime
                );

                _layerVelocities[i] = velocity;
                _layerSpriteList[i].transform.localPosition = newPos;
            }

            _previousTargetPosition = currentTargetPosition;
        }

        public void ResetParallax()
        {
            if (_targetTransform != null)
            {
                _previousTargetPosition = _targetTransform.position;
            }

            for (int i = 0; i < _layerSpriteList.Count; i++)
            {
                if (_layerSpriteList[i] != null)
                {
                    _layerSpriteList[i].transform.localPosition = _layerInitialPositions[i];
                    _layerTargetPositions[i] = _layerInitialPositions[i];
                    _layerVelocities[i] = Vector3.zero;
                }
            }
        }
    }
}
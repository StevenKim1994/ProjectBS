using UnityEngine;
using BS.Common;
using System;
using UnityEngine.Events;

namespace BS.GameObject
{
    public class AbstractCharacterMover : MonoBehaviour, ICharacterMover
    {
        public Vector2 ViewDirection => _viewDirection;
        public float Velocity => _velocity;

        public UnityAction<Vector2> OnMove => _onMoveEvent;

        private UnityAction<Vector2> _onMoveEvent;
        private Transform _moveTargetTransform;

        private Vector2 _viewDirection;
        private float _velocity;
        private Vector2 _currentVelocity;

        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _deceleration = 8f;
        [SerializeField] private float _velocityPower = 0.9f;


        private void Awake()
        {
            _moveTargetTransform = this.transform;
        }

        public virtual void Jump(float force)
        {

        }

        public virtual void Move(Vector2 direction, float speed)
        {
            _viewDirection = direction;

        }

        public virtual void Stop()
        {
            // Gradually stop with deceleration
            _currentVelocity = Vector2.MoveTowards(
                _currentVelocity,
                Vector2.zero,
                _deceleration * Time.fixedDeltaTime * 2f
            );
        }
    }
}
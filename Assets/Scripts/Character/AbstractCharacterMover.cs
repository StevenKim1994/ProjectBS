using UnityEngine;
using BS.Common;
using System;
using UnityEngine.Events;

namespace BS.GameObjects
{
    public class AbstractCharacterMover : MonoBehaviour, ICharacterMover
    {
        public Vector2 ViewDirection => _viewDirection;
        public float Velocity => _velocity;

        private Transform _moveTargetTransform;

        private Vector2 _viewDirection;
        private float _velocity; // current horizontal velocity for platformer movement

        [SerializeField] private float _acceleration =10f;
        [SerializeField] private float _deceleration =8f;
        [SerializeField] private float _velocityPower =0.9f;

        // Input-driven movement state
        private float _inputX; // last horizontal input set by Move/Stop (not per-frame)
        private float _moveSpeed; // last requested move speed

        // Sliding tuning
        [SerializeField] private float _stopThreshold =0.02f; // snap-to-stop threshold for velocity

        private void Awake()
        {
            _moveTargetTransform = this.transform;
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        private void Tick(float dt)
        {
            bool hasInput = Mathf.Abs(_inputX) >0.0001f;

            float targetSpeed = _inputX * Mathf.Max(0f, _moveSpeed);

            float accelRate = hasInput ? _acceleration : _deceleration;

            float speedDiff = targetSpeed - _velocity;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, _velocityPower) * Mathf.Sign(speedDiff);

            _velocity += movement * dt;

            if (!hasInput && Mathf.Abs(_velocity) <= _stopThreshold)
            {
                _velocity =0f;
            }

            var pos = _moveTargetTransform.position;
            pos.x += _velocity * dt;
            _moveTargetTransform.position = pos;
        }

        public virtual void Jump(float force)
        {
            // TODO :: viewDirection 방향으로 포물선 운동
        }

        public virtual void Move(Vector2 direction, float speed)
        {
            // Update view direction (keep last non-zero)
            if (direction.sqrMagnitude >0.0001f)
            {
                _viewDirection = direction.normalized;
            }

            // Cache input and speed; actual movement happens per-frame in Tick()
            _inputX = Mathf.Clamp(direction.x, -1f,1f);
            _moveSpeed = speed;
        }

        public virtual void Stop()
        {
            _inputX = 0f;
        }

        public virtual void Turn(Vector2 dir)
        {
            if(_viewDirection != dir.normalized)
            {
                Stop();
                _viewDirection = dir.normalized;
            }
        }

        public virtual void Slow(float slowValue)
        {
            slowValue = Mathf.Abs(slowValue);
            if (_moveSpeed > 0)
            {
                _moveSpeed *= slowValue;
            }

            // DESC :: 현재 이동속도에 슬로우 계수로 처리함.
        }
    }
}
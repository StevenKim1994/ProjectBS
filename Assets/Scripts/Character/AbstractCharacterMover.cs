using UnityEngine;
using BS.Common;
using System;
using UnityEngine.Events;

namespace BS.GameObjects
{
    public abstract class AbstractCharacterMover : MonoBehaviour, IMover
    {
        public Vector2 ViewDirection => _viewDirection;
        public float Velocity => _velocity;

        private Transform _moveTargetTransform;
        private Rigidbody2D _rigidbody2D;

        private Vector2 _viewDirection;
        private float _velocity; // current horizontal velocity for platformer movement

        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _deceleration = 8f;
        [SerializeField] private float _velocityPower = 0.9f;

        // Input-driven movement state
        private float _inputX; // last horizontal input set by Move/Stop (not per-frame)
        private float _moveSpeed; // last requested move speed

        // Sliding tuning
        [SerializeField] private float _stopThreshold = 0.02f; // snap-to-stop threshold for velocity

        // Jump settings
        [SerializeField] private int _maxJumpCount = 1; // 최대 점프 횟수 (기본값 1 = 단일 점프)
        private int _currentJumpCount = 0; // 현재 사용한 점프 횟수

        // Knockback settings
        [SerializeField] private float _knockbackUpwardForce = 3f; // 넉백 시 위쪽 힘
        private bool _isKnockedBack = false; // 넉백 상태
        private float _knockbackDuration = 0.3f; // 넉백 지속 시간
        private float _knockbackTimer = 0f; // 넉백 타이머

        private void Awake()
        {
            _moveTargetTransform = this.transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        private void Tick(float dt)
        {
            // 넉백 타이머 처리
            if (_isKnockedBack)
            {
                _knockbackTimer -= dt;
                if (_knockbackTimer <= 0f)
                {
                    _isKnockedBack = false;
                    _inputX = 0f; // 넉백 종료 시 입력 초기화
                }
                return; // 넉백 중에는 일반 이동 처리 안 함
            }

            // 기존 이동 로직
            bool hasInput = Mathf.Abs(_inputX) > 0.0001f;
            float targetSpeed = _inputX * Mathf.Max(0f, _moveSpeed);
            float accelRate = hasInput ? _acceleration : _deceleration;
            float speedDiff = targetSpeed - _velocity;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, _velocityPower) * Mathf.Sign(speedDiff);

            _velocity += movement * dt;

            if (!hasInput && Mathf.Abs(_velocity) <= _stopThreshold)
            {
                _velocity = 0f;
            }

            var pos = _moveTargetTransform.position;
            pos.x += _velocity * dt;
            _moveTargetTransform.position = pos;
        }

        /// <summary>
        /// ViewDirection 방향으로 포물선 운동 점프
        /// </summary>
        /// <param name="force">점프 힘</param>
        public virtual void Jump(float force)
        {
            // 점프 횟수 체크
            if (_currentJumpCount >= _maxJumpCount)
            {
                return; // 최대 점프 횟수 초과
            }

            JumpWithPhysics(force);
            _currentJumpCount++;
        }

        /// <summary>
        /// Rigidbody2D를 사용한 물리 기반 점프
        /// </summary>
        private void JumpWithPhysics(float force)
        {
            // ViewDirection의 x 방향(좌우)을 사용하고, y는 점프를 위해 위쪽으로 고정
            // ViewDirection.x: -1(왼쪽) 또는 1(오른쪽)
            float horizontalDirection = _viewDirection.x;
        
            // 점프 방향: 수평 방향 + 수직 방향 조합
            // 예: 45도 각도로 점프하려면 (horizontalDirection, 1)을 normalized
            Vector2 jumpDirection = new Vector2(horizontalDirection, 1f).normalized;

            // 수평/수직 비율 조정
            Vector2 jumpForce = jumpDirection * force;

            // AddForce 대신 velocity 직접 설정 (더 즉각적인 반응)
            _rigidbody2D.linearVelocity = jumpForce;
        }

        public virtual void Move(Vector2 direction, float speed)
        {
            // 넉백 중에는 이동 불가
            if (_isKnockedBack)
                return;

            // Update view direction (keep last non-zero)
            if (direction.sqrMagnitude > 0.0001f)
            {
                _viewDirection = direction.normalized;
            }

            // Cache input and speed; actual movement happens per-frame in Tick()
            _inputX = Mathf.Clamp(direction.x, -1f, 1f);
            _moveSpeed = speed;
        }

        public virtual void Stop()
        {
            _inputX = 0f;
        }

        public virtual void Turn(Vector2 dir)
        {
            if (_viewDirection != dir.normalized)
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
        }

        /// <summary>
        /// 넉백 효과 적용 - ViewDirection 반대 방향으로 밀려남
        /// </summary>
        /// <param name="force">넉백 힘 (수평 방향)</param>
        public virtual void Knockback(float force)
        {
            if (_rigidbody2D == null)
                return;

            // 현재 바라보는 방향의 반대로 넉백
            // ViewDirection.x: 1(오른쪽) → 넉백은 왼쪽(-1)
            // ViewDirection.x: -1(왼쪽) → 넉백은 오른쪽(1)
            float knockbackDir = -Mathf.Sign(_viewDirection.x);
   
            // ViewDirection이 0이면 기본적으로 왼쪽으로 넉백
            if (Mathf.Abs(_viewDirection.x) < 0.01f)
            {
                knockbackDir = -1f;
            }

            // 넉백 벡터: 수평(반대 방향) + 수직(위쪽)
            Vector2 knockbackForce = new Vector2(
                knockbackDir * force,
                _knockbackUpwardForce
            );

            // Rigidbody2D에 즉시 적용
            _rigidbody2D.linearVelocity = knockbackForce;

            // 넉백 상태 활성화
            _isKnockedBack = true;
            _knockbackTimer = _knockbackDuration;

            // 일반 이동 입력 차단
            Stop();
        }

        /// <summary>
        /// 점프 중단 (긴급 착지)
        /// </summary>
        public void CancelJump()
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, 0);
 }

        /// <summary>
        /// 점프 횟수 초기화 (바닥 착지 시 호출)
        /// </summary>
        public void ResetJumpCount()
        {
 _currentJumpCount = 0;
        }

        /// <summary>
        /// 현재 점프 가능 여부 확인
        /// </summary>
     public bool CanJump()
        {
        return _currentJumpCount < _maxJumpCount;
        }

   /// <summary>
    /// 최대 점프 횟수 설정
   /// </summary>
        public void SetMaxJumpCount(int count)
      {
      _maxJumpCount = Mathf.Max(1, count);
        }

        /// <summary>
        /// 현재 남은 점프 횟수 반환
        /// </summary>
 public int GetRemainingJumpCount()
     {
    return _maxJumpCount - _currentJumpCount;
  }

        /// <summary>
        /// 넉백 상태 확인
     /// </summary>
        public bool IsKnockedBack()
        {
            return _isKnockedBack;
        }

        /// <summary>
  /// 넉백 상태 강제 해제
        /// </summary>
      public void CancelKnockback()
        {
  _isKnockedBack = false;
       _knockbackTimer = 0f;
        }
    }
}
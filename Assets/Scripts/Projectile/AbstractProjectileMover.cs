using UnityEngine;

namespace BS.GameObjects
{
    public abstract class AbstractProjectileMover : MonoBehaviour, IMover
    {
        public Vector2 ViewDirection => _viewDirection;
        public float Velocity => _velocity;

        private Transform _projectileTransform;
        private Rigidbody2D _rigidbody2D;

        private Vector2 _viewDirection;
        private float _velocity; // current speed magnitude

        [SerializeField] private float _gravityScale = 1f; // 중력 스케일
        [SerializeField] private bool _useGravity = true; // 중력 사용 여부
        [SerializeField] private bool _rotateWithVelocity = true; // 속도에 따라 회전 여부
        [SerializeField] private float _rotationOffset = 0f; // 회전 오프셋 (각도)

        private bool _isMoving = false;
        private Vector2 _initialVelocity;

        protected virtual void Awake()
        {
            _projectileTransform = this.transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();

            if (_rigidbody2D == null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            }

            // Rigidbody2D 초기 설정
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.gravityScale = _useGravity ? _gravityScale : 0f;
            _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            if (_isMoving)
            {
                UpdateVelocityAndRotation();
            }
        }

        /// <summary>
        /// 속도와 회전 업데이트
        /// </summary>
        protected virtual void UpdateVelocityAndRotation()
        {
            if (_rigidbody2D == null)
                return;

            // 현재 속도 벡터 가져오기
            Vector2 currentVelocity = _rigidbody2D.linearVelocity;

            // 속도 크기 계산
            _velocity = currentVelocity.magnitude;

            // ViewDirection 업데이트 (정규화된 속도 방향)
            if (_velocity > 0.01f)
            {
                _viewDirection = currentVelocity.normalized;

                // 속도에 따라 회전
                if (_rotateWithVelocity)
                {
                    RotateToVelocity(currentVelocity);
                }
            }
        }

        /// <summary>
        /// 속도 방향으로 투사체 회전
        /// </summary>
        protected virtual void RotateToVelocity(Vector2 velocity)
        {
            if (velocity.sqrMagnitude < 0.01f)
                return;

            // 속도 벡터를 각도로 변환 (degree)
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // 회전 오프셋 적용
            angle += _rotationOffset;

            // 투사체 회전 적용
            _projectileTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        /// <summary>
        /// 투사체 발사 (방향과 속도로)
        /// </summary>
        public virtual void Move(Vector2 direction, float speed)
        {
            if (_rigidbody2D == null)
                return;

            // 방향 정규화
            if (direction.sqrMagnitude > 0.0001f)
            {
                _viewDirection = direction.normalized;
            }

            // 초기 속도 설정
            _initialVelocity = _viewDirection * speed;
            _velocity = speed;

            // Rigidbody2D에 속도 적용
            _rigidbody2D.linearVelocity = _initialVelocity;

            _isMoving = true;

            // 초기 회전 설정
            if (_rotateWithVelocity)
            {
                RotateToVelocity(_initialVelocity);
            }
        }

        /// <summary>
        /// 투사체 정지
        /// </summary>
        public virtual void Stop()
        {
            if (_rigidbody2D != null)
            {
                _rigidbody2D.linearVelocity = Vector2.zero;
                _rigidbody2D.angularVelocity = 0f;
            }

            _velocity = 0f;
            _isMoving = false;
        }

        /// <summary>
        /// 각도로 발사 (도 단위)
        /// </summary>
        public virtual void LaunchAtAngle(float angleDegrees, float speed)
        {
            float angleRadians = angleDegrees * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
            Move(direction, speed);
        }

        /// <summary>
        /// 타겟 위치를 향해 발사
        /// </summary>
        public virtual void LaunchTowards(Vector2 targetPosition, float speed)
        {
            Vector2 direction = (targetPosition - (Vector2)_projectileTransform.position).normalized;
            Move(direction, speed);
        }

        /// <summary>
        /// 포물선 발사 (특정 타겟에 도달하도록)
        /// </summary>
        public virtual void LaunchParabolic(Vector2 targetPosition, float angle = 45f)
        {
            Vector2 startPos = _projectileTransform.position;
            Vector2 displacement = targetPosition - startPos;

            float angleRad = angle * Mathf.Deg2Rad;
            float gravity = Mathf.Abs(Physics2D.gravity.y) * _gravityScale;

            if (gravity <= 0f)
            {
                Debug.LogWarning("Gravity is zero, cannot calculate parabolic trajectory");
                return;
            }

            // 포물선 발사 속도 계산
            float speed = Mathf.Sqrt(
                (displacement.x * displacement.x * gravity) /
                (2 * Mathf.Cos(angleRad) * Mathf.Cos(angleRad) *
                (displacement.x * Mathf.Tan(angleRad) - displacement.y))
            );

            if (float.IsNaN(speed) || float.IsInfinity(speed))
            {
                // 계산 실패 시 직선 발사
                LaunchTowards(targetPosition, 10f);
                return;
            }

            // 발사 방향 계산
            float dirX = Mathf.Sign(displacement.x);
            Vector2 direction = new Vector2(
                dirX * Mathf.Cos(angleRad),
                Mathf.Sin(angleRad)
            ).normalized;

            Move(direction, speed);
        }

        /// <summary>
        /// 중력 스케일 설정
        /// </summary>
        public void SetGravityScale(float scale)
        {
            _gravityScale = scale;
            if (_rigidbody2D != null && _useGravity)
            {
                _rigidbody2D.gravityScale = _gravityScale;
            }
        }

        /// <summary>
        /// 중력 사용 여부 설정
        /// </summary>
        public void SetUseGravity(bool useGravity)
        {
            _useGravity = useGravity;
            if (_rigidbody2D != null)
            {
                _rigidbody2D.gravityScale = _useGravity ? _gravityScale : 0f;
            }
        }

        /// <summary>
        /// 속도 기반 회전 여부 설정
        /// </summary>
        public void SetRotateWithVelocity(bool rotate)
        {
            _rotateWithVelocity = rotate;
        }

        /// <summary>
        /// 회전 오프셋 설정 (도 단위)
        /// </summary>
        public void SetRotationOffset(float offsetDegrees)
        {
            _rotationOffset = offsetDegrees;
        }

        /// <summary>
        /// 현재 이동 중인지 확인
        /// </summary>
        public bool IsMoving()
        {
            return _isMoving && _velocity > 0.01f;
        }

        /// <summary>
        /// 현재 속도 벡터 반환
        /// </summary>
        public Vector2 GetVelocityVector()
        {
            return _rigidbody2D != null ? _rigidbody2D.linearVelocity : Vector2.zero;
        }

        /// <summary>
        /// 속도에 힘 추가 (충격 등)
        /// </summary>
        public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
        {
            if (_rigidbody2D != null)
            {
                _rigidbody2D.AddForce(force, mode);
            }
        }
    }
}
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BS.GameObjects;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IdleMove", story: "Idle Move", category: "Action", id: "1a5a1aab0fa89b63a148fb3cbe4fbd26")]
public partial class IdleMoveAction : Action
{
    [SerializeReference]
    public BlackboardVariable<float> _idleMoveSpeed;

    [SerializeReference]
    public BlackboardVariable<float> _directionChangeTime;

    private AbstractEnermy _currentEnermy;
    private BehaviorGraphAgent _agent;
    private float _currentDirectionTime;
    private int _moveDirection = 1; // 1: 오른쪽, -1: 왼쪽

    protected override Status OnStart()
    {
        _agent = GameObject.GetComponent<BehaviorGraphAgent>();
        _currentEnermy = GameObject.GetComponent<AbstractEnermy>();

        // 초기 방향을 랜덤하게 설정
        _moveDirection = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        _currentDirectionTime = 0f;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_currentEnermy == null)
        {
            return Status.Failure;
        }

        // 방향 전환 시간 체크
        _currentDirectionTime += Time.deltaTime;

        if (_directionChangeTime != null && _currentDirectionTime >= _directionChangeTime.Value)
        {
            // 방향 전환
            _moveDirection *= -1;
            _currentDirectionTime = 0f;
        }

        // 좌우 이동
        Vector2 direction = new Vector2(_moveDirection, 0f);

        // 속도 설정
        float speed = _idleMoveSpeed != null ? _idleMoveSpeed.Value : _currentEnermy.CurrentSpeed;

        // 이동 실행
        _currentEnermy.Move(direction);

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (_currentEnermy != null)
        {
            _currentEnermy.Stop();
        }
    }
}


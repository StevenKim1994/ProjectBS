using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BS.GameObjects;

/// <summary>
/// 아이들 상태에서 좌우로 이동하는 단순 순찰 액션
/// 타겟 탐색이나 추적 기능은 별도의 노드(FindPlayerCharacterActionNode, ChasePlayerCharacterActionNode)에서 처리
/// </summary>
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IdleMove", story: "Idle Move", category: "AI/Action", id: "1a5a1aab0fa89b63a148fb3cbe4fbd26")]
public partial class IdleMoveAction : Action
{
    /// <summary>
    /// 아이들 상태에서의 이동 속도
    /// </summary>
    [SerializeReference]
    public BlackboardVariable<float> _idleMoveSpeed;

    /// <summary>
    /// 좌우 방향을 전환할 시간 간격 (초)
    /// </summary>
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

        if (_currentEnermy == null)
        {
            Debug.LogError($"[IdleMoveAction] AbstractEnermy component not found on {GameObject.name}");
            return Status.Failure;
        }

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

        // 속도 설정 (BlackboardVariable이 설정되어 있으면 사용, 아니면 캐릭터의 기본 속도 사용)
        float speed = _idleMoveSpeed != null && _idleMoveSpeed.Value > 0
            ? _idleMoveSpeed.Value
            : _currentEnermy.CurrentSpeed;

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


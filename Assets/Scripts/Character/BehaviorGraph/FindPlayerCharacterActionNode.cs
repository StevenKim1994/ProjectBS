using UnityEngine;
using Unity.Behavior;
using BS.Common;
using BS.GameObjects;

[NodeDescription("Find Player Character",
    story: "Find Player Character",
    category: "AI/Action", id: "find_player_character")]
public class FindPlayerCharacterActionNode : Action
{
    public BlackboardVariable<float> _searchRadius;
    public BlackboardVariable<Transform> _target;
    public BlackboardVariable<float> _patrolSpeed;
    public BlackboardVariable<float> _directionChangeTime;

    private float _currentDirectionTime;
    private int _moveDirection = 1; // 1: 오른쪽, -1: 왼쪽
    private AbstractEnermy _currentEnermy;
    private BehaviorGraphAgent _agent;

    protected override Status OnStart()
    {
        _agent = GameObject.GetComponent<BehaviorGraphAgent>();
        _currentEnermy = GameObject.GetComponent<AbstractEnermy>();
        _agent.SetVariableValue<AbstractEnermy>("Current Enermy", _currentEnermy);
        _agent.SetVariableValue<float>("Patrol Speed", _currentEnermy.Ability.MoveSpeed);

        _currentDirectionTime = 0f;
        _moveDirection = 1;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // 매 프레임마다 플레이어 찾기
        Transform playerTransform = FindPlayerInRadius();

        if (playerTransform != null)
        {
            _target.Value = playerTransform;
            _agent.SetVariableValue<Transform>("Target", playerTransform);

            return Status.Success;
        }

        // 플레이어를 찾지 못했으면 좌우로 이동
        PatrolLeftRight();

        // 플레이어를 찾지 못했으면 계속 탐색
        return Status.Running;
    }

    protected override void OnEnd()
    {
        _currentEnermy.Stop();
    }

    /// <summary>
    /// 좌우로 이동하는 순찰 로직 
    /// </summary>
    private void PatrolLeftRight()
    {
        // 방향 전환 시간 체크
        _currentDirectionTime += Time.deltaTime;
        if (_currentDirectionTime >= _directionChangeTime.Value)
        {
            _moveDirection *= -1; // 방향 전환
            _currentDirectionTime = 0f;
        }

        Vector2 direction = new Vector2(_moveDirection, 0f);

        _currentEnermy.Move(direction);
    }

    /// <summary>
    /// 탐색 반경 내에서 Player 태그를 가진 GameObject를 찾습니다
    /// </summary>
    private Transform FindPlayerInRadius()
    {
        // Agent의 현재 위치
        Vector3 agentPosition = _currentEnermy.transform.position;

        // Player 태그를 가진 모든 GameObject 찾기
        GameObject[] players = GameObject.FindGameObjectsWithTag(Constrants.TAG_PLAYER);

        foreach (GameObject player in players)
        {
            // Agent와 플레이어 간의 거리 계산
            float distance = Vector3.Distance(agentPosition, player.transform.position);

            // 탐색 반경 내에 있는지 확인
            if (distance <= _searchRadius.Value)
            {
                return player.transform;
            }
        }

        return null;
    }
}

using UnityEngine;
using Unity.Behavior;
using BS.Common;

[NodeDescription("Find Player Character",
    story: "Find Player Character",
    category: "AI/Action")]
public class FindPlayerCharacterActionNode : Action
{
    [SerializeField, Tooltip("탐색 반경")]
    public BlackboardVariable<float> _searchRadius;

    [SerializeField, Tooltip("찾은 Player Tag GameObject를 저장할 Blackboard 변수")]
    public BlackboardVariable<Transform> _target;

    protected override Status OnStart()
    {
        // 시작 시 초기화 
        if (_target != null)
        {
            _target.Value = null;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // 매 프레임마다 플레이어 찾기
        Transform playerTransform = FindPlayerInRadius();

        if (playerTransform != null)
        {
            // 플레이어를 찾았으면 Blackboard 변수에 저장
            if (_target != null)
            {
                _target.Value = playerTransform;
                if(Parent.GameObject.TryGetComponent<BehaviorGraphAgent>(out var agent))
                {
                    agent.SetVariableValue<Transform>("Target", _target);
                }
            }

            return Status.Success;
        }

            // 플레이어를 찾지 못했으면 계속 탐색
        return Status.Running;
    }

    protected override void OnEnd()
    {
        // 종료 시 정리 작업 (필요한 경우)
    }

    /// <summary>
    /// 탐색 반경 내에서 Player 태그를 가진 GameObject를 찾습니다
    /// </summary>
    private Transform FindPlayerInRadius()
    {
        // Agent의 현재 위치
        Vector3 agentPosition = GameObject.transform.position;

        // Player 태그를 가진 모든 GameObject 찾기
        GameObject[] players = GameObject.FindGameObjectsWithTag(Constrants.TAG_PLAYER);

        foreach (GameObject player in players)
        {
            // Agent와 플레이어 간의 거리 계산
            float distance = Vector3.Distance(agentPosition, player.transform.position);

            // 탐색 반경 내에 있는지 확인
            if (distance <= _searchRadius)
            {
                return player.transform;
            }
        }

        return null;
    }

    /// <summary>
    /// 에디터에서 탐색 범위를 시각화 (Gizmos)
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (GameObject != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(GameObject.transform.position, _searchRadius);
        }
    }
}

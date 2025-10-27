using UnityEngine;
using Unity.Behavior;
using BS.Common;
using Unity.VisualScripting;
using BS.GameObjects;

[NodeDescription("Chase Player Character",
 story: "Chase Player Character",
 category: "AI/Action", id: "chase_player_character")]
public class ChasePlayerCharacterActionNode : Action
{
    [SerializeField, Tooltip("추적 속도")]
    private BlackboardVariable<float> _chaseSpeed; // DESC :: 외부에서 Ability의 능력치로 적용

    [SerializeField, Tooltip("추적중지 거리(공격 사정거리 범위 안에 들어왔는지 여부")]
    private BlackboardVariable<float> _stopChaseDistance;

    [SerializeField, Tooltip("플레이어 캐릭터")]
    private BlackboardVariable<Transform> _target;

    protected override Status OnStart()
    {
        if (Parent.GameObject.TryGetComponent<AbstractEnermy>(out var enermy))
        {
            _chaseSpeed.Value = enermy.Ability.MoveSpeed;
            _stopChaseDistance.Value = enermy.Ability.AttackRange;
            if(enermy.BehaviorGraphAgent.GetVariable<Transform>("Target", out var target))
            {
                _target = target;
            }
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // 매 프레임마다 플레이어 추적 로직 구현
        // 예: 플레이어 위치로 이동, 애니메이션 재생 등
        // 추적이 성공적으로 이루어졌다면 Status.Success 반환
        // 그렇지 않으면 계속 추적 중이므로 Status.Running 반환
        if (_target == null || _target.Value == null)
        {
            return Status.Running;
        }

        var enemy = GameObject.TryGetComponent<AbstractEnermy>(out var e) ? e : null;
        if (enemy == null)
        {
            return Status.Failure;
        }

        Vector3 agentPos = GameObject.transform.position;
        Vector3 targetPos = _target.Value.position;

        //2D 기준: 수평/수직 거리 계산
        float distance = Vector2.Distance(new Vector2(agentPos.x, agentPos.y), new Vector2(targetPos.x, targetPos.y));
        float stopDist = _stopChaseDistance != null ? _stopChaseDistance.Value : 0f;

        // 사정거리 이내면 정지 및 성공 반환
        if (distance <= stopDist)
        {
            enemy.Mover.Stop();
            return Status.Success;
        }

        // 타겟을 향해 이동 (플랫폼 기준 X축 위주)
        float speed = (_chaseSpeed != null && _chaseSpeed.Value > 0f) ? _chaseSpeed.Value : enemy.Ability.MoveSpeed;
        float dirX = Mathf.Sign(targetPos.x - agentPos.x);
        Vector2 dir = new Vector2(dirX, 0f);
        enemy.Mover.Move(dir, speed);

        return Status.Running;
    }

    protected override void OnEnd()
    {
        // 종료 시 정리 작업 (필요한 경우)
        if (GameObject != null && GameObject.TryGetComponent<AbstractEnermy>(out var enemy) && enemy.Mover != null)
        {
            enemy.Mover.Stop();
        }
        if (_target != null)
        {
            _target.Value = null;
        }

    }
}

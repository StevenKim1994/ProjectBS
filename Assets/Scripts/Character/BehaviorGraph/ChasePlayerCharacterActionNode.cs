using UnityEngine;
using Unity.Behavior;
using BS.Common;
using BS.GameObjects;

[NodeDescription("Chase Player Character",
 story: "Chase Player Character",
 category: "AI/Action", id: "chase_player_character")]
public class ChasePlayerCharacterActionNode : Action
{
    public BlackboardVariable<float> _stopChaseDistance;
    public BlackboardVariable<Transform> _target;
    public BlackboardVariable<AbstractEnermy> _currentEnermy;
    private BehaviorGraphAgent _agent;

    protected override Status OnStart()
    {
        _agent = GameObject.GetComponent<BehaviorGraphAgent>();

        if (_agent.GetVariable<AbstractEnermy>("Current Enermy", out var enermy))
        {
            _currentEnermy.Value = enermy;
        }
        else
        {
            _currentEnermy.Value = GameObject.GetComponent<AbstractEnermy>();
        }

        if (_agent.GetVariable<Transform>("Target", out var target))
        {
            _target.Value = target;
        }

        if(_agent.GetVariable<float>("Stop Chase Distance", out var stopChaseDistance))
        {
            _stopChaseDistance.Value = stopChaseDistance;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // 매 프레임마다 플레이어 추적 로직 구현
        // 예: 플레이어 위치로 이동, 애니메이션 재생 등
        // 추적이 성공적으로 이루어졌다면 Status.Success 반환
        // 그렇지 않으면 계속 추적 중이므로 Status.Running 반환
        if (_target.Value == null)
        {
            return Status.Running;
        }

        Vector3 agentPos = GameObject.transform.position;
        Vector3 targetPos = _target.Value.position;

        //2D 기준: 수평/수직 거리 계산
        float distance = Vector2.Distance(new Vector2(agentPos.x, agentPos.y), new Vector2(targetPos.x, targetPos.y));
        float stopDist = _stopChaseDistance.Value; 

        // 사정거리 이내면 정지 및 성공 반환
        if (distance <= stopDist)
        {
            _currentEnermy.Value.Stop();
            return Status.Success;
        }

        // 타겟을 향해 이동 (플랫폼 기준 X축 위주)
        float dirX = Mathf.Sign(targetPos.x - agentPos.x);
        Vector2 dir = new Vector2(dirX, 0f);
        _currentEnermy.Value.Move(dir);

        return Status.Running;
    }

    protected override void OnEnd()
    {

    }
}

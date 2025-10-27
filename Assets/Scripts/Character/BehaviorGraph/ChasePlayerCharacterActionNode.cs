using UnityEngine;
using Unity.Behavior;

[NodeDescription("Chase Player Character",
    story: "Chase Player Character",
    category: "AI/Action")]
public class ChasePlayerCharacterActionNode : Action
{
    public BlackboardVariable<float> _chaseSpeed; // DESC :: 외부에서 Ability의 능력치로 적용

    protected override Status OnStart()
    {
        // 시작 시 초기화 작업 (필요한 경우)
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // 매 프레임마다 플레이어 추적 로직 구현
        // 예: 플레이어 위치로 이동, 애니메이션 재생 등
        // 추적이 성공적으로 이루어졌다면 Status.Success 반환
        // 그렇지 않으면 계속 추적 중이므로 Status.Running 반환
        return Status.Running;
    }

    protected override void OnEnd()
    {
        // 종료 시 정리 작업 (필요한 경우)
    }
}

using UnityEngine;
using Unity.Behavior;

[NodeDescription("Attack Count Condition",
story: "공격 횟수를 비교합니다.",
category: "AI/Decision",
id: "attack_count_condition"
    )]
public class AttackCountConditionNode : Condition
{
    [SerializeField, Tooltip("공격 횟수")]
    private BlackboardVariable<int> _attackCount;

    [SerializeField, Tooltip("현재 공격횟수")]
    private BlackboardVariable<int> _currentAttackCount;

    public override void OnStart()
    {
        base.OnStart();
    }

    public override bool IsTrue()
    {
        return _currentAttackCount > _attackCount;
    }
}

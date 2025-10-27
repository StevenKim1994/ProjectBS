using UnityEngine;
using Unity.Behavior;

[NodeDescription("Attack Count Condition",
story: "공격 횟수를 비교합니다.",
category: "AI/Decision")]
public class AttackCountConditionNode : Condition
{
    public BlackboardVariable<int> _attackCount;

    public override bool IsTrue()
    {
        return _attackCount > 0;
    }
}

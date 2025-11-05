using UnityEngine;
using UnityEngine.Animations;
using BS.GameObjects;
using BS.Common;

namespace BS.Animations
{
    public class AttackAnimBehaviorState : StateMachineBehaviour
    {
        [SerializeField] private float _damageColiderActiveStartTime = 0.2f;
        private AbstractCharacter _cachedCharacter;

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            base.OnStateMachineExit(animator, stateMachinePathHash);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_cachedCharacter == null)
            {
                _cachedCharacter = animator.GetComponentInParent<AbstractCharacter>();
            }

            // 매 진입 시 전진/공격 콜라이더 세팅 수행
            if (_cachedCharacter is NightCharacter nightCharacter)
            {
                nightCharacter.ForwardAttackMovement();
            }

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_cachedCharacter != null)
            {
                if (stateInfo.normalizedTime >= _damageColiderActiveStartTime)
                {
                    _cachedCharacter.SetAttackDamageColliderActive(true);
                }
                else
                {
                    _cachedCharacter.SetAttackDamageColliderActive(false);
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            if (_cachedCharacter != null)
            {
                _cachedCharacter.SetAttackDamageColliderActive(false);
            }
        }
    }
}
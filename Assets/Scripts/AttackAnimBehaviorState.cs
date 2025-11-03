using UnityEngine;
using UnityEngine.Animations;
using BS.GameObjects;
using BS.Common;

namespace BS.Animations
{
    public class AttackAnimBehaviorState : StateMachineBehaviour
    {
        [SerializeField] private float _damageColiderActiveStartTime = 0.2f;
        [SerializeField] private Vector2 _damageColiderSize = new Vector2(1f, 1f);
        private AbstractCharacter _cachedCharacter;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_cachedCharacter == null)
            {
                _cachedCharacter = animator.GetComponentInParent<AbstractCharacter>();

                if (_cachedCharacter is NightCharacter nightCharacter)
                {
                    nightCharacter.ForwardAttackMovement();
                }
            }

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_cachedCharacter != null)
            {
                if(stateInfo.normalizedTime >= _damageColiderActiveStartTime)
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
                _cachedCharacter.ResetAttackCombo();
            }
        }
    }
}
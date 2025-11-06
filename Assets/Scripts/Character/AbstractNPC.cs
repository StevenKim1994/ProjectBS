using UnityEngine;
using BS.Common;
using BS.System;
namespace BS.GameObjects
{
    public class AbstractNPC : AbstractCharacter, IInteractable
    {
        [SerializeField]
        protected SpriteRenderer _notiArrowRender;
        public SpriteRenderer NotiArrowRender => _notiArrowRender;

        protected bool _isTalking = false;
        public bool IsTalking => _isTalking;

        protected bool _isInPlayerRange = false;
        public bool IsInPlayerRange => _isInPlayerRange;

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Constrants.TAG_PLAYER))
            {
                if (collision.transform.TryGetComponent<NightCharacter>(out NightCharacter player))
                {
                    _isInPlayerRange = true;
                    PlayerSystem.Instance.SetInteractObject(this);
                    Debug.Log("Player is in NPC range.");
                }
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.CompareTag(Constrants.TAG_PLAYER))
            {
                if (collision.transform.TryGetComponent<NightCharacter>(out NightCharacter player))
                {
                    Debug.Log("Player is still in NPC range.");
                }
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Constrants.TAG_PLAYER))
            {
                if (collision.transform.TryGetComponent<NightCharacter>(out NightCharacter player))
                {
                    _isInPlayerRange = false;
                    PlayerSystem.Instance.SetInteractObject(null);
                    Debug.Log("Player has left NPC range.");
                }
            }
        }

        public virtual void Interact()
        {

        }
    }
}
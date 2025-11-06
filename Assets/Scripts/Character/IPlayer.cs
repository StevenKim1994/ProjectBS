
using BS.GameObjects;
using UnityEngine;

public interface IPlayer 
{
    void GainRewardableObject(AbstractRewardableObject abstractRewardableObject);
    public AbstractCharacter GetCharacterType();

    public virtual void Throwing()
    {

    }

    public virtual void Interact()
    {

    }

    public virtual void Attack()
    {

    }

    public virtual void Defense()
    {

    }

    public virtual void Jump()
    {

    }

    public virtual void Move(Vector2 dir)
    {

    }

    public virtual void Turn(Vector2 dir)
    {

    }
}

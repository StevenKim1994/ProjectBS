
using BS.GameObjects;

public interface IPlayer 
{
    void GainRewardableObject(AbstractRewardableObject abstractRewardableObject);
    public AbstractCharacter GetCharacterType();
}

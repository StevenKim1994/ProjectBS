using UnityEngine;

namespace BS.GameObjects
{
    public interface IRewardableObject 
    {
        bool IsRewarded { get; }

        void Release();
    }
}
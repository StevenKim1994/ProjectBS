using UnityEngine;
using UnityEngine.U2D;
using BS.Common;

namespace BS.GameObjects
{
    public interface IPropObject 
    {
        void TakeDamage(float amount);
        void DestroyProp();
        void HitAnim();
    }
}
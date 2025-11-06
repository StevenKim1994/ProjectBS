using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BS.System
{
    public interface ISystem
    {

        /// <summary>
        /// Load 메서드에서는 다른 시스템을 참조하지 말것!
        /// </summary>
        public virtual void Load()
        {

        }

        public virtual void Unload()
        {

        }

        protected virtual void Initialize()
        {

        }

        protected virtual void Release()
        {

        }

    }
}
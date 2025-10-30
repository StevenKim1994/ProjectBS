using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BS.System
{
    public interface ISystem
    {

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
using UnityEngine;
using UnityEngine.Pool;
using BS.Common;
using BS.GameObjects;
using System;

namespace BS.System
{ 
    public class EnermySystem : ISystem
    {
        private static EnermySystem _instance;

        public static EnermySystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<EnermySystem>();
                }
                return _instance;
            }
        }

        public void Load()
        {

        }

        public void Unload()
        {

        }
    }
}
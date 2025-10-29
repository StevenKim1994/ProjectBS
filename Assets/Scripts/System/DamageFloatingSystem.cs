using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;
using BS.GameObjects;
using BS.Common;

namespace BS.System
{
    public class DamageFloatingSystem : ISystem
    {
        private static DamageFloatingSystem _instance;
        
        public static DamageFloatingSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<DamageFloatingSystem>();
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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BS.GameObjects;

namespace BS.System
{
    public class ProjectileSystem : ISystem
    {
        private static ProjectileSystem _instance;
        public static ProjectileSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<ProjectileSystem>();
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

        public void SpawnProjectile<T>( AbstractCharacter owner, Transform targetObject, Quaternion rotation)
        {

        }
    }
}
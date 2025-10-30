using System;
using System.Collections.Generic;
using UnityEngine;
using BS.System;
using BS.Common;

namespace BS.GameObjects
{
    public class SystemGameObject : MonoBehaviour
    {
        private static SystemGameObject _instance;

        public static SystemGameObject Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindFirstObjectByType<SystemGameObject>();
                }
                return _instance;
            }
        }

        [SerializeField]
        private AbstractInGameScene _currentGameScene;

        public AbstractInGameScene CurrentGameScene => _currentGameScene;

        private Dictionary<Type, ISystem> _systems = new Dictionary<Type, ISystem>();
        public T GetSystem<T>() where T : ISystem
        {
            Type type = typeof(T);
            if (_systems.ContainsKey(type))
            {
                return (T)_systems[type];
            }
            throw new Exception($"System of type {type} not found.");
        }

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            if (_currentGameScene == null) 
            {
                _currentGameScene = GameObject.FindFirstObjectByType<AbstractInGameScene>();
            }

            _systems.Add(typeof(ResourceSystem), new ResourceSystem());
            _systems.Add(typeof(DataSystem), new DataSystem());
            _systems.Add(typeof(ScreenSystem), new ScreenSystem());
            _systems.Add(typeof(InputControlSystem), new InputControlSystem());
            _systems.Add(typeof(DamageColiderSystem), new DamageColiderSystem());
            _systems.Add(typeof(UISystem), new UISystem());
            _systems.Add(typeof(EnermySystem), new EnermySystem());
            _systems.Add(typeof(DamageFloatingSystem), new DamageFloatingSystem());
            _systems.Add(typeof(RewardableObjectSystem), new RewardableObjectSystem());
            _systems.Add(typeof(ProjectileSystem), new ProjectileSystem());
            _systems.Add(typeof(TimeSystem), new TimeSystem());
        }

        public void LoadAllSystems()
        {
            foreach (var system in _systems.Values)
            {
                system.Load();
            }
        }

        public void UnloadAllSystems()
        {
            foreach (var system in _systems.Values)
            {
                system.Unload();
            }
        }

    }
}
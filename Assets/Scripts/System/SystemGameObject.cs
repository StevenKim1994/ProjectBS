using System;
using System.Collections.Generic;
using UnityEngine;
using BS.System;
using BS.Common;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;


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
    private PixelPerfectCamera _pixelPerfectCamera;

    public PixelPerfectCamera PixelPerfectCamera => _pixelPerfectCamera;

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

        if(_pixelPerfectCamera == null) // DESC :: PixelPerfectCamera가 할당되지 않았을 경우 자동 할당 만약, 특정 카메라를 얻고 싶다면 수동으로 할당
        {
            _pixelPerfectCamera = GameObject.FindFirstObjectByType<PixelPerfectCamera>();
        }

        _systems.Add(typeof(ResourceSystem), new ResourceSystem());
        _systems.Add(typeof(ScreenSystem), new ScreenSystem());
        _systems.Add(typeof(InputControlSystem), new InputControlSystem());
        _systems.Add(typeof(DamageColiderSystem), new DamageColiderSystem());
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

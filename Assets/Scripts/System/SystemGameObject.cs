using System;
using System.Collections.Generic;
using UnityEngine;
using BS.System;
using BS.Common;

public class SystemGameObject : MonoBehaviour
{
    private static SystemGameObject _instance;
    public static SystemGameObject Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject systemGameObject = new GameObject(Constrants.STR_SYSTEMOBJECT);
                _instance = systemGameObject.AddComponent<SystemGameObject>();
            }
            return _instance;
        }
    }

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
    }

    private void Start()
    {
        _systems.Add(typeof(ScreenSystem), new ScreenSystem());
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

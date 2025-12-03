using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : IServiceLocator
{
    private static ServiceLocator _instance;
    public static ServiceLocator Instance => _instance ??= new ServiceLocator();

    private readonly Dictionary<Type, object> _services = new();

    public void RegisterService<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public T GetService<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        
        throw new Exception($"Service {typeof(T)} not registered");
    }
}
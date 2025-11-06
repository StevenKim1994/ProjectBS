using System;
using UnityEngine;
using BS.Common;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class UIViewAttribute : Attribute
{
    private string _addressablePath;
    public string AddressablePath
    {
        get => _addressablePath;
    }

    private bool _isStackable;
    public bool IsStackable
    {
        get => _isStackable;
    }

    public UIViewAttribute(string addressablePath, bool isStackable = true)
    {
        _addressablePath = addressablePath;
        _isStackable = isStackable;
    }
}

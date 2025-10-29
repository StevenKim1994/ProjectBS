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

    public UIViewAttribute(string addressablePath)
    {
        _addressablePath = addressablePath;
    }
}

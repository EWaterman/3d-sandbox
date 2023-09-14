using UnityEngine;

/// <summary>
/// A simple scriptable object wrapper on a type.
/// 
/// Most often used to store globally accessible fields/objects statically inside of a scriptable
/// object. We do this to avoid needing to ever access other scripts to retrieve their variables.
/// Anyone who needs to access a shared variable can simply reference the SO associated with it.
/// 
/// Code derived from: https://github.com/roboryantron/Unite2017
/// </summary>
public abstract class VariableSO<T> : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField, TextArea]
    string _devDescription;
#endif

    [field: SerializeField] public T Value { get; protected set; }

    public void SetValue(VariableSO<T> value)
    {
        Value = value.Value;
    }

    public void SetValue(T value)
    {
        Value = value;
    }
}
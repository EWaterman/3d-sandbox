using UnityEngine;

/**
 * Convenience base class for factories that are also scriptable objects, which,
 * for our purposes here, is all factories.
 */
public abstract class Factory<T> : ScriptableObject, IFactory<T>
{
    public abstract T Create();
}

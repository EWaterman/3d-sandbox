using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A generic scriptable object pool that can generate members of type T on-demand via a factory.
 */
public abstract class Pool<T> : ScriptableObject, IPool<T>
{
    protected readonly Stack<T> available = new Stack<T>();
    public abstract IFactory<T> factory { get; set; }
    protected bool hasBeenPrewarmed { get; set; }

    /**
     * Creates an instance of T using the factory.
     */
    protected virtual T Create()
    {
        return factory.Create();
    }

    /**
     * Pre-instantiate the pool with a number of T instances.
     * 
     * Note: This method can be called at any time, but only once for the lifetime of the pool.
     */
    public virtual void Prewarm(int num)
    {
        if (hasBeenPrewarmed)
        {
            Debug.LogWarning($"Pool {name} has already been prewarmed.");
            return;
        }
        for (int i = 0; i < num; i++)
        {
            available.Push(Create());
        }
        hasBeenPrewarmed = true;
    }

    /**
     * Pop an instance from the pool if one is available, otherwise create a new one.
     * Note that this means if we're not careful about putting instances back on the stack
     * we can end up infinitely creating new ones so remember to always call Return(T obj)
     */
    public virtual T Request()
    {
        return available.Count > 0 ? available.Pop() : Create();
    }

    /**
     * Batch fetch a number of instances from the pool.
     */
    public virtual IEnumerable<T> Request(int num = 1)
    {
        List<T> members = new List<T>(num);
        for (int i = 0; i < num; i++)
        {
            members.Add(Request());
        }
        return members;
    }

    /**
     * Return an instance to the pool.
     */
    public virtual void Return(T member)
    {
        available.Push(member);
    }

    /**
     * Return multiple instances to the pool.
     */
    public virtual void Return(IEnumerable<T> members)
    {
        foreach (T member in members)
        {
            Return(member);
        }
    }

    public virtual void OnDisable()
    {
        available.Clear();
        hasBeenPrewarmed = false;
    }
}

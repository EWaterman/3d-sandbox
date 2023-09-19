using UnityEngine;

/**
 * Abstract Pool for use when the type to pool is a Unity Component.
 * 
 * Assumes the pooled components will be wrapped in some parent component.
 * The components are tracked via their transforms because we know that will always exist so
 * long as the component is alive.
 */
public abstract class ComponentPool<T> : Pool<T> where T : Component
{
    Transform _parent;
    Transform _poolRoot;
    Transform PoolRoot
    {
        get
        {
            if (_poolRoot == null)
            {
                _poolRoot = new GameObject(name).transform;
                _poolRoot.SetParent(_parent);
            }
            return _poolRoot;
        }
    }

    /**
     * Sets the Transform to which this pool should become a child.
     * 
     * NOTE: Setting the parent to an object marked DontDestroyOnLoad will effectively make this pool DontDestroyOnLoad.
     *       This can only be circumvented by manually destroying the object or its parent or by setting the parent to an
     *       object not marked DontDestroyOnLoad.
     */
    public void SetParent(Transform t)
    {
        _parent = t;
        PoolRoot.SetParent(_parent);
    }

    public override T Request()
    {
        T member = base.Request();
        member.gameObject.SetActive(true);
        return member;
    }

    public override void Return(T member)
    {
        member.transform.SetParent(PoolRoot.transform);
        member.gameObject.SetActive(false);
        base.Return(member);
    }

    protected override T Create()
    {
        T newMember = base.Create();
        newMember.transform.SetParent(PoolRoot.transform);
        newMember.gameObject.SetActive(false);
        return newMember;
    }

    /**
     * Disables the pool. In the case where we're live disabling via the Inspector,
     * destroy the pool immediately to avoid any further actions taking place on the next update.
     */
    public override void OnDisable()
    {
        base.OnDisable();
        if (_poolRoot != null)
        {

#if UNITY_EDITOR
            DestroyImmediate(_poolRoot.gameObject);
#else
			Destroy(_poolRoot.gameObject);
#endif
        }
    }
}

/**
 * Represents the object pool design pattern, which is essentially a collection
 * of re-usable objects of type T. We should use pools in cases where creating instances
 * of an object is expensive, but we frequently need new, short lived, instances.
 * 
 * Some pools have a limited number of instances available, and if a request comes in while
 * the pool is empty, it will either error or wait until an instance becomes available. Other
 * pools allow themselves to grow in size dynamically.
 * 
 * More info: https://en.wikipedia.org/wiki/Object_pool_pattern
 */
public interface IPool<T>
{
    // Pre-instantiate a number of instances for the pool.
    void Prewarm(int num);

    // Fetch an instance of T from the pool.
    T Request();

    // Returns an instance of T back to the pool.
    void Return(T member);
}

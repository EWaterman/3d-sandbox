/**
 * Represents the factory design pattern.
 * 
 * Type param "T" is the type that the factory is creating.
 */
public interface IFactory<T>
{
    // Creates an instance of T
    T Create();
}

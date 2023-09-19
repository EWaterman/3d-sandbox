using UnityEngine;

/// <summary>
///  To be implemented by all objects that can be interacted with (by the player).
///  Unity hates interfaces so use this abstract class when we need to fetch components.
/// </summary>
public abstract class Interactable : MonoBehaviour, IInteractable
{
    /// <summary>
    /// What to do when the player enters within interaction range of the object.
    /// </summary>
    public virtual void OnEnterInteractRange()
    {
        // By default do nothing.
    }

    /// <summary>
    /// What to do when the player leaves the interaction range of the object.
    /// </summary>
    public virtual void OnExitInteractRange()
    {
        // By default do nothing.
    }

    /// <summary>
    /// The actual behaviour when the object is interacted with.
    /// </summary>
    public abstract void OnInteract();
}
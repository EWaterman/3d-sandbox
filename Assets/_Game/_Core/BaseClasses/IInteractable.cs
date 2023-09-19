/// <summary>
///  To be implemented by all objects that can be interacted with (by the player).
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// What to do when the player enters within interaction range of the object.
    /// </summary>
    public void OnEnterInteractRange();

    /// <summary>
    /// What to do when the player leaves the interaction range of the object.
    /// </summary>
    public void OnExitInteractRange();

    /// <summary>
    /// The actual behaviour when the object is interacted with.
    /// </summary>
    public void OnInteract();
}
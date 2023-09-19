using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Acts as a message broker between Event Triggers (publisher) and Event Listeners (subscriber)
/// so that they can communicate while still being decoupled from each other.
/// 
/// When an event is triggered, invokes all listeners that are subscribed to that event.
/// </summary>
[CreateAssetMenu(menuName = "Events/Empty Event Channel")]
public class EmptyEventChannel : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField, TextArea]
    string _devDescription;
#endif

    public UnityAction Listeners;

    /// <summary>
    ///  Notifies/invokes all listeners of the channel that an event has occurred.
    ///  If an event needs multiple params, wrap them in a context object.
    /// </summary>
    public void RaiseEvent()
    {
        Listeners?.Invoke();
    }
}
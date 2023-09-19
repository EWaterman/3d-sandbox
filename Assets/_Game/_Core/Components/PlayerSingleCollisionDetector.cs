using UnityEngine;

/// <summary>
/// Detects if the object has entered collision with the player and raises an event.
/// Immediately destroys self afterwards so that the event is only triggered once.
/// </summary>
public class PlayerSingleCollisionDetector : MonoBehaviour
{
    [Header("Broadcasting On")]
    [SerializeField, Tooltip("The channel to raise events to when the player enters our trigger zone.")]
    BoolEventChannel _triggerEnterChannel;

    void OnTriggerEnter(Collider other)
    {
        if (other.IsPlayer())
        {
            _triggerEnterChannel.RaiseEvent(true);
            Destroy(this);
        }
    }
}

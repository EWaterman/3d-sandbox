using UnityEngine;

/// <summary>
/// Detects if the object has collided with the player and raises an event.
/// Is a simpler version of the <see cref="PlayerCollisionDetector"/> which uses a single channel
/// and sends a "True" event on trigger enter and a "False" event on trigger exit.
/// </summary>
public class SimplePlayerCollisionDetector : MonoBehaviour
{
    [Header("Broadcasting On")]
    [SerializeField, Tooltip("The channel to raise events to when the player enters/exits our trigger zone.")]
    BoolEventChannel _inCollisionWithPlayerChannel;

    void OnTriggerEnter(Collider other)
    {
        if (other.IsPlayer())
        {
            _inCollisionWithPlayerChannel.RaiseEvent(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.IsPlayer())
        {
            _inCollisionWithPlayerChannel.RaiseEvent(false);
        }
    }
}

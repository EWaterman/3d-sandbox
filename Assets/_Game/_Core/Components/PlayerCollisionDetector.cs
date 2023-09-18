using UnityEngine;

/// <summary>
/// Detects if the object has entered/exited collision with the player and raises associated events.
/// </summary>
public class PlayerCollisionDetector : MonoBehaviour
{
    [Header("Broadcasting On")]
    [SerializeField, Tooltip("The channel to raise events to when the player enters our trigger zone.")]
    ColliderEventChannel _triggerEnterChannel;

    [SerializeField, Tooltip("The channel to raise events to when the player exits our trigger zone.")]
    ColliderEventChannel _triggerExitChannel;

    void OnTriggerEnter(Collider other)
    {
        if (other.IsPlayer())
        {
            _triggerEnterChannel.RaiseEvent(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.IsPlayer())
        {
            _triggerExitChannel.RaiseEvent(other);
        }
    }
}

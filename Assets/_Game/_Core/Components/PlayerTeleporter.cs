using UnityEngine;

/// <summary>
/// Teleports the object tagged as Player to a new position.
/// </summary>
public abstract class PlayerTeleporter : MonoBehaviour
{
    [SerializeField]
    CharacterController _playerController;

    [SerializeField, Tooltip("The destination to teleport the player to.")]
    Transform _destination;

    [SerializeField, Tooltip("True if the player should maintain their X position after teleporting.")]
    bool _preserveX;

    [SerializeField, Tooltip("True if the player should maintain their Y position after teleporting.")]
    bool _preserveY;

    [SerializeField, Tooltip("True if the player should maintain their Z position after teleporting.")]
    bool _preserveZ;

    void OnTriggerEnter(Collider other)
    {
        if (other.IsPlayer())
        {
            OnPlayerTriggerEnter();
        }
    }

    /// <summary>
    /// Overridable. The behaviour for when the player enters the trigger zone of the portal.
    /// By default we simply teleport the player.
    /// </summary>
    protected virtual void OnPlayerTriggerEnter()
    {
        TeleportPlayer();
    }

    protected void TeleportPlayer()
    {
        // The Character Controller doesn't play nice with teleportation style movements
        // so we need to disable it before jumping the player to a new location.
        _playerController.enabled = false;

        Vector3 controllerPosition = _playerController.transform.position;

        float x = _preserveX ? controllerPosition.x : _destination.position.x;
        float y = _preserveY ? controllerPosition.y : _destination.position.y;
        float z = _preserveZ ? controllerPosition.z : _destination.position.z;

        _playerController.transform.position = new Vector3(x, y, z);

        _playerController.enabled = true;
    }
}

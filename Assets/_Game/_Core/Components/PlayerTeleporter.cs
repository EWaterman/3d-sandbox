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

    [SerializeField, Tooltip("True if the portal works from either side of the plane. False if just the front side.")]
    bool _worksFromBothSides;

    void OnTriggerEnter(Collider other)
    {
        if (other.IsPlayer())
        {
            OnPlayerTriggerEnter();
        }
    }

    /// <summary>
    /// Overridable. The behaviour for when the player enters the trigger zone of the portal.
    /// By default we simply teleport the player if they're approaching from the correct side.
    /// </summary>
    protected virtual void OnPlayerTriggerEnter()
    {
        if (IsEnteringFromCorrectSide())
            TeleportPlayer();
    }

    /// <summary>
    /// Determines if the player is entering the portal from the "open" side. This is computed
    /// by taking the dot product between a vector that is parallel to the plane and one that is
    /// pointing to the player. If this dot is positive, it means we're coming from in front of
    /// the portal.
    /// </summary>
    protected bool IsEnteringFromCorrectSide()
    {
        if (_worksFromBothSides)
            return true;

        Vector3 portalToPlayer = _playerController.transform.position - transform.position;
        return Vector3.Dot(transform.up, portalToPlayer) > 0;
    }

    protected void TeleportPlayer()
    {
        // The Character Controller doesn't play nice with teleportation style movements
        // so we need to disable it before jumping the player to a new location.
        _playerController.enabled = false;

        Vector3 playerPosition = _playerController.transform.position;

        float x = _preserveX ? playerPosition.x : _destination.position.x;
        float y = _preserveY ? playerPosition.y : _destination.position.y;
        float z = _preserveZ ? playerPosition.z : _destination.position.z;

        _playerController.transform.position = new Vector3(x, y, z);

        _playerController.enabled = true;
    }
}

using UnityEngine;

/// <summary>
/// Teleports the object tagged as Player to a new position. For use when we want a
/// bi-directional portal (can teleport there and back)
/// </summary>
public class TwoWayTeleporter : PlayerTeleporter
{
    [SerializeField] BoolVariableSO _isTeleporting;

    protected override void OnPlayerTriggerEnter()
    {
        // Because our teleporters transport the player into the trigger of the next
        // portal, we need to check if we're already coming from another portal before
        // trying to teleport.
        if (_isTeleporting.Value)
        {
            _isTeleporting.SetFalse();  // We're at our destination
        }
        else if (IsEnteringFromCorrectSide())
        {
            _isTeleporting.SetTrue();
            TeleportPlayer();
        }
    }
}

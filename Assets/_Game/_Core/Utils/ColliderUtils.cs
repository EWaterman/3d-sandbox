using UnityEngine;

/// <summary>
/// Static unility methods for enhancing and working with colliders.
/// </summary>
public static class ColliderUtils
{
    public static bool IsPlayer(this Collider collider)
    {
        return collider.CompareTag("Player");
    }
}

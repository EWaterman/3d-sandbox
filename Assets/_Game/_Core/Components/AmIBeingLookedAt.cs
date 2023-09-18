using UnityEngine;

/// <summary>
/// Determine whether the player is looking in the direction of the associated object.
/// </summary>
public class AmIBeingLookedAt : MonoBehaviour
{
    [SerializeField, Tooltip("The main camera that follows the player.")]
    Camera _playerCamera;

    [SerializeField, Tooltip("The channel to raise events to when the player looks at/away from us.")]
    BoolEventChannel _beingLookedAtChannel;

    bool _isInView;

    void Start()
    {
        _isInView = AreWeWithinView();
        RaiseEvent();
    }

    void Update()
    {
        bool isInViewThisFrame = AreWeWithinView();
        if (isInViewThisFrame == _isInView)
            return;

        _isInView = isInViewThisFrame;
        RaiseEvent();
    }

    void RaiseEvent()
    {
        _beingLookedAtChannel.RaiseEvent(_isInView);
    }

    bool AreWeWithinView()
    {
        Vector3 viewportCoordinates = _playerCamera.WorldToViewportPoint(transform.position);

        // WorldToViewportPoint returns X, Y values between 0 and 1 if we're within the viewport
        // and a positive Z value if we're in front of the camera.
        return viewportCoordinates.x >= 0 &&
            viewportCoordinates.x <= 1 &&
            viewportCoordinates.y >= 0 &&
            viewportCoordinates.y <= 1 &&
            viewportCoordinates.z > 0;
    }
}

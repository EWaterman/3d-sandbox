using UnityEngine;

/// <summary>
/// Controls the position of the portal camera and maps it to the portal plane's material
/// so that in can be rendered properly.
/// </summary>
public class PortalCameraController : MonoBehaviour
{
    [SerializeField, Tooltip("The transform of the main camera in the scene (which is mapped to the player).")]
    Transform _playerCamera;

    [SerializeField, Tooltip("The regular camera that pairs with this portal VCam.")]
    Camera _portalCamera;

    [SerializeField, Tooltip("The material that holds this camera's view.")]
    Material _portalMaterial;

    [SerializeField, Tooltip("The transform of the actual portal this camera is tied to.")]
    Transform _thisPortal;

    [SerializeField, Tooltip("The transform of the portal in the other area.")]
    Transform _otherPortal;

    void Start()
    {
        // Dynamically generate the texture for the portal plane on startup because
        // it depends on the dimensions of the game screen.
        if (_portalCamera.targetTexture != null)
            _portalCamera.targetTexture.Release();

        _portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _portalMaterial.mainTexture = _portalCamera.targetTexture;
    }

    void Update()
    {
        // Determine where the player is in reference to the other portal and ourself up to be
        // positioned in the same place in reference to our own portal. This correctly syncs up
        // the camera view through the portal plane with what the player should see.
        Vector3 playerPosRelToPortal = _playerCamera.position - _otherPortal.position;
        transform.position = _thisPortal.position + playerPosRelToPortal;
    }
}

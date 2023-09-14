using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles all inputs of the main player character. Uses the new input system and
/// the "Player Input" component.
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] float _walkSpeed;
    [SerializeField] float _sprintSpeed;
    [SerializeField] float _jumpHeight;
    [SerializeField] float _coyoteTime;
    [SerializeField] float _gravity = -9.8f;

    [Header("Required Components")]
    [SerializeField] CharacterController _characterController;
    [SerializeField] Camera _camera;

    Transform _localRefTransform;
    Vector3 _velocity = Vector3.zero;
    Vector3 _movementInputDirection = Vector2.zero;
    bool _isSprinting = false;
    bool _tryApplyJumpNextFrame = false;
    float _jumpTimer = 0;

    bool IsMovingDownward => _velocity.y < 0;
    bool CanJump => _jumpTimer > 0;

    void Awake()
    {
        _localRefTransform = _camera.transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        AdvanceJumpTimer(Time.deltaTime);
        ApplyVerticalForcesToVelocity();
        ApplyHorizontalForcesToVelocity();
        MoveCharacter();
    }

    void OnMove(InputValue value)
    {
        // OnMove only gets triggered when input is changed. For keyboard, this means
        // that if we continue to hold down a key while rotating the camera, OnMove only
        // gets called once. As such we can't convert the movement to local coordinates
        // here, we need to do it in the update loop.
        _movementInputDirection = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        _isSprinting = value.isPressed;
    }

    void OnJump()
    {
        _tryApplyJumpNextFrame = true;
    }

    void AdvanceJumpTimer(float deltaTime)
    {
        if (_characterController.isGrounded)
        {
            _jumpTimer = _coyoteTime;  // Reset the timer
        }
        else if (_jumpTimer > 0)
        {
            _jumpTimer -= deltaTime;
        }
    }

    void ApplyVerticalForcesToVelocity()
    {
        if (_tryApplyJumpNextFrame)
        {
            _tryApplyJumpNextFrame = false;

            if (CanJump)
            {
                // This equation makes the jump height more intuitive to set.
                // A value of 1 is "normal" feeling.
                _velocity.y += Mathf.Sqrt(_jumpHeight * -2f * _gravity);

                // Disables coyote time until we hit the ground again since we're
                // already jumping.
                _jumpTimer = 0;

                return;
            }
        }

        if (!_characterController.isGrounded)
        {
            // += so that we constantly accelerate as we fall. We apply a second delta
            // time here because real life acceleration of free falling depends on the
            // square of the delta time.
            _velocity.y += _gravity * Time.deltaTime;
        }
        else if (IsMovingDownward)
        {
            // Meaning we're on the ground and not jumping. Reset the y velocity
            // so we don't continue to build up speed while grounded.
            _velocity.y = 0f;
        }
    }

    /// <summary>
    /// Convert the input movement vector to be relative to the direction the player
    /// is facing and apply it to the velocity.
    /// 
    /// Important to note:
    /// For a Vector2, x is left / right movement, y is forward / back.
    /// For a Vector3, x is still left / right, but z is the forward / back.
    /// </summary>
    void ApplyHorizontalForcesToVelocity()
    {
        //TODO: remove the _localRefTransform once we're actually rotating the player, then just use our own transform.
        Vector3 localMovement =
            _localRefTransform.right * _movementInputDirection.x +
            _localRefTransform.forward * _movementInputDirection.y;

        localMovement *= (_isSprinting ? _sprintSpeed : _walkSpeed);

        _velocity.x = localMovement.x;
        _velocity.z = localMovement.z;
    }

    /// <summary>
    /// Actually perform the player movement. Should only be invoked from the Update()
    /// method and only once per frame so that we can ensure smooth movement.
    /// </summary>
    /// <param name="velocity">The direction to move the player. Should NOT have deltaTime applied yet.</param>
    void MoveCharacter()
    {
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
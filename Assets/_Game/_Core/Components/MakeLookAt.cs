using UnityEngine;

/// <summary>
/// Ensures the attached object is always facing the given transform.
/// </summary>
public class MakeLookAt : MonoBehaviour
{
    [SerializeField, Tooltip("The object that we should be looking at.")]
    Transform _target;

    [SerializeField, Tooltip("True if the object should maintain its X rotation.")]
    bool _excludeXAxis = false;

    [SerializeField, Tooltip("True if the object should maintain its Y rotation.")]
    bool _excludeYAxis = false;

    [SerializeField, Tooltip("True if the object should maintain its Z rotation.")]
    bool _excludeZAxis = false;

    void Update()
    {
        Vector3 adjustedTarget = new()
        {
            x = _excludeXAxis ? transform.position.x : _target.position.x,
            y = _excludeYAxis ? transform.position.y : _target.position.y,
            z = _excludeZAxis ? transform.position.z : _target.position.z
        };
        transform.LookAt(adjustedTarget);
    }
}

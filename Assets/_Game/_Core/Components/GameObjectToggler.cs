using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toggles objects on and off depending on the boolean result of a given event.
/// </summary>
public class GameObjectToggler : MonoBehaviour
{
    [SerializeField, Tooltip("The objects to toggle ON when the given condition is true (and OFF when it is false).")]
    List<GameObject> _objectsTurnOnWhenTrue;

    [SerializeField, Tooltip("The objects to toggle OFF when the given condition is true (and ON when it is false).")]
    List<GameObject> _objectsTurnOffWhenTrue;

    [Header("Listening To")]
    [SerializeField, Tooltip("The event that triggers toggling our objects.")]
    BoolEventChannel _toggleEventChannel;

    void OnEnable()
    {
        _toggleEventChannel.Listeners += Toggle;
    }

    void OnDisable()
    {
        _toggleEventChannel.Listeners -= Toggle;
    }

    void Toggle(bool condition)
    {
        foreach (GameObject obj in _objectsTurnOnWhenTrue)
        {
            obj.SetActive(condition);
        }

        foreach (GameObject obj in _objectsTurnOffWhenTrue)
        {
            obj.SetActive(!condition);
        }
    }
}

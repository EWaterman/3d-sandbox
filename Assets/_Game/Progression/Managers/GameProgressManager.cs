using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the state of progression through the game.
/// </summary>
public class GameProgressManager : MonoBehaviour
{
    [SerializeField, Tooltip("The scriptable object that holds the actual state.")]
    GameProgressModel _gameProgress;

    [Header("Listening To")]
    [SerializeField, Tooltip("The event that triggers when a dialogue balloon is interacted with.")]
    EmptyEventChannel _balloonInteractedWithEventChannel;

    // TODO: Quick and dirty solution.
    // Better would be to have dialogues register themselves to a list or dict and have a more proper progress tracking system.
    [Header("Config")]
    [SerializeField, Tooltip("How many dialogues we need to interact with before the ending is unlocked.")]
    int _numDialoguesBeforeEndUnlocks = 5;
    [SerializeField, Tooltip("Objects that will be enabled once we've unlocked the ending.")]
    List<Transform> _objectsToEnableOnEnd;
    [SerializeField, Tooltip("Objects that will be disabled once we've unlocked the ending.")]
    List<Transform> _objectsToDisableOnEnd;

    void OnEnable()
    {
        _balloonInteractedWithEventChannel.Listeners += OnDialogueListenedTo;
    }

    void OnDisable()
    {
        _balloonInteractedWithEventChannel.Listeners -= OnDialogueListenedTo;
    }

    void Awake()
    {
        _gameProgress.ResetState();
    }

    void OnDialogueListenedTo()
    {
        // We don't care at this point what dialogue was listened to, only that one was.
        // This is fine to track as a simple counter for now since each balloon can only
        // be interacted with once.
        _gameProgress.NumDialoguesListenedTo++;

        if (_gameProgress.NumDialoguesListenedTo == _numDialoguesBeforeEndUnlocks)
            OnAllDialoguesListenedTo();
    }

    void OnAllDialoguesListenedTo()
    {
        // TODO: use the GameObjectToggler
        foreach (Transform t in _objectsToEnableOnEnd)
        {
            t.gameObject.SetActive(true);
        }

        foreach (Transform t in _objectsToDisableOnEnd)
        {
            t.gameObject.SetActive(false);
        }
    }
}

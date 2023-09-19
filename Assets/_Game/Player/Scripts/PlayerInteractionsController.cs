using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the interaction inputs of the main player character.
/// Uses the new input system and the "Player Input" component.
/// </summary>
public class PlayerInteractionsController : MonoBehaviour
{
    /// <summary>The objects that are currently interactable (ie within the interaction zone).</summary>
    readonly LinkedList<Interactable> _interactables = new();

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out Interactable interactable))
        {
            _interactables.AddLast(interactable);

            interactable.OnEnterInteractRange();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out Interactable interactable))
        {
            _interactables.Remove(interactable);

            interactable.OnExitInteractRange();
        }
    }

    /// <summary>
    /// Player input. Interact with the closest interactable we find within range.
    /// </summary>
    void OnInteract()
    {
        LinkedListNode<Interactable> firstNode = FindFirstAvailableInteractable();
        if (firstNode == null)
            return;

        Interactable interactable = firstNode.Value;
        interactable.OnInteract();

        // We only ever interact with things once before they're destroyed so it's quicker
        // to just remove them from the list here the moment we interact.
        _interactables.Remove(firstNode);
    }

    /// <summary>
    /// Loop through the list of interactables, returning the first non-null.
    /// Nulls can happen if the items have gotten destroyed by something outside of this class.
    /// </summary>
    LinkedListNode<Interactable> FindFirstAvailableInteractable()
    {
        LinkedListNode<Interactable> node = _interactables.First;
        while (node != null)
        {
            if (node.Value != null && node.Value.gameObject.activeInHierarchy)
                break;

            LinkedListNode<Interactable> next = node.Next;

            // Might as well remove null/destroyed interactables as we find them.
            _interactables.Remove(node);
            Debug.LogWarning("Removing object from interactables list because it no longer exists.");

            node = next;
        }

        return node;
    }
}

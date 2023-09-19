using UnityEngine;

/// <summary>
/// Holds the state of progression through the game.
/// </summary>
[CreateAssetMenu(menuName = "GameProgress/GameProgressModel")]
public class GameProgressModel : ScriptableObject
{
    public int NumDialoguesListenedTo = 0;

    public void ResetState()
    {
        NumDialoguesListenedTo = 0;
    }
}

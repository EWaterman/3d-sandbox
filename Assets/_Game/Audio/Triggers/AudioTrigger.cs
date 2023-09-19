using UnityEngine;

/// <summary>
/// Simply triggers an audio cue when invoked.
/// </summary>
public class AudioTrigger : MonoBehaviour
{
    [Header("Sound definition")]
    [SerializeField] AudioClip _clip;

    [Header("Broadcasting On")]
    [SerializeField] AudioClipEventChannel _audioEventChannel;

    /// <summary>
    /// Causes the audio clip to be played.
    /// </summary>
    public void TriggerAudioCue()
    {
        _audioEventChannel.RaiseEvent(_clip);
    }
}

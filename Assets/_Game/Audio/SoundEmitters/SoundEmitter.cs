using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/**
 * Pairs with an AudioSource component to play audio and to notify the AudioManager upon completion.
 */
[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    /**
     * Instructs the AudioSource to play a single clip in a position in 3D space.
     */
    public void PlayAudioClip(AudioClip clip, AudioConfigModel settings)
    {
        _audioSource.clip = clip;
        //settings.ApplyTo(_audioSource);  // don't need custom config currently
        //_audioSource.transform.position = whereToPlay;  // just play a global sound for now without a position
        _audioSource.Play();

        StartCoroutine(FinishedPlaying(clip.length));
    }

    /**
     * Resumes audio that was paused.
     * Needed to support game pausing.
     */
    public void Resume()
    {
        _audioSource.Play();
    }

    /**
     * Pauses the audio mid-way through.
     * Needed to support game pausing.
     */
    public void Pause()
    {
        _audioSource.Pause();
    }

    /**
     * Used when the SFX finished playing. Called by the AudioManager.
     */
    public void Stop() // Redundant?
    {
        _audioSource.Stop();
    }

    public bool IsInUse()
    {
        return _audioSource.isPlaying;
    }

    /**
     * Used to notify the AudioManager (via Unity Action) that the sound has played fully.
     */
    IEnumerator FinishedPlaying(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        OnSoundFinishedPlaying.Invoke(this);
    }
}

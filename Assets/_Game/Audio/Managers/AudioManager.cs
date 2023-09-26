using UnityEngine;
using UnityEngine.Audio;

/**
 * Handles all things audio. Listens to channels that trigger audio cues,
 * then spawns sound emitters to play the given audio when those cues are triggered.
 * 
 * Shouldn't contain any logic about what audio to play and how. That all gets passed in by the triggers.
 * 
 * Derived from code here: https://github.com/UnityTechnologies/open-project-1/tree/devlogs/2-scriptable-objects/UOP1_Project/Assets/Scripts/Audio
 */
public class AudioManager : MonoBehaviour
{
    const string AUDIO_MIXER_GROUP_PARAM_MASTER_VOLUME = "MasterVolume";

    [Header("Global config")]
    [SerializeField] AudioConfigModel _globalAudioConfig;  // We always use the same config for now

    [Header("SoundEmitters pool")]
    [SerializeField] SoundEmitterPool _pool;
    [SerializeField] int _initialSize = 3;

    [Header("Listening to")]
    [SerializeField] AudioClipEventChannel _audioEventChannel;

    [Header("Audio control")]
    [SerializeField] AudioMixer audioMixer;
    [Range(0f, 1f)] [SerializeField] float _masterVolume = 1f;

    void Awake()
    {
        _pool.Prewarm(_initialSize);
        _pool.SetParent(transform);
    }
    void OnEnable()
    {
        _audioEventChannel.Listeners += PlayAudioClip;
    }

    void OnDisable()
    {
        _audioEventChannel.Listeners -= PlayAudioClip;
    }

    /// <summary>
    /// Used in the Inspector to debug volumes. Is called when any of the variables change, and
    /// will directly change the value of the volumes on the AudioMixer.
    /// </summary>
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            SetGroupVolume(AUDIO_MIXER_GROUP_PARAM_MASTER_VOLUME, _masterVolume);
        }
    }

    public void SetGroupVolume(string parameterName, float normalizedVolume)
    {
        bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
        if (!volumeSet)
            Debug.LogError("The AudioMixer parameter [" + parameterName + "] was not found");
    }

    public float GetGroupVolume(string parameterName)
    {
        if (audioMixer.GetFloat(parameterName, out float rawVolume))
        {
            return MixerValueToNormalized(rawVolume);
        }
        else
        {
            Debug.LogError("The AudioMixer parameter was not found");
            return 0f;
        }
    }

    float MixerValueToNormalized(float mixerValue)
    {
        // We're assuming the range [-80dB to 0dB] becomes [0 to 1]
        return 1f + (mixerValue / 80f);
    }
    float NormalizedToMixerValue(float normalizedValue)
    {
        // We're assuming the range [0 to 1] becomes [-80dB to 0dB]
        // This doesn't allow values over 0dB
        return (normalizedValue - 1f) * 80f;
    }

    /// <summary>
    /// Pull a sound emitter from the pool and play an audio group through it.
    /// </summary>
    public void PlayAudioClip(AudioClip audioToPlay)
    {
        SoundEmitter soundEmitter = _pool.Request();
        if (soundEmitter != null)  // This should never be null because we create new instances on demand but check anyways.
        {
            soundEmitter.PlayAudioClip(audioToPlay, _globalAudioConfig);
            soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
        }
    }

    /// <summary>
    /// Stops a sound emitter and returns it to the pool.
    /// </summary>
    void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
        soundEmitter.Stop();
        _pool.Return(soundEmitter);
    }
}

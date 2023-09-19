using UnityEngine;

/**
 * Factory for spawining SoundEmitter instances.
 */
[CreateAssetMenu(fileName = "NewSoundEmitterFactory", menuName = "Audio/SoundEmitter Factory")]
public class SoundEmitterFactory : Factory<SoundEmitter>
{
    public SoundEmitter prefab = default;

    public override SoundEmitter Create()
    {
        return Instantiate(prefab);
    }
}

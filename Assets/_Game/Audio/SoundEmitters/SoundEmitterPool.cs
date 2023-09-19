using UnityEngine;

[CreateAssetMenu(fileName = "SoundEmitterPool", menuName = "Audio/SoundEmitter Pool")]
public class SoundEmitterPool : ComponentPool<SoundEmitter>
{
    [SerializeField] SoundEmitterFactory _factory;

    public override IFactory<SoundEmitter> factory
    {
        get
        {
            return _factory;
        }
        set
        {
            _factory = value as SoundEmitterFactory;
        }
    }
}

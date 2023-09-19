using UnityEngine;
using DG.Tweening;

/// <summary>
/// Controls the main functionality of the floating speech bubbles.
/// </summary>
public class SpeechBubbleController : Interactable
{
    [Header("Required Components")]
    [SerializeField, Tooltip("The component responsible for triggering the audio clip to play.")]
    AudioTrigger _audioTrigger;
    [SerializeField, Tooltip("The Game Object containing the model/image of the speech bubble.")]
    Transform _bubbleModelObject;

    [Header("Broadcasting On")]
    [SerializeField, Tooltip("The event that triggers when a dialogue balloon is interacted with.")]
    EmptyEventChannel _balloonInteractedWithEventChannel;

    [Header("Config - Enter interact zone")]
    [SerializeField, Tooltip("How long zooming in/out should take when entering/exiting the interact radius of the player.")]
    float _enterInteractZoneAnimationDuration = 0.5f;
    [SerializeField, Tooltip("The % amount to scale the bubble when entering the interact radius of the player.")]
    float _enterInteractZoneScaleAmount = 1.3f;

    [Header("Config - On Interaction")]
    [SerializeField, Tooltip("How fast the bubble should scale/move up after the player interacts with it.")]
    float _onInteractAnimationPart1Duration = 0.5f;
    [SerializeField, Tooltip("How fast the bubble should scale/move down after the player interacts with it.")]
    float _onInteractAnimationPart2Duration = 0.5f;

    bool _hasBeenInteractedWith = false;  // THIS IS GETTING SET TO TRUE FOR ALL BUBBLES WHEN ONE IS INTERACTED WITH???????????? HUNH? Is this because abstract classes??

    public override void OnEnterInteractRange()
    {
        transform.DOShakeScale(_enterInteractZoneAnimationDuration,
            strength : _enterInteractZoneScaleAmount,
            vibrato : 10,
            randomness : 25f,
            randomnessMode : ShakeRandomnessMode.Harmonic);
    }

    public override void OnInteract()
    {
        // We only allow a button to be pressed once.
        if (_hasBeenInteractedWith)
        {
            return;
        }

        _hasBeenInteractedWith = true;

        _balloonInteractedWithEventChannel.RaiseEvent();

        _audioTrigger.TriggerAudioCue();

        DisappearBubble();
    }

    Tween ScaleBubble(float duration, float scaleAmount = 1, Ease ease = Ease.InOutSine)
    {
        return _bubbleModelObject.DOScale(scaleAmount, duration).SetEase(ease);
    }

    Tween MoveYBubble(float duration, float position)
    {
        return _bubbleModelObject.DOMoveY(position, duration);
    }

    void DisappearBubble()
    {
        float balloonCurrentY = _bubbleModelObject.position.y;

        // Start by briefly scaling up the balloon and moving it up
        Sequence sequencePart1 = DOTween.Sequence()
            .Append(MoveYBubble(_onInteractAnimationPart1Duration, balloonCurrentY + 0.2f));

        // Then shrink it down to zero and drop it to the ground
        Sequence sequencePart2 = DOTween.Sequence()
            .Append(MoveYBubble(_onInteractAnimationPart2Duration, balloonCurrentY - 1.25f))
            .Insert(0, ScaleBubble(_onInteractAnimationPart2Duration, 0));

        DOTween.Sequence()
            .Append(sequencePart1)
            .Append(sequencePart2)
            .Play()
            .OnComplete(OnBubbleDisappearComplete);
    }

    void OnBubbleDisappearComplete()
    {
        // We only allow a button to be pressed once so destroy ourself once its gone.
        Destroy(transform.gameObject);
    }
}

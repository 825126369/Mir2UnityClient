using UnityEngine;

public class AnimationEventFunc : MonoBehaviour
{
    public void OnEvent(string args)
    {
        onFire.Instance.fire(GameEvents.OnAnimationEvent, args);
    }
}


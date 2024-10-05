using UnityEngine;

public class AnimationEventFunc : MonoBehaviour
{
    public void OnEvent(string args)
    {
        EventMgr.Instance.Broadcast(GameEvents.OnAnimationEvent, args);
    }
}


using UnityEngine;

public class AnimatorTrigger : MonoBehaviour {

    public event System.Action<string> OnAnimationEvent;
    
    private void AnimationEvent(string eventName) {
        if (OnAnimationEvent != null)
            OnAnimationEvent(eventName);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventDetector : MonoBehaviour {

    public event Action<string> OnAnimationEvent;

    [SerializeField] private List<string> eventNamesFilter;

    public void AnimationEvent(string eventName) {
        if (eventNamesFilter.Count == 0 || eventNamesFilter.Contains(eventName)) {
            if (OnAnimationEvent != null)
                OnAnimationEvent(eventName);
        }
    }
}

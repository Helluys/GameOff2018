using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIcon : MonoBehaviour {

    [SerializeField] private RectTransform displayObject;
    [SerializeField] private float animationTime = 1;
    private bool isLoading = false;


    float originalSize;
    Quaternion originalRot;

    private void Awake()
    {
        originalSize = displayObject.sizeDelta.x;
        originalRot = displayObject.localRotation;
    }

    public void StartLoading()
    {
        UpdateDeltaSize(originalSize);
        displayObject.localRotation = originalRot;
        isLoading = true;
        RotateAnimation();
    }

    public void StopLoading()
    {
        isLoading = false;
    }

    public void RotateAnimation()
    {
        LeanTween.value(this.gameObject, UpdateDeltaSize, originalSize, 1.3f * originalSize, animationTime/2).setOnComplete(
           () => LeanTween.value(this.gameObject, UpdateDeltaSize, 1.3f * originalSize, originalSize, animationTime/2).setEaseOutSine());
        LeanTween.rotateAround(displayObject, Vector3.forward, -360, animationTime).setEaseOutSine();
        LeanTween.delayedCall(animationTime,LoopAnimation );
    }

    public void LoopAnimation()
    {
        if (isLoading)
            RotateAnimation();
    }
    

    private void UpdateDeltaSize(float val)
    {
        displayObject.sizeDelta = new Vector2(val, val);
    }
}

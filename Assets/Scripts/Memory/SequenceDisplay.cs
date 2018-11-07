using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceDisplay : MonoBehaviour {

    #region Variables
    [Header("Display Settings")]
    [SerializeField] [Range(0.1f, 2.0f)] private float displayTime = 0.5f;
    [SerializeField] [Range(1.0f, 5.0f)] private float onOffRatio = 2;
    [SerializeField] private Color indicatorOnColor, indicatorOffColor;
    [Header("References")]
    [SerializeField] private SequenceDisplayElement[] displayElements;
    [SerializeField] private RectTransform displayObject;
    [SerializeField] private Image playIndicator;
    [SerializeField] private RectTransform SuccesEffect;

    // Whether or not the display is doing something, the state of this variable is always display on screen with the play indicator
    private bool isRunning = false;
    public bool IsRunning { get { return isRunning; }
        private set {
            isRunning = value;
            if (value)
            {
                playIndicator.color = indicatorOffColor;
                LeanTween.cancel(playIndicator.rectTransform);
                playIndicator.rectTransform.localScale = Vector3.one;

            }
            else
            {
                playIndicator.color = indicatorOnColor;
                playIndicator.rectTransform.localScale = Vector3.one * 0.8f;
                LeanTween.scale(playIndicator.rectTransform, Vector3.one * 1.1f, 0.5f).setEaseOutExpo().setLoopPingPong();
            }
        }
    }

    private Coroutine cPlayDisplay;
    #endregion
    #region Methods
    /// <summary>
    /// Display the sequence
    /// </summary>
    /// <param name="sequence">Sequence to display</param>
    public void Display(Sequence sequence)
    {
        StartCoroutine(EDisplay(sequence));
    }
    /// <summary>
    /// Coroutine for the sequence displqy
    /// </summary>
    /// <param name="sequence">Sequence to display</param>
    IEnumerator EDisplay(Sequence sequence)
    {
        // Check if there's enough elements to display the sequence
        if(sequence.sequencePossibility > displayElements.Length)
        {
            Debug.LogError("Too few elements in the SequenceDisplay to display the sequence.");
            yield break;
        }

        IsRunning = true;
        int index = -1;
        List<int> list = sequence.GetList();

        for(int i = 0; i < list.Count; i++)
        {
            index = list[i];
            TurnOn(index,true,displayTime);
            yield return new WaitForSeconds(displayTime+displayTime/onOffRatio);
        }
        IsRunning = false;
    }
    /// <summary>
    /// Turn on a given display element
    /// </summary>
    /// <param name="index">index of the element to turn on</param>
    /// <param name="time">time the element should stay on before fading</param>
    public void TurnOn(int index,bool sound = true,float time = 0.5f)
    {
        displayElements[index].TurnOn(time);
        if(sound)
        SoundController.Instance.PlaySound((SoundName)index);
    }
    #region Animations
    /// <summary>
    /// Animation for a correct sequence
    /// </summary>
    public void SuccessAnimation()
    {
        IsRunning = true;

        for (int i = 0; i < displayElements.Length; i++)
            TurnOn(i,false);


        float originalSize = displayObject.sizeDelta.x;
        LeanTween.value(this.gameObject, UpdateDeltaSize, originalSize, 1.3f * originalSize, 0.5f).setOnComplete(
           () => LeanTween.value(this.gameObject, UpdateDeltaSize,1.3f* originalSize, originalSize, 0.5f).setEaseOutBounce()) ;
        LeanTween.rotateAround(displayObject, Vector3.forward, 360, 1.0f).setEaseOutSine();
        LeanTween.delayedCall(1.5f, () => IsRunning = false);

        RectTransform rect = Instantiate(SuccesEffect, displayObject.position, Quaternion.identity);
        rect.parent = displayObject;
        rect.SetAsFirstSibling();
        LeanTween.rotateAround(rect, Vector3.forward, 360, 2.0f).setEaseOutQuad();
        LeanTween.alphaCanvas(rect.GetComponent<CanvasGroup>(), 0, 2.0f).setEaseOutQuad();
        LeanTween.scale(rect, Vector3.one * 5.0f, 2.0f).setEaseOutQuad().setOnComplete(() => Destroy(rect.gameObject));
    }
    /// <summary>
    /// Animation for a wrong sequence
    /// </summary>
    public void FailAnimation()
    {
        IsRunning = true;
        float originalSize = displayObject.sizeDelta.x;
        LeanTween.value(this.gameObject, UpdateDeltaSize, originalSize, 0.78f * originalSize, .5f).setOnComplete(
           () => LeanTween.value(this.gameObject, UpdateDeltaSize, 0.78f * originalSize, originalSize, .5f).setEaseOutBounce());
       LeanTween.rotateAround(displayObject, Vector3.forward, 45, .5f).setOnComplete(
            () => LeanTween.rotateAround(displayObject, Vector3.forward, -45, .5f).setEaseOutElastic());
        LeanTween.delayedCall(1.5f, () => IsRunning = false);
    }
    /// <summary>
    /// Use to set the width and height of the rectTransform through leantween
    /// </summary>
    /// <param name="val"></param>
    private void UpdateDeltaSize(float val)
    {
        displayObject.sizeDelta = new Vector2(val,val);
    }
    #endregion
    #endregion
}

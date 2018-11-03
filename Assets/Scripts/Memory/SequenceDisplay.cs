using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceDisplay : MonoBehaviour {

    #region Variables
    [Header("Display Settings")]
    [SerializeField] [Range(0.1f, 2.0f)] private float displayTime = 0.5f;
    [SerializeField] [Range(1.0f, 5.0f)] private float onOffRatio = 2;
    [Header("References")]
    [SerializeField] private SequenceDisplayElement[] displayElements;
    [SerializeField] private RectTransform displayObject;
    public bool isRunning { get; private set;}
    private Coroutine cPlayDisplay;
    #endregion
    #region Methods
    #region Unity
    private void Start()
    {
        isRunning = false;
    }
    #endregion
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

        isRunning = true;
        int index = -1;
        List<int> list = sequence.GetList();

        for(int i = 0; i < list.Count; i++)
        {
            index = list[i];
            displayElements[index].TurnOn(displayTime);
            yield return new WaitForSeconds(displayTime+displayTime/onOffRatio);
        }
        isRunning = false;
    }
    /// <summary>
    /// Turn on a given display element
    /// </summary>
    /// <param name="index">index of the element to turn on</param>
    /// <param name="time">time the element should stay on before fading</param>
    public void TurnOn(int index,float time = 0.5f)
    {
        displayElements[index].TurnOn(time);
    }

    public void SuccessAnimation()
    {
       float originalSize = displayObject.sizeDelta.x;
        LeanTween.value(this.gameObject, UpdateDeltaSize, originalSize, 1.3f * originalSize, 0.5f).setOnComplete(
           () => LeanTween.value(this.gameObject, UpdateDeltaSize,1.3f* originalSize, originalSize, 0.5f).setEaseOutBounce()) ;
        LeanTween.rotateAround(displayObject, Vector3.forward, 360, 1.0f).setEaseOutSine();
    }

    public void FailAnimation()
    {
        float originalSize = displayObject.sizeDelta.x;
        LeanTween.value(this.gameObject, UpdateDeltaSize, originalSize, 0.7f * originalSize, 0.5f).setOnComplete(
           () => LeanTween.value(this.gameObject, UpdateDeltaSize, 0.7f * originalSize, originalSize, 0.5f).setEaseOutBounce());
        LeanTween.rotateAround(displayObject, Vector3.forward, -360, 1.0f).setEaseOutElastic();
    }

    private void UpdateDeltaSize(float val)
    {
        displayObject.sizeDelta = new Vector2(val,val);
    }
    #endregion
}

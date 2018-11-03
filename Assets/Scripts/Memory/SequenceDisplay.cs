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

    public void SetDisplay(int index,float time = 0.5f)
    {
        displayElements[index].TurnOn(time);
    }
    #endregion
}

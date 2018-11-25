using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SequencePlayer : MonoBehaviour {

    #region Variables
    private Coroutine cSequenceplay;

    // Events
    public delegate void Del_KeyPressEvent(InputType type,bool last = false);
    public Del_KeyPressEvent OnValidKeyPress;
    public Del_KeyPressEvent OnKeyPress;
    public delegate void Del_SequenceEvent();
    public Del_SequenceEvent OnSuccess;
    public Del_SequenceEvent OnFail;
    #endregion

    #region Methods
    #region Public
    /// <summary>
    /// Start listening for the player input
    /// </summary>
    /// <param name="sequence"></param>
    public void StartSequencePlay(Sequence sequence)
    {
        if (sequence.sequencePossibility > 4)
        {
            Debug.LogError("Too few keys set to play the sequence");
            return;
        }
        cSequenceplay = StartCoroutine(ESequencePlay(sequence));
    }
    /// <summary>
    /// Stop listening for player input
    /// </summary>
    public void StopSequencePlay()
    {
        if (cSequenceplay != null)
            StopCoroutine(cSequenceplay);
    }
    #endregion
    #region Private
    /// <summary>
    /// Raise event on key press
    /// </summary>
    /// <returns></returns>
    private InputType keyPress()
    {
        foreach(InputType input in InputManager.Instance.simonInputs)
        {
            if (InputManager.Instance.IsKeyDown(input))
            {
                if (OnKeyPress != null)
                    OnKeyPress(input);

                return input;
            }
        }
        return InputType.None;
    }
    /// <summary>
    /// Coroutine used to listen for the player input + verification of the sequence
    /// </summary>
    /// <param name="sequence"></param>
    /// <returns></returns>
    private IEnumerator ESequencePlay(Sequence sequence)
    {
        InputType input = InputType.None;
        List<int> list = sequence.GetList();
        for (int i = 0; i < list.Count; i++)
        {
            yield return new WaitUntil(() => (input = keyPress()) != InputType.None);
            if (!sequence.Verify(i, InputManager.Instance.simonInputsValues[input]))
            {
                if (OnFail != null)
                    OnFail();
                yield break;
            }
            if (OnValidKeyPress != null)
            {
                bool last = i == list.Count - 1;
                OnValidKeyPress(input, last);
            }
            yield return null;
        }
        if (OnSuccess != null)
            OnSuccess();
    }
    #endregion
    #endregion
}

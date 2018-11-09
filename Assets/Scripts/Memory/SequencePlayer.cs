using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencePlayer : MonoBehaviour {

    #region Variables
    private KeyCode[] keys;
    private Coroutine cSequenceplay;

    // Events
    public delegate void Del_KeyPressEvent(int index,bool last = false);
    public Del_KeyPressEvent OnValidKeyPress;
    public Del_KeyPressEvent OnKeyPress;
    public delegate void Del_SequenceEvent();
    public Del_SequenceEvent OnSuccess;
    public Del_SequenceEvent OnFail;
    #endregion

    #region Methods
    #region Public
    /// <summary>
    /// Set the key to use 
    /// </summary>
    /// <param name="keys">Arrays of key to use</param>
    public void SetKeys(KeyCode[] keys)
    {
        this.keys = keys;
    }
    /// <summary>
    /// Start listening for the player input
    /// </summary>
    /// <param name="sequence"></param>
    public void StartSequencePlay(Sequence sequence)
    {
        if (sequence.sequencePossibility > keys.Length)
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
    private int keyPress()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                if (OnKeyPress != null)
                    OnKeyPress(i);
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// Coroutine used to listen for the player input + verification of the sequence
    /// </summary>
    /// <param name="sequence"></param>
    /// <returns></returns>
    private IEnumerator ESequencePlay(Sequence sequence)
    {
        int val = -1;
        List<int> list = sequence.GetList();
        for (int i = 0; i < list.Count; i++)
        {
            yield return new WaitUntil(() => (val = keyPress()) != -1);
            if (!sequence.Verify(i, val))
            {
                if (OnFail != null)
                    OnFail();
                yield break;
            }
            if (OnValidKeyPress != null)
            {
                bool last = i == list.Count - 1;
                OnValidKeyPress(val, last);
            }
            yield return new WaitForEndOfFrame();
        }
        if (OnSuccess != null)
            OnSuccess();
    }
    #endregion
    #endregion
}

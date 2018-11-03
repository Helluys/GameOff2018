using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencePlayer : MonoBehaviour {

    private KeyCode[] keys;

    public delegate void Del_KeyPressEvent(int index);
    public Del_KeyPressEvent OnValidKeyPress;
    public delegate void Del_SequenceEvent();
    public Del_SequenceEvent OnSuccess;
    public Del_SequenceEvent OnFail;
    private Coroutine cSequenceplay;

    public void SetKeys(KeyCode[] keys)
    {
        this.keys = keys;
    }

    private int keyPress()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                return i;
            }
        }
        return -1;
    }

    public void StartSequencePlay(Sequence sequence)
    {
        if(sequence.sequencePossibility > keys.Length)
        {
            Debug.LogError("Too few keys set to play the sequence");
            return;
        }

       cSequenceplay = StartCoroutine(ESequencePlay(sequence));
    }
    
    IEnumerator ESequencePlay(Sequence sequence)
    {
        Debug.Log("Start sequence playing");
        int val = -1;
        List<int> list = sequence.GetList();
        for (int i = 0;i< list.Count; i++)
        {
            yield return new WaitUntil(() => (val = keyPress()) != -1);
            if (!sequence.Verify(i, val))
            {
                if (OnFail != null)
                    OnFail();
                yield break;
            }
            if (OnValidKeyPress != null)
                OnValidKeyPress(val);
            yield return new WaitForEndOfFrame();
        }
        if (OnSuccess != null)
            OnSuccess();
    }

    public void StopSequencePlay()
    {
        if (cSequenceplay != null)
            StopCoroutine(cSequenceplay);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public Sequence sequence;
    public SequenceDisplay display;
    public SequencePlayer player;

    private void Start()
    {
        sequence = new Sequence(4);
        player.SetKeys(new KeyCode[] { KeyCode.Keypad8, KeyCode.Keypad6, KeyCode.Keypad4, KeyCode.Keypad2 });
        player.OnKeyPress += OnKeyPress;
        player.OnValidKeyPress += OnValidKey;
        player.OnSuccess += OnSuccess;
        player.OnFail += OnFail;
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.N))
            sequence.StartNew();
        if (Input.GetKeyDown(KeyCode.C))
            sequence.Complexify();
        if (Input.GetKeyDown(KeyCode.U))
            sequence.UnComplexify();
        if (Input.GetKeyDown(KeyCode.D))
        {
            sequence.DebugDisplay();
            display.Display(sequence);
        }

        if (Input.GetKeyDown(KeyCode.A))
            player.StartSequencePlay(sequence);
            */
        if (Input.GetKeyDown(KeyCode.P))
            StartCoroutine(EGamePhase(true));
    }

    private void OnKeyPress(int index,bool last)
    {
        display.TurnOn(index,true,0.3f);
    }

    private void OnValidKey(int index,bool last)
    {

    }

    private void OnSuccess()
    {
        display.SuccessAnimation();
        StartCoroutine(EGamePhase());
        SoundController.Instance.PlaySound(SoundName.sequenceSuccess);
    }

    private void OnFail()
    {
        display.FailAnimation();
        sequence.UnComplexify();
        StartCoroutine(EGamePhase());
        SoundController.Instance.PlaySound(SoundName.sequenceFail);
    }

    private IEnumerator EGamePhase(bool first = false)
    {
        yield return new WaitUntil(() => !display.IsRunning);

        if (first)
            sequence.StartNew();
        else
            sequence.Complexify();
   
        display.Display(sequence);
        yield return new WaitUntil(() => !display.IsRunning);
        player.StartSequencePlay(sequence);
    }
}

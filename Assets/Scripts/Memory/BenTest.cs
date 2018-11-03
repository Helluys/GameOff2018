using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenTest : MonoBehaviour
{

    private Sequence sequence;
    public SequenceDisplay display;
    public SequencePlayer player;

    private void Start()
    {
        sequence = new Sequence(4);
        player.SetKeys(new KeyCode[] { KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3 });
        player.OnKeyPress += OnKeyPress;
        player.OnValidKeyPress += OnValidKey;
        player.OnSuccess += OnSuccess;
        player.OnFail += OnFail; 
    }

    private void Update()
    {
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
    }

    private void OnKeyPress(int index)
    {
        display.TurnOn(index);
    }

    private void OnValidKey(int index)
    {
        Debug.Log("Valid");
    }

    private void OnSuccess()
    {
        Debug.Log("Success");
    }

    private void OnFail()
    {
        Debug.Log("Fail");
    }
}

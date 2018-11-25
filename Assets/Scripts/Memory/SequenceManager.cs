using System.Collections;
using UnityEngine;

public class SequenceManager : MonoBehaviour {
    public Sequence sequence;
    public SequenceDisplay display;
    public SequencePlayer player;

    private void Start () {
        sequence = new Sequence(4);
        player.OnKeyPress += OnKeyPress;
        player.OnValidKeyPress += OnValidKey;
        player.OnSuccess += OnSuccess;
        player.OnFail += OnFail;

        Invoke("StartGamePhase", 2f);
    }

    private void StartGamePhase() {
        StartCoroutine(EGamePhase(true));
    }

    private void OnKeyPress (InputType input, bool last) {
        display.TurnOn(InputManager.Instance.simonInputsValues[input], true, 0.3f);
    }

    private void OnValidKey (InputType input, bool last) {

    }

    private void OnSuccess () {
        display.SuccessAnimation();
        StartCoroutine(EGamePhase());
        SoundController.Instance.PlaySound(SoundName.sequenceSuccess);
    }

    private void OnFail () {
        display.FailAnimation();
        sequence.UnComplexify();
        StartCoroutine(EGamePhase());
        SoundController.Instance.PlaySound(SoundName.sequenceFail);
    }

    private IEnumerator EGamePhase (bool first = false) {
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

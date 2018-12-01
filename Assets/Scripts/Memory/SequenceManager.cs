using System.Collections;
using UnityEngine;


public class SequenceManager : MonoBehaviour {

    public Sequence sequence;
    public SequenceDisplay display;
    public SequencePlayer player;

    public int maxSequence { get; private set; }

    private Coroutine gamePhaseCoroutine;

    private void Start () {
        maxSequence = 0;
        sequence = new Sequence(4);
        player.OnKeyPress += OnKeyPress;
        player.OnValidKeyPress += OnValidKey;
        player.OnSuccess += OnSuccess;
        player.OnFail += OnFail;
    }

    public void StartGamePhase () {
        gamePhaseCoroutine = StartCoroutine(EGamePhase(true));
    }

    public void StopGamePhase () {
        if (gamePhaseCoroutine != null)
            StopCoroutine(gamePhaseCoroutine);
    }

    private void OnKeyPress (InputType input, bool last) {
        display.TurnOn(InputManager.Instance.simonInputsValues[input], true, 0.3f);
    }

    private void OnValidKey (InputType input, bool last) {

    }

    private void OnSuccess () {
        maxSequence = Mathf.Max(maxSequence, sequence.GetList().Count);
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

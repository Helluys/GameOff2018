using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [SerializeField] private DialogUI dialogUI;
    [SerializeField] private RectTransform awaitedKeysParent;
    [SerializeField] private GameObject awaitedKeyUIPrefab;

    [SerializeField] private string movementTutorialText;
    [SerializeField] private GameObject step1Wall;

    [SerializeField] private string rollTutorialText;
    [SerializeField] private Collider rollValidateZone;

    [SerializeField] private string memoryTutorialText1;
    [SerializeField] private string memoryTutorialText2;
    [SerializeField] private string memoryTutorialText3;
    [SerializeField] private Transform enemiesParent;

    [SerializeField] private string itemsTutorialText;
    [SerializeField] private GameObject step2Wall;
    
    [SerializeField] private string walkToPortalTutorialText;
    [SerializeField] private Collider walkValidateZone;

    [SerializeField] private string portalTutorialText1;
    [SerializeField] private string portalTutorialText2;

    [SerializeField] private string finalTutorialText;

    private SequenceManager sequenceManager;

    private WaitUntil waitStepOk;
    private WaitUntil waitRollValidate;
    private WaitUntil waitWalkValidate;
    private WaitUntil waitAllEnemiesDeath;
    private bool stepOk = false;

    private void Start () {
        sequenceManager = GameManager.instance.GetPlayer().GetComponent<SequenceManager>();
        waitStepOk = new WaitUntil(() => stepOk);
        waitRollValidate = new WaitUntil(() => rollValidateZone.bounds.Contains(Vector3.ProjectOnPlane(GameManager.instance.GetPlayer().transform.position, Vector3.up)));
        waitAllEnemiesDeath = new WaitUntil(() => enemiesParent.childCount == 0);
        waitWalkValidate = new WaitUntil(() => walkValidateZone.bounds.Contains(Vector3.ProjectOnPlane(GameManager.instance.GetPlayer().transform.position, Vector3.up)));

        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence () {
        sequenceManager.enabled = false;
        sequenceManager.display.enabled = false;
        rollValidateZone.gameObject.SetActive(false);
        walkValidateZone.gameObject.SetActive(false);
        GameManager.instance.exitPortal.enabled = false;

        // Movement
        dialogUI.DisplayText(movementTutorialText);
        yield return AwaitInput(new List<InputType>() {
            InputType.Up, InputType.Left, InputType.Right, InputType.Down
        });
        step1Wall.SetActive(false);

        // Roll
        rollValidateZone.gameObject.SetActive(true);
        dialogUI.DisplayText(rollTutorialText);
        AwaitedKeyUI awaitedKeyUIRoll = CreateAwaitedUI(false, InputType.Roll);
        yield return waitRollValidate;
        Destroy(awaitedKeyUIRoll.gameObject);
        rollValidateZone.gameObject.SetActive(false);

        // Memory : Part 1
        dialogUI.DisplayText(memoryTutorialText1);
        dialogUI.OnDisplayFinished += DialogUI_OnDisplayFinished;
        yield return waitStepOk;
        stepOk = false;
        dialogUI.OnDisplayFinished -= DialogUI_OnDisplayFinished;

        sequenceManager.enabled = true;
        sequenceManager.display.enabled = true;
        sequenceManager.StartGamePhase();

        AwaitedKeyUI awaitedKeyUIRed = CreateAwaitedUI(false, InputType.Red);
        AwaitedKeyUI awaitedKeyUIBlue = CreateAwaitedUI(false, InputType.Blue);
        AwaitedKeyUI awaitedKeyUIYellow = CreateAwaitedUI(false, InputType.Yellow);
        AwaitedKeyUI awaitedKeyUIGreen = CreateAwaitedUI(false, InputType.Green);

        sequenceManager.player.OnValidKeyPress += SequencePlayer_OnValidKeyPress;
        yield return new WaitUntil(() => stepOk);
        sequenceManager.player.OnValidKeyPress -= SequencePlayer_OnValidKeyPress;
        stepOk = false;

        // Memory : Part 2
        sequenceManager.player.OnSuccess += SequencePlayer_OnSuccess;
        dialogUI.DisplayText(memoryTutorialText2);
        yield return waitStepOk;
        sequenceManager.player.OnSuccess -= SequencePlayer_OnSuccess;
        stepOk = false;

        // Memory : Part 3
        dialogUI.DisplayText(memoryTutorialText3);
        yield return waitAllEnemiesDeath;
        stepOk = false;

        Destroy(awaitedKeyUIRed.gameObject);
        Destroy(awaitedKeyUIBlue.gameObject);
        Destroy(awaitedKeyUIYellow.gameObject);
        Destroy(awaitedKeyUIGreen.gameObject);

        // Items
        GameManager.instance.GetPlayer().items.SetItem(ItemManager.Instance.GetRandomItem(), 0);
        GameManager.instance.GetPlayer().items.SetItem(ItemManager.Instance.GetRandomItem(), 1);
        dialogUI.DisplayText(itemsTutorialText);
        yield return AwaitInput(new List<InputType>() {
            InputType.Item1, InputType.Item2
        });
        step2Wall.SetActive(false);

        // Walk to portal
        walkValidateZone.gameObject.SetActive(true);
        dialogUI.DisplayText(walkToPortalTutorialText);
        yield return waitWalkValidate;
        walkValidateZone.gameObject.SetActive(false);

        // Portal : Part 1 (wait)
        dialogUI.DisplayText(portalTutorialText1);
        dialogUI.OnDisplayFinished += DialogUI_OnDisplayFinished;
        yield return waitStepOk;
        stepOk = false;
        dialogUI.OnDisplayFinished -= DialogUI_OnDisplayFinished;

        // Portal : Part 2 (activate)
        yield return new WaitForSeconds(3f);
        dialogUI.OnDisplayFinished += DialogUI_OnDisplayFinished;
        dialogUI.DisplayText(portalTutorialText2);
        GameManager.instance.exitPortal.enabled = true;
        GameManager.instance.exitPortal.active = true;
        yield return waitStepOk;
        stepOk = false;
        dialogUI.OnDisplayFinished -= DialogUI_OnDisplayFinished;
        yield return new WaitForSeconds(1f);
        sequenceManager.player.StopSequencePlay();
        sequenceManager.player.StartSequencePlay(sequenceManager.sequence);

        // Portal : Part 3 (open)
        GameManager.instance.exitPortal.OnEnter += ExitPortal_OnEnter;
        sequenceManager.player.OnSuccess += SequencePlayer_OnSuccess;
        yield return waitStepOk;
        stepOk = false;

        dialogUI.DisplayText(finalTutorialText);
    }

    private AwaitedKeyUI CreateAwaitedUI (bool destroyOnPress, InputType input) {
        AwaitedKeyUI awaitedKeyUI = Instantiate(awaitedKeyUIPrefab, awaitedKeysParent).GetComponent<AwaitedKeyUI>();
        awaitedKeyUI.destroyOnPress = destroyOnPress;
        awaitedKeyUI.awaitedInput = input;
        return awaitedKeyUI;
    }

    private IEnumerator AwaitInput (List<InputType> expectedInput) {
        foreach (InputType input in expectedInput) {
            CreateAwaitedUI(true, input);
        }

        yield return new WaitUntil(() => awaitedKeysParent.childCount == 0);
    }

    private void SequencePlayer_OnValidKeyPress (InputType type, bool last) {
        stepOk = true;
    }

    private void SequencePlayer_OnSuccess () {
        stepOk = true;
    }

    private void DialogUI_OnDisplayFinished () {
        stepOk = true;
    }

    private void ExitPortal_OnEnter () {
        dialogUI.gameObject.SetActive(false);
    }
}

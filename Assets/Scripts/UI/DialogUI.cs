using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour {

    public event Action OnDisplayFinished;

    [SerializeField] private Text textUI;
    [SerializeField] [Range(0.1f, 1000f)] private float displaySpeed;

    private WaitForSeconds waitForDisplayDelay;
    private Coroutine displayTextCoroutine;

    private void Start () {
        waitForDisplayDelay = new WaitForSeconds(1f / displaySpeed);
    }

    public void SetDisplaySpeed(float speed) {
        waitForDisplayDelay = new WaitForSeconds(1f / displaySpeed);
    }

    internal void DisplayText (object memoryTutorialText) {
        throw new NotImplementedException();
    }

    public void DisplayText (string text) {
        if (displayTextCoroutine != null) {
            StopCoroutine(displayTextCoroutine);
        }

        textUI.text = "";
        displayTextCoroutine = StartCoroutine(DisplayTextCoroutine(text));
    }

    private IEnumerator DisplayTextCoroutine (string text) {
        for (int i = 0; i < text.Length; i++) {
            textUI.text += text[i];
            yield return waitForDisplayDelay;
        }

        if (OnDisplayFinished != null)
            OnDisplayFinished();
    }
}

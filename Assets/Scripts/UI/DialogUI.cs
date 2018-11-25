using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DialogUI : MonoBehaviour {

    [SerializeField] [Range(0.1f, 1000f)] private float displaySpeed;

    private Text textUI;
    private WaitForSeconds waitForDisplayDelay;
    private Coroutine displayTextCoroutine;

    private void Start () {
        textUI = GetComponent<Text>();
        waitForDisplayDelay = new WaitForSeconds(1f / displaySpeed);
    }

    public void SetDisplaySpeed(float speed) {
        waitForDisplayDelay = new WaitForSeconds(1f / displaySpeed);
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
    }
}

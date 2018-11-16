using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteDisplay : MonoBehaviour {

    [SerializeField] private GameObject container;
    [SerializeField] private Text timerText;

    private void Update () {
        if (!container.activeSelf && GameManager.instance.levelEnded) {
            Enable();
        }
    }

    private void Enable () {
        timerText.text = "Time: " + GameManager.instance.timer.ToString("0.00");
        container.SetActive(true);
    }

}

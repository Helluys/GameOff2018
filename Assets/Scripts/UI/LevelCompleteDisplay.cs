using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteDisplay : MonoBehaviour {
    [SerializeField] private GameObject container;
    [SerializeField] private Text timerText;
    [SerializeField] private string timerTextPrefix = "Time: ";
    [SerializeField] private Text killCountText;
    [SerializeField] private string killCountTextPrefix = "Enemies shut down: ";

    private void Update () {
        if (!container.activeSelf && GameManager.instance.levelEnded) {
            Enable();
        }
    }

    private void Enable () {
        timerText.text = timerTextPrefix + GameManager.instance.timer.ToString("0.00");
        killCountText.text = killCountTextPrefix + GameManager.instance.killCount;
        container.SetActive(true);
    }

}

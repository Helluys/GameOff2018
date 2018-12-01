using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    [SerializeField] private float appearTime = 0.5f;
    [SerializeField] private GameObject container;
    [SerializeField] private Text timerText;
    [SerializeField] private string timerTextPrefix = "Time left: ";
    [SerializeField] private Text killCountText;
    [SerializeField] private string killCountTextPrefix = "Enemies shut down: ";
    [SerializeField] private Text maxSequenceText;
    [SerializeField] private string maxSequenceTextPrefix = "Enemies shut down: ";
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start () {
        container.SetActive(false);
        GameManager.instance.GetPlayer().OnDeath += GameOverUI_OnDeath;
    }

    private void GameOverUI_OnDeath (Player player) {
        timerText.text = timerTextPrefix + GameManager.instance.timer.ToString("0.00");
        killCountText.text = killCountTextPrefix + GameManager.instance.killCount;
        maxSequenceText.text = maxSequenceTextPrefix + GameManager.instance.GetPlayer().sequenceManager.maxSequence;
        container.SetActive(true);
        StartCoroutine(EDisplayScreen());
    }

    private IEnumerator EDisplayScreen () {
        float elapsedTime = 0;
        while (elapsedTime < appearTime) {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / appearTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    public void BackToMenu () {
        SceneController.Instance.BackToMenu();
    }

    public void Retry()
    {
        SceneController.Instance.ReloadScene();
        Debug.Log("Reload");
    }
}

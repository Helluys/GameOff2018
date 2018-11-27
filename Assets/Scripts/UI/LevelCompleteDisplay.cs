using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelCompleteDisplay : MonoBehaviour {
    [SerializeField] private float appearTime = 0.5f;
    [SerializeField] private GameObject container;
    [SerializeField] private Text timerText;
    [SerializeField] private string timerTextPrefix = "Time: ";
    [SerializeField] private Text killCountText;
    [SerializeField] private string killCountTextPrefix = "Enemies shut down: ";
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup.alpha = 0;
    }
    private void Update () {
        if (!container.activeSelf && GameManager.instance.levelEnded) {
            Enable();
        }
    }

    private void Enable () {
        timerText.text = timerTextPrefix + GameManager.instance.timer.ToString("0.00");
        killCountText.text = killCountTextPrefix + GameManager.instance.killCount;
        container.SetActive(true);
        ItemManager.Instance.InitCardDisplay();
        StartCoroutine(EDisplayScreen());
    }

    private IEnumerator EDisplayScreen()
    {
        float elapsedTime = 0;
        while(elapsedTime < appearTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / appearTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

}

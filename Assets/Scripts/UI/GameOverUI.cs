﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    [SerializeField] private float appearTime = 0.5f;
    [SerializeField] private GameObject container;
    [SerializeField] private Text timerText;
    [SerializeField] private string timerTextPrefix = "Time: ";
    [SerializeField] private Text killCountText;
    [SerializeField] private string killCountTextPrefix = "Enemies shut down: ";
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start () {
        container.SetActive(false);
        GameManager.instance.GetPlayer().OnDeath += GameOverUI_OnDeath;
    }

    private void GameOverUI_OnDeath (Player player) {
        timerText.text = timerTextPrefix + GameManager.instance.timer.ToString("0.00");
        killCountText.text = killCountTextPrefix + GameManager.instance.killCount;
        container.SetActive(true);
        StartCoroutine(EDisplayScreen());
    }

    private IEnumerator EDisplayScreen () {
        float elapsedTime = 0;
        while (elapsedTime < appearTime) {
            canvasGroup.alpha = Mathf.Lerp(0, 0.8f, elapsedTime / appearTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0.8f;
    }

    public void BackToMenu () {
        SceneController.Instance.BackToMenu();
    }
}
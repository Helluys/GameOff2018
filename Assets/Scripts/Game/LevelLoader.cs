using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    [SerializeField] private GameObject loadingScreenPrefab;

    private ManualBarUI loadingBar;
    private AsyncOperation loadingOperation;
    public List<Item> storedItems { get; set; }

    public void StartLoading (string sceneName) {
        DontDestroyOnLoad(this);
        loadingBar = Instantiate(loadingScreenPrefab, FindObjectOfType<Canvas>().rootCanvas.transform).transform.GetComponentInChildren<ManualBarUI>();

        StartCoroutine(LoadSceneWithProgressBar(sceneName));
    }

    private IEnumerator LoadSceneWithProgressBar (string sceneName) {
        // Reset all tweens to avoid updates on destroyed objects
        LeanTween.reset();

        // Load scene asynchronously
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadingOperation.isDone) {
            loadingBar.manualValue = loadingOperation.progress;
            yield return null;
        }

        GameManager.instance.GetPlayer().items.SetItem(storedItems[0], 0);
        GameManager.instance.GetPlayer().items.SetItem(storedItems[1], 1);

        Destroy(gameObject);
    }
}

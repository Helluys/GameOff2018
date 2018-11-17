using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public event System.Action<string> OnSceneLoaded;

    [SerializeField] private GameObject loadingScreenPrefab;

    private ManualBarUI loadingBar;
    private AsyncOperation loadingOperation;

    public void StartLoading (string sceneName) {
        DontDestroyOnLoad(this);
        loadingBar = Instantiate(loadingScreenPrefab, FindObjectOfType<Canvas>().rootCanvas.transform).transform.GetComponentInChildren<ManualBarUI>();

        StartCoroutine(LoadSceneWithProgressBar(sceneName));
    }

    IEnumerator LoadSceneWithProgressBar (string sceneName) {
        // Reset all tweens to avoid updates on destroyed objects
        LeanTween.reset();

        // For testing purposes
        loadingBar.manualMaxValue = 1f;
        yield return new WaitForSeconds(1f);
        loadingBar.manualValue = 0.25f;
        yield return new WaitForSeconds(1f);
        loadingBar.manualValue = 0.5f;
        yield return new WaitForSeconds(1f);
        loadingBar.manualValue = 0.75f;
        yield return new WaitForSeconds(1f);
        loadingBar.manualValue = 1f;

        // Load scene asynchronously
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadingOperation.isDone) {
            loadingBar.manualValue = loadingOperation.progress;
            yield return null;
        }

        if (OnSceneLoaded != null) {
            OnSceneLoaded(sceneName);
        }
        Destroy(gameObject);
    }
}

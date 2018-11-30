using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Menu = 0,
    Tutorial = 1,
    Level1 = 2,
    Level2 = 3,
    Level3 = 4,
    Level4 = 5
}

public class SceneController : SingletonBehaviour<SceneController> {

    [SerializeField] private float minLoadingTime = 2;
    [SerializeField] private float appearTime = 0.5f;

    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Image loadingBar;
    [SerializeField] private LoadingIcon loadingIcon;
    [SerializeField] private Text loadingText;
    [SerializeField] private GameObject skipTuto;
    [SerializeField] private CanvasGroup loadingCanvas;

    private List<SceneName> levelScene = new List<SceneName>() { SceneName.Level1, SceneName.Level2, SceneName.Level3, SceneName.Level4,SceneName.Tutorial };
    private bool isLoading = false;

    private SceneName currentScene = SceneName.Menu;

    private AsyncOperation loadingOperation;
    public List<Item> storedItems;

    private void Start()
    {
        storedItems = new List<Item>();
        LoadingScreen.SetActive(false);
        loadingCanvas.alpha = 0;

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(SceneName scene)
    {
        if (isLoading || currentScene == scene)
            return;

        currentScene = scene;
        StartCoroutine(EShowLoadingScreen(true));
        StartCoroutine(ELoadScene(scene));
    }

    private void OnLevelWasLoaded(int level)
    {
        if ((SceneName)level == SceneName.Tutorial)
            skipTuto.SetActive(true);
        else
            skipTuto.SetActive(false);

        if ((SceneName)level == SceneName.Menu && storedItems != null)
            storedItems.Clear();
    }

    public void SkipTutorial()
    {
        skipTuto.SetActive(false);
        TutorialManager.Instance.Skip();
        SoundController.Instance.PlaySound(SoundName.UIButton4);
    }

    public void BackToMenu()
    {
        if (currentScene == SceneName.Menu)
            Application.Quit();

        SoundController.Instance.PlaySound(SoundName.UIButton3);
        LoadScene(SceneName.Menu);
    }

    private IEnumerator EShowLoadingScreen(bool on)
    {
        if(on) LoadingScreen.SetActive(true);
        float elapsedTime = 0;
        while(elapsedTime < appearTime)
        {
            loadingCanvas.alpha = Mathf.Lerp(on ? 0 : 1, on ? 1 : 0, elapsedTime / appearTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        loadingCanvas.alpha = on ? 1 : 0;
        if (!on) LoadingScreen.SetActive(false);
    }

    private IEnumerator ELoadScene(SceneName scene)
    {
        isLoading = true;
        // Reset all tweens to avoid updates on destroyed objects
        LeanTween.reset();

        // Load scene asynchronously
        loadingOperation = SceneManager.LoadSceneAsync((int)scene);
        loadingText.text = "Loading...";
        loadingIcon.StartLoading();
        float loadProgress = 0;
        float elapsedTime = 0;
        bool first = true;

        if(levelScene.Contains(scene))
           loadingOperation.allowSceneActivation = false;

        SoundController.Instance.DimMusic(true, minLoadingTime, 10);

        while (!loadingOperation.isDone)
        {
            loadProgress = Mathf.Min(elapsedTime / minLoadingTime, loadingOperation.progress);
            loadingBar.fillAmount = loadProgress + 0.1f;
            if (loadProgress >= 0.9f && !loadingOperation.allowSceneActivation)
            {
                if (first) {
                    first = false;
                    SoundController.Instance.PlaySound(SoundName.UIButton1);
                    loadingIcon.StopLoading();
                    loadingText.text = "Press Enter to start";
                    LeanTween.scale(loadingText.rectTransform, 1.05f * Vector3.one, 0.5f).setLoopPingPong().setEaseOutCubic();
                }

                if (Input.GetKeyDown(KeyCode.Return))
                    loadingOperation.allowSceneActivation = true;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        LeanTween.cancel(loadingText.rectTransform);
        LeanTween.scale(loadingText.rectTransform, Vector3.one, 0);

        if (levelScene.Contains(scene)){
            if (storedItems.Count > 0)
            {
                GameManager.instance.GetPlayer().items.SetItem(storedItems[0], 0);
                GameManager.instance.GetPlayer().items.SetItem(storedItems[1], 1);
            }
        }
        loadingIcon.StopLoading();
        StartCoroutine(EShowLoadingScreen(false));
        isLoading = false;
    }
}

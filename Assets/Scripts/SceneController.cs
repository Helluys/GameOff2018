using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Menu = 0,
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4
}

public class SceneController : SingletonBehaviour<SceneController> {

    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Image loadingBar;
    [SerializeField] private GameObject loadingIcon;
    [SerializeField] private Text loadingText;

    private List<SceneName> levelScene = new List<SceneName>() { SceneName.Level1, SceneName.Level2, SceneName.Level3, SceneName.Level4 };
    private bool isLoading = false;

    private AsyncOperation loadingOperation;
    public List<Item> storedItems;

    private void Start()
    {
        storedItems = new List<Item>();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(SceneName scene)
    {
        if (isLoading)
            return;

        LoadingScreen.SetActive(true);
        StartCoroutine(ELoadScene(scene));
    }

    private IEnumerator ELoadScene(SceneName scene)
    {
        isLoading = true;
        // Reset all tweens to avoid updates on destroyed objects
        LeanTween.reset();

        // Load scene asynchronously
        loadingOperation = SceneManager.LoadSceneAsync((int)scene);
        loadingText.text = "Loading...";
        loadingIcon.SetActive(true);

        if(levelScene.Contains(scene))
           loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            loadingBar.fillAmount = loadingOperation.progress + 0.1f;
            if(loadingOperation.progress >= 0.9f && !loadingOperation.allowSceneActivation)
            {
                loadingIcon.SetActive(false);
                loadingText.text = "Press Enter to start";
                if (Input.GetKeyDown(KeyCode.Space))
                    loadingOperation.allowSceneActivation = true;
            }
            yield return null;
        }

        if (levelScene.Contains(scene)){
            if (storedItems.Count > 0)
            {
                GameManager.instance.GetPlayer().items.SetItem(storedItems[0], 0);
                GameManager.instance.GetPlayer().items.SetItem(storedItems[1], 1);
            }
        }

        LoadingScreen.SetActive(false);
        isLoading = false;
    }
}

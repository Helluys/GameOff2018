using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public Animator[] animators;
    private void Start()
    {
        foreach (Animator animator in animators)
            animator.SetFloat("speed", 2.0f);
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene(0);
    }
}

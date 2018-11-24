using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField] LeanTweenType easing;
    public Animator[] animators;
    [SerializeField] new private Transform camera;

    [SerializeField] private Transform[] cameraPos;
    private Coroutine cGoToPos;

    Vector3 currentPos;
    Vector3 targetPos;
    Quaternion currentAng;
    Quaternion targetAng;

    private void Start()
    {
        foreach (Animator animator in animators)
            animator.SetFloat("speed", 2.0f);

        GoToPos(1, 3);
    }

   public void OpenInputSettings()
   {
        GoToPos(2, 2);
   }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToPos(int index, float time)
    {
        LeanTween.cancel(gameObject);
        currentPos = camera.position;
        targetPos = cameraPos[index].position;
        currentAng = camera.rotation;
        targetAng = cameraPos[index].rotation;
        LeanTween.value(gameObject, LerpUpdate, 0, 1, time).setEase(easing);
    }

    public void BackToMainView()
    {
        if (!InputManager.Instance.isWaitingForInput)
            GoToPos(1, 2);
    }

    private void LerpUpdate(float val)
    {
        camera.position = Vector3.Lerp(currentPos, targetPos, val);
        camera.rotation = Quaternion.Lerp(currentAng, targetAng, val);
    }

}

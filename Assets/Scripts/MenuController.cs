using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField] LeanTweenType easing;
    public Animator[] animators;
    [SerializeField] new private Transform camera;
    [SerializeField] private Transform[] cameraPos;

    [SerializeField] private AudioClip welcome;
    [SerializeField] private AudioClip controls;
    [SerializeField] private AudioClip preferences;
    [SerializeField] private AudioClip madeMe;

    Vector3 currentPos;
    Vector3 targetPos;
    Quaternion currentAng;
    Quaternion targetAng;

    private void Start()
    {
        camera.position = cameraPos[0].position;
        camera.rotation = cameraPos[0].rotation;

        foreach (Animator animator in animators)
            animator.SetFloat("speed", 2.0f);

        GoToPos(1, 2);
        Invoke("Welcome",0.5f);
    }

    private void Welcome()
    {
        SoundController.Instance.Say(welcome);
    }
    private void Preferences()
    {
        SoundController.Instance.Say(preferences);
    }
    private void MadeMe()
    {
        SoundController.Instance.Say(madeMe);
    }
    private void Controls()
    {
        SoundController.Instance.Say(controls);
    }

    public void OpenInputSettings()
   {
        GoToPos(2, 2);
        SoundController.Instance.PlaySound(SoundName.UIButton4);
        Invoke("Controls", 0.5f);
   }

    public void OpenOptionsSettings()
    {
        GoToPos(3, 2);
        SoundController.Instance.PlaySound(SoundName.UIButton4);
        Invoke("Preferences", 0.5f);
    }

    public void OpenCredits()
    {
        GoToPos(4, 2);
        SoundController.Instance.PlaySound(SoundName.UIButton4);
        Invoke("MadeMe", 0.5f);
    }

    public void LoadPlayScene()
    {
        SoundController.Instance.PlaySound(SoundName.UIButton4);
        SceneController.Instance.LoadScene(SceneName.Tutorial);
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
        {
            GoToPos(1, 2);
            SoundController.Instance.PlaySound(SoundName.UIButton4);
        }
        else
            SoundController.Instance.PlaySound(SoundName.UIButton3);
    }

    private void LerpUpdate(float val)
    {
        camera.position = Vector3.Lerp(currentPos, targetPos, val);
        camera.rotation = Quaternion.Lerp(currentAng, targetAng, val);
    }

}

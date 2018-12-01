using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour {

    [SerializeField] LeanTweenType easing;
    [SerializeField] new private Transform camera;
    [SerializeField] private Transform[] cameraPos;

    [SerializeField] private AudioClip thankYou;

    Vector3 currentPos;
    Vector3 targetPos;
    Quaternion currentAng;
    Quaternion targetAng;

    private void Start()
    {
        camera.position = cameraPos[0].position;
        camera.rotation = cameraPos[0].rotation;

        GoToPos(1, 60);
        Invoke("ThankYou", 1.0f);
    }

    private void ThankYou()
    {
        SoundController.Instance.Say(thankYou);
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

    private void BackToMenu()
    {
        SceneController.Instance.LoadScene(SceneName.Menu);
    }

    private void LerpUpdate(float val)
    {
        camera.position = Vector3.Lerp(currentPos, targetPos, val);
        camera.rotation = Quaternion.Lerp(currentAng, targetAng, val);
    }
}



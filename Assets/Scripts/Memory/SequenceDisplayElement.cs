using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceDisplayElement : MonoBehaviour {

    #region Variables
    [SerializeField] private Color OnColor;
    [SerializeField] private Color OffColor;
    [SerializeField] [Range(0.0f,1.0f)] private float fadeTimeRatio = 0.5f;
    private Image image;
    private Coroutine cTurnOn;
    #endregion

    #region Methods
    void Start () {
        image = GetComponent<Image>();
        image.color = OffColor;
	}
    /// <summary>
    /// Set the display element on or off
    /// </summary>
    /// <param name="displayTime">How long the display should stay on</param>
    public void TurnOn(float displayTime)
    {
        if (cTurnOn != null)
            StopCoroutine(cTurnOn);

        cTurnOn = StartCoroutine(ETurnOn(displayTime));
    }
    /// <summary>
    /// Coroutine for the turn on
    /// </summary>
    /// <param name="displayTime">How long the display should stay on</param>
    /// <returns></returns>
    private IEnumerator ETurnOn(float displayTime)
    {
        image.color = OnColor;
        yield return new WaitForSeconds(displayTime);

        float elapsedTime = 0;
        float fadeTime = displayTime * fadeTimeRatio;
        while (elapsedTime < fadeTime)
        {
            image.color = Color.Lerp(OnColor, OffColor, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = OffColor;
    }
    #endregion
}

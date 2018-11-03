using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceDisplayElement : MonoBehaviour {

    #region Variables
    [SerializeField] private Color OnColor;
    [SerializeField] private Color OffColor;
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
    /// <param name="On"></param>
	public void TurnOn(float displayTime)
    {
        if (cTurnOn != null)
            StopCoroutine(cTurnOn);

        cTurnOn = StartCoroutine(ETurnOn(displayTime));
    }

    private IEnumerator ETurnOn(float displayTime)
    {
        image.color = OnColor;
        yield return new WaitForSeconds(displayTime);
        image.color = OffColor;
    }
    #endregion
}

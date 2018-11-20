using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour {

    private Text timerDisplay;

    private void Start()
    {
        timerDisplay = GetComponent<Text>();

    }

    void Update () {

        timerDisplay.text = ConvertToTimeFormat(GameManager.instance.timer);
	}

    public string ConvertToTimeFormat(float value)
    {
        string minutes = ((int)value / 60).ToString("00");
        string seconds = (value % 60).ToString("00.00");
        string time = minutes.ToString() + ":" + seconds.ToString();
        return time.Replace(',',':');
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuOption : MonoBehaviour {

    [SerializeField] private Slider soundVolume;
    [SerializeField] private Slider musicVolume;

	// Use this for initialization
	void Start () {

        soundVolume.value = SoundController.Instance.globalSoundVolume;
        musicVolume.value = SoundController.Instance.globalMusicVolume;

        soundVolume.onValueChanged.AddListener((float f)=> SoundController.Instance.globalSoundVolume = soundVolume.value);
        musicVolume.onValueChanged.AddListener((float f) => SoundController.Instance.globalMusicVolume = musicVolume.value);
        musicVolume.onValueChanged.AddListener((float f) => SoundController.Instance.UpdateMusicVolume());
    }
}

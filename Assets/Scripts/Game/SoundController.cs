using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundName {
    button0 = 0,
    button1 = 1,
    button2 = 2,
    button3 = 3,
    sequenceSuccess = 4,
    sequenceFail = 5,
    text = 6
}

[System.Serializable]
public struct AudioVolumePair {
    public AudioClip audio;
    [Range(0, 1)] public float volume;
}

public class SoundController : SingletonBehaviour<SoundController> {

    #region Variables
    //Audio sources
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    //Getter/setter for the global volumes
    public float globalMusicVolume = 1;
    public float globalSoundVolume = 1;
    public float globalSpeechVolume = 1;

    //Playlist (sound & musics) and their volumes
    [SerializeField] private AudioVolumePair menuSong;
    [SerializeField] private AudioVolumePair[] playlist;
    [SerializeField] private AudioVolumePair[] sounds;

    /// <summary>
    /// Index of the current music in the playlist
    /// </summary>
    private int currentMusicIndex = 0;
    /// <summary>
    /// Coroutine use to play musics one after another
    /// </summary>
    private Coroutine cNextMusic;
    #endregion

    #region Methods

    #region Unity

    private void Start () {
        PlayMusic(menuSong);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.N))
            PlayNextSong();
    }

    private void OnSceneLoaded (Scene scene, LoadSceneMode loadMode) {
        SceneName sceneName = (SceneName) scene.buildIndex;
        switch (sceneName) {
            case SceneName.Menu:
                PlayMusic(menuSong);
                break;

            case SceneName.Tutorial:
                PlayMusic(playlist[2]);
                break;

            case SceneName.Level1:
                PlayMusic(playlist[0]);
                break;

            case SceneName.Level2:
                PlayMusic(playlist[1]);
                break;
        }
        musicSource.loop = true;
    }
    #endregion

    #region Public

    /// <summary>
    /// Plays the next song in the playlist
    /// </summary>
    public void PlayNextSong () {
        if (cNextMusic != null) {
            StopCoroutine(cNextMusic);
            cNextMusic = StartCoroutine(PlayNextMusic(currentMusicIndex, false));
        }
    }

    /// <summary>
    /// Plays the previous song in the playlist
    /// </summary>
    public void PlayPrevSong () {
        if (cNextMusic != null) {
            StopCoroutine(cNextMusic);
            cNextMusic = StartCoroutine(PlayNextMusic(currentMusicIndex - 2, false));
        }
    }

    /// <summary>
    /// Plays a song in the playlist
    /// </summary>
    /// <param name="index">Index of the song to play</param>
    public void PlayMusicFromPlaylist (int index) {
        PlayMusic(playlist[index]);
        currentMusicIndex = index;
        cNextMusic = StartCoroutine(PlayNextMusic(index));
    }

    /// <summary>
    /// Updates the music volume
    /// </summary>
    public void UpdateMusicVolume () {
        float currentVolume = playlist[currentMusicIndex].volume;
        musicSource.volume = currentVolume * globalMusicVolume;
    }

    /// <summary>
    /// Plays a sound
    /// </summary>
    /// <param name="sound">Name of the sound to play</param>
    public void PlaySound (SoundName sound) {
        float volume = sounds[(int) sound].volume * globalSoundVolume;
        soundSource.PlayOneShot(sounds[(int) sound].audio, volume);
    }

    public void Say (AudioClip clip) {
        soundSource.PlayOneShot(clip, globalSpeechVolume);
    }
    /// <summary>
    /// Transition between two music
    /// </summary>
    /// <param name="on">true if fade in, false in fade out</param>
    /// <param name="time">Duration of the transition</param>
    public void SoundTransition (bool on, float time) {

        LeanTween.cancel(gameObject);
        float volume = musicSource.volume;
        float begin = on ? 0 : volume;
        float end = on ? volume : 0;
        LeanTween.value(gameObject, SetMusicSourceVolume, begin, end, time);
    }

    public void DimMusic (bool on, float time, float value) {
        LeanTween.cancel(gameObject);
        float volume = musicSource.volume;
        float begin = on ? volume : volume;
        float end = on ? volume / value : volume * value;
        LeanTween.value(gameObject, SetMusicSourceVolume, begin, end, time);
    }

    #endregion

    #region Private

    /// <summary>
    /// Plays an audio clip with a given volume
    /// </summary>
    /// <param name="clip">AudioClip to play</param>
    /// <param name="volume">Volume of the audioClip</param>
    private void PlayMusic (AudioVolumePair clip) {

        if (cNextMusic != null)
            StopCoroutine(cNextMusic);

        currentMusicIndex = -1;
        musicSource.clip = clip.audio;
        musicSource.volume = clip.volume * globalMusicVolume;
        SoundTransition(true, 2.0f);
        musicSource.Play();
    }

    /// <summary>
    /// Coroutine use to play the next music
    /// </summary>
    /// <param name="index">Index of the next music to play</param>
    /// <param name="wait">Whether to wait for the current music to be over before playing the next one</param>
    /// <returns></returns>
    private IEnumerator PlayNextMusic (int index, bool wait = true) {
        if (wait)
            yield return new WaitUntil(() => !musicSource.isPlaying);
        else
            yield return null;

        int nextIndex = index + 1;
        nextIndex = nextIndex > playlist.Length - 1 ? 0 : nextIndex;
        nextIndex = nextIndex < 0 ? playlist.Length - 1 : nextIndex;
        PlayMusicFromPlaylist(nextIndex);
    }

    /// <summary>
    /// Sets the musicSource volume
    /// </summary>
    /// <param name="f">volume to set</param>
    private void SetMusicSourceVolume (float f) {
        musicSource.volume = f;
    }

    #endregion

    #endregion
}

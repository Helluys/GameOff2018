using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundName
{
    button0 = 0,
    button1 = 1,
    button2 = 2,
    button3 = 3,
    sequenceSuccess = 4,
    sequenceFail = 5
}

public class SoundController : SingletonBehaviour<SoundController> {

    #region Variables
    //Audio sources
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    //Playlist (sound & musics) and their volumes
    [SerializeField] private List<AudioClip> playlist;
    [SerializeField] [Range(0,1)] private List<float> musicsVolumes;
    [SerializeField] private List<AudioClip> sounds;
    [SerializeField] [Range(0,1)] private List<float> soundsVolumes;

    //Getter/setter for the global volumes
    public float globalMusicVolume = 1;
    public float globalSoundVolume = 1;

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
    private void Start()
    {
        PlayMusicFromPlaylist(0);
    }
    /*
    private void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 1:
                PlayMusic(menuSong, 1);
                musicSource.loop = true;
                break;
            case 2:
                PlayMusic(tutorialSong, 1);
                musicSource.loop = true;
                break;
            case 3:
            case 4:
                PlayMusicFromPlaylist(Random.Range(0, playlist.Count - 1));
                musicSource.loop = false;
                break;
            default:
                break;
        }
    }
    */
    #endregion
    #region Public
    /// <summary>
    /// Plays the next song in the playlist
    /// </summary>
    public void PlayNextSong()
    {
        if (cNextMusic != null)
        {
            StopCoroutine(cNextMusic);
            cNextMusic = StartCoroutine(PlayNextMusic(currentMusicIndex, false));
        }
    }
    /// <summary>
    /// Plays the previous song in the playlist
    /// </summary>
    public void PlayPrevSong()
    {
        if (cNextMusic != null)
        {
            StopCoroutine(cNextMusic);
            cNextMusic = StartCoroutine(PlayNextMusic(currentMusicIndex - 2, false));
        }
    }
    /// <summary>
    /// Plays a song in the playlist
    /// </summary>
    /// <param name="index">Index of the song to play</param>
    public void PlayMusicFromPlaylist(int index)
    {
        PlayMusic(playlist[index],musicsVolumes[index]);
        currentMusicIndex = index;
        cNextMusic = StartCoroutine(PlayNextMusic(index));
    }
    /// <summary>
    /// Updates the music volume
    /// </summary>
    public void UpdateMusicVolume()
    {
        float currentVolume = musicsVolumes[currentMusicIndex];
        musicSource.volume = currentVolume * globalMusicVolume;
    }
    /// <summary>
    /// Plays a sound
    /// </summary>
    /// <param name="sound">Name of the sound to play</param>
    public void PlaySound(SoundName sound)
    {
        float volume = soundsVolumes[(int)sound] * globalSoundVolume;
        soundSource.PlayOneShot(sounds[(int)sound], volume);
    }
    /// <summary>
    /// Transition between two music
    /// </summary>
    /// <param name="on">true if fade in, false in fade out</param>
    /// <param name="time">Duration of the transition</param>
    public void SoundTransition(bool on, float time)
    {
        LeanTween.cancel(this.gameObject);
        float volume = musicSource.volume;
        float begin = on ? 0 : volume;
        float end = on ? volume : 0;
        LeanTween.value(this.gameObject, SetMusicSourceVolume, begin, end, time);
    }
    #endregion
    #region Private
    /// <summary>
    /// Plays an audio clip with a given volume
    /// </summary>
    /// <param name="clip">AudioClip to play</param>
    /// <param name="volume">Volume of the audioClip</param>
    private void PlayMusic(AudioClip clip,float volume)
    {
        if (cNextMusic != null)
            StopCoroutine(cNextMusic);

        currentMusicIndex = -1;
        musicSource.clip = clip;
        musicSource.volume = volume * globalMusicVolume;
        SoundTransition(true, 1.0f);
        musicSource.Play();
    }
    /// <summary>
    /// Coroutine use to play the next music
    /// </summary>
    /// <param name="index">Index of the next music to play</param>
    /// <param name="wait">Whether to wait for the current music to be over before playing the next one</param>
    /// <returns></returns>
    private IEnumerator PlayNextMusic(int index,bool wait = true)
    {
        if (wait)
            yield return new WaitUntil(() => !musicSource.isPlaying);
        else
            yield return null;

        int nextIndex = index + 1;
        nextIndex = nextIndex > playlist.Count - 1 ? 0 : nextIndex;
        nextIndex = nextIndex < 0 ? playlist.Count - 1 : nextIndex;
        PlayMusicFromPlaylist(nextIndex);
    }
    /// <summary>
    /// Sets the musicSource volume
    /// </summary>
    /// <param name="f">volume to set</param>
    private void SetMusicSourceVolume(float f)
    {
        musicSource.volume = f;
    }
    #endregion
    #endregion
}

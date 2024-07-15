using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager Instance;

    // Audio sources for music and sound effects
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public SaveData saveData;

    [System.Serializable]
    public struct SoundEffectPool
    {
        public string category;
        public List<AudioClip> clips;
    }
    public List<SoundEffectPool> soundEffectPools;

    // List to store music tracks
    [System.Serializable]
    public struct MusicTrack
    {
        public string name;
        public AudioClip clip;
    }
    public List<MusicTrack> musicTracks;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();


        PlayNextMusicTrack();
    }
    public void Start()
    {
        ReloadSettings();

    }

    void Update()
    {

        if (!musicSource.isPlaying)
        {
            PlayNextMusicTrack();
        }


    }

    public void PlaySoundEffect(string category)
    {
        SoundEffectPool pool = soundEffectPools.Find(sep => sep.category == category);
        if (pool.clips != null && pool.clips.Count > 0)
        {
            int randomIndex = Random.Range(0, pool.clips.Count);
            AudioClip clip = pool.clips[randomIndex];
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sound effect pool not found or empty: " + category);
        }
    }

    public void ReloadSettings()
    {
        saveData = SaveSystem.LoadGame();

        musicSource.volume = saveData.musicVolume;
        sfxSource.volume = saveData.soundEffectVolume;
    }

    private void PlayNextMusicTrack()
    {
        if (musicTracks.Count == 0)
        {
            Debug.LogWarning("No music tracks available.");
            return;
        }

        int randomIndex = Random.Range(0, musicTracks.Count);
        musicSource.clip = musicTracks[randomIndex].clip;
        musicSource.Play();
    }

    public void PlayMusicTrack(string trackName)
    {
        MusicTrack musicTrack = musicTracks.Find(mt => mt.name == trackName);
        if (musicTrack.clip != null)
        {
            musicSource.clip = musicTrack.clip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music track not found: " + trackName);
        }
    }
}

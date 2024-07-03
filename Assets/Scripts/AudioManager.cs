using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager Instance;

    // Audio sources for music and sound effects
    public AudioSource musicSource;
    public AudioSource sfxSource;


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
        // Ensure only one instance of the AudioManager exists
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

        // Create audio sources
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.volume = 0.2f;

        // Play the first music track
        PlayNextMusicTrack();
    }

    void Update()
    {
        // Check if the current music track has finished playing
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

    // Function to play the next music track
    private void PlayNextMusicTrack()
    {
        if (musicTracks.Count == 0)
        {
            Debug.LogWarning("No music tracks available.");
            return;
        }

        // Randomly select a new music track from the list
        int randomIndex = Random.Range(0, musicTracks.Count);
        musicSource.clip = musicTracks[randomIndex].clip;
        musicSource.Play();
    }

    // Function to play a specific music track by name
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

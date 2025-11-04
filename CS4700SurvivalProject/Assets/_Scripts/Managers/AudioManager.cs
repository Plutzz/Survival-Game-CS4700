using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixerGroup masterMixerGroup;
    [Header("Sounds")]
    public SoundAudioClip[] soundAudioClipsArray; 
    public MMF_Player InteractSFX;

    [SerializeField]
    private Queue<UnityEngine.GameObject> soundAudioClipsQueue;
    private UnityEngine.GameObject musicGameObject;

    [SerializeField]
    private int maxAudioSources = 10;

    [Header("Music")]
    public SongAudioClip[] musicAudioClipsArray;
    


    public enum Sounds
    {
        TestSound,
        Shot,
        Bell,
        Reload,
        DoorOpen,
        DoorClosed,
    }

    public enum Songs
    {
        MenuSong,
        InterrogationSong,
        JudgementSong,
        GunpointSong,
        WanderSong,
        Outro
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            soundAudioClipsQueue = null;
        };
        
        UpdateVolume(default);
        GameManager.Instance.OnGameStateChanged += UpdateVolume;
        // UIManager.Instance.OnApplySettings.AddListener(UpdateVolume);
        UpdateVolume();
    }

    /// <summary>
    /// Use this for sounds that may be repeated very quickly Ex: a bunch of towers shooting
    /// </summary>
    /// <param name="sound"></param><param name="worldPosition"></param>
    public void PlaySound(Sounds sound, Transform worldPosition = null)
    {
        if (soundAudioClipsQueue == null)
        {
            soundAudioClipsQueue = new Queue<UnityEngine.GameObject>(maxAudioSources);
        }

        Debug.Log("Number of Audio Sources: " + soundAudioClipsQueue.Count);

        UnityEngine.GameObject soundGameObject;
        AudioSource audioSource;
        //Create Audio Source Game Object
        if (soundAudioClipsQueue.Count < maxAudioSources)
        {
            soundGameObject = new GameObject("Sound");
            
            if (worldPosition != null)
            {
                Debug.Log("Moving Sound to : "  + worldPosition);
                soundGameObject.transform.position = worldPosition.position;
                soundGameObject.transform.position = worldPosition.position;
            }
            
            soundAudioClipsQueue.Enqueue(soundGameObject);
            audioSource = soundGameObject.AddComponent<AudioSource>();
            
        }
        else
        {
            soundGameObject = soundAudioClipsQueue.Dequeue();
            soundAudioClipsQueue.Enqueue(soundGameObject);
            audioSource = soundGameObject.GetComponent<AudioSource>();
            audioSource.Stop();
        }
        SoundAudioClip audioClip = GetAudioClip(sound);
        audioSource.outputAudioMixerGroup = masterMixerGroup;
        audioSource.clip = audioClip.audioClip;
        audioSource.volume = audioClip.volume;
        audioSource.spatialBlend = audioClip.spacialBlend;
        audioSource.minDistance = audioClip.minFalloff;
        audioSource.maxDistance = audioClip.maxFalloff;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.PlayOneShot(audioSource.clip);
    }

    /// <summary>
    /// Use this for sound effects where you want to hear the entire sound effect ex: Lightning
    /// WARNING: using this with sound effects that are repeated often may result in broken audio
    /// </summary>
    /// <param name="_sound"></param>

    public void PlayEntireSound(Sounds _sound)
    {
        //Create Audio Source Game Object
        UnityEngine.GameObject soundGameObject = new UnityEngine.GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(_sound).audioClip;
        audioSource.volume = GetAudioClip(_sound).volume;
        audioSource.priority = 50;
        audioSource.PlayOneShot(audioSource.clip);
        Destroy(soundGameObject, audioSource.clip.length);
    }

    /// <summary>
    /// Changes the currently playing song
    /// </summary>
    /// <param name="_sound"></param>
    /// <returns></returns>
    public void PlaySong(Songs _song)
    {
        if (musicGameObject == null)
        {
            musicGameObject = new UnityEngine.GameObject("Music");
            musicGameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(musicGameObject);
        }
        
        AudioSource audioSource = musicGameObject.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = masterMixerGroup;
        
        audioSource.Stop();
        audioSource.clip = GetAudioClip(_song).audioClip;
        audioSource.volume = GetAudioClip(_song).volume;
        audioSource.priority = 10;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopSong()
    {
        if (musicGameObject == null) return;
        AudioSource audioSource = musicGameObject.GetComponent<AudioSource>();
        audioSource.Stop();
    }

    private SoundAudioClip GetAudioClip(Sounds _sound)
    {
        foreach (SoundAudioClip soundAudioClip in soundAudioClipsArray)
        {
            if (soundAudioClip.sound == _sound)
            {
                return soundAudioClip;
            }
        }

        Debug.LogError("Sound " + soundAudioClipsArray + "not found!");
        return null;
    }

    private SongAudioClip GetAudioClip(Songs _song)
    {
        foreach (SongAudioClip songAudioClip in musicAudioClipsArray)
        {
            if (songAudioClip.song == _song)
            {
                return songAudioClip;
            }
        }

        Debug.LogError("Song " + musicAudioClipsArray + "not found!");
        return null;
    }
    
    private void UpdateVolume(GameState state)
    {
        
        // Get the saved volume (likely 0–100 or 0–1)
        int savedVolume = SaveManager.SettingsStore.GetInt("MasterVolume", 50);

        // Convert 0–100 into 0–1
        float normalized = savedVolume / 100f;

        // Convert 0–1 linear volume into decibels
        float volumeDB = Mathf.Log10(Mathf.Max(normalized, 0.0001f)) * 20f;

        Debug.Log("Updating Volume: " + SaveManager.SettingsStore.GetInt("MasterVolume", 50) + "Converted to: " + volumeDB);
        
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", volumeDB);
        
    }
    
    private void UpdateVolume()
    {
        
        // Get the saved volume (likely 0–100 or 0–1)
        int savedVolume = SaveManager.SettingsStore.GetInt("MasterVolume", 50);

        // Convert 0–100 into 0–1
        float normalized = savedVolume / 100f;

        // Convert 0–1 linear volume into decibels
        float volumeDB = Mathf.Log10(Mathf.Max(normalized, 0.0001f)) * 20f;

        Debug.Log("Updating Volume: " + SaveManager.SettingsStore.GetInt("MasterVolume", 50) + "Converted to: " + volumeDB);
        
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", volumeDB);
        
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged -= UpdateVolume;
        
        // UIManager.Instance.OnApplySettings.RemoveListener(UpdateVolume);
    }

    [Serializable]
    public class SoundAudioClip
    {
        public Sounds sound;
        public AudioClip audioClip;

        [SerializeField, Range(0f, 1f)]
        public float volume = .5f;
        [FormerlySerializedAs("spacialAttenuation")] public float spacialBlend = 0f;
        public float minFalloff = 0f;
        public float maxFalloff = 100f;
    }

    [Serializable]
    public class SongAudioClip
    {
        public Songs song;
        public AudioClip audioClip;

        [SerializeField, Range(0f, 1f)]
        public float volume = .5f;
    }


}
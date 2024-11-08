using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System;
using PrimeTween;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<AudioManager>();

                if (_instance == null)
                {
                    GameObject singleton = new(typeof(AudioManager).ToString());
                    _instance = singleton.AddComponent<AudioManager>();
                    _instance.AddComponent<AudioSource>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void AddComponent<T>()
    {
        _instance.AddComponent<AudioSource>();
    }

    private AudioSource _singletonAudioSource;
    AudioSource _backgroundAudioSource;
    AudioSource _pauseMenuAudioSource;

    public static string MusicGroup = "Music";
    public static string SfxGroup = "SFX";

    // parameter suffix
    const string k_Parameter = "Volume";
    private static float s_LastSFXPlayTime = -1f;
    private static float sfxCooldown = 0.1f; // Global cooldown for playing sound effects

    [SerializeField] AudioMixer m_MainAudioMixer;
    // basic range of UI sound clips
    [Header("UI Sounds")]
    [Tooltip("General button click.")]
    [SerializeField] AudioClip _defaultButtonSound;
    [Tooltip("General button click.")]
    [SerializeField] AudioClip _ButtonHoverSound;
    [Tooltip("General error sound.")]

    [SerializeField] AudioClip[] _backgroundMusic;
    [SerializeField] AudioClip[] _PauseMenuMusic;

    private void OnEnable()
    {
        //SettingsEvents.SettingsUpdated += OnSettingsUpdated;
    }

    private void OnDisable()
    {
        //SettingsEvents.SettingsUpdated -= OnSettingsUpdated;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _singletonAudioSource = GetComponent<AudioSource>();
        CreateBackgroundAudioSource();
        CreatePauseMenuAudioSource();
    }
    void CreateBackgroundAudioSource()
    {
        _backgroundAudioSource = new GameObject("_BackgroundAudioSource").AddComponent<AudioSource>();
        _backgroundAudioSource.volume = 0.0f;
        _backgroundAudioSource.loop = true;
        _backgroundAudioSource.clip = _backgroundMusic[0];
        _backgroundAudioSource.spatialBlend = 0;
        _backgroundAudioSource.Play();
        Tween.AudioVolume(_backgroundAudioSource, endValue: 0.05f, duration: 14f);
        DontDestroyOnLoad(_backgroundAudioSource);
    }

        void CreatePauseMenuAudioSource()
    {
        _pauseMenuAudioSource = new GameObject("_PauseMenuAudioSource").AddComponent<AudioSource>();
        _pauseMenuAudioSource.volume = 0.0f;
        _pauseMenuAudioSource.loop = true;
        _pauseMenuAudioSource.clip = _backgroundMusic[0];
        _pauseMenuAudioSource.spatialBlend = 0;
        _pauseMenuAudioSource.Play();
        Tween.AudioVolume(_pauseMenuAudioSource, endValue: 0.05f, duration: 14f);
        DontDestroyOnLoad(_pauseMenuAudioSource);
    }

    public void FadeOutBackgroundMusic() {
        Tween.AudioVolume(_backgroundAudioSource, endValue: 0f, duration: 1.5f);
    }

    public void EnablePauseMusic() {
        Sequence.Create().Group(Tween.AudioVolume(_backgroundAudioSource, endValue: 0f, duration: 1f)
                         .Group(Tween.AudioVolume(_pauseMenuAudioSource, endValue: 0f, duration: 1f)));
    }

    public void PlayDefaultButtonSound()
    {
        if (_singletonAudioSource == null)
        {
            Debug.Log("AudioManager has no AudioSource :(");
            return;
        }
        _singletonAudioSource.clip = _defaultButtonSound;
        _singletonAudioSource.Play();
    }

     public void PlayAltButtonSound()
    {
        if (_singletonAudioSource == null)
        {
            Debug.Log("AudioManager has no AudioSource :(");
            return;
        }
        _singletonAudioSource.clip = _ButtonHoverSound;
        _singletonAudioSource.Play();
    }
    
    public void SetVolume(float targetVolume)
    {
        if (_singletonAudioSource == null)
            return;

        _singletonAudioSource.volume = targetVolume;
    }

/*
    void OnSettingsUpdated(S_GameSettings gameData)
    {
        // use the gameData to set the music and sfx volume
        SetVolume(gameData.AudioVolume);
    }
    */
}


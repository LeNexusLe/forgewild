using System.Collections;
using UnityEngine;
using WorldTime;

public class BackgroundAudioManager : MonoBehaviour
{
    [Header("Ambient")]
    [SerializeField] private AudioClip ambientClip;
    [SerializeField, Range(0f, 1f)] private float ambientVolume = 0.5f;

    [Header("Day / Night Music")]
    [SerializeField] private AudioClip dayMusic;
    [SerializeField] private AudioClip nightMusic;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.5f;
    [SerializeField] private float fadeDuration = 2f;

    [Header("World Time Reference")]
    [SerializeField] private WorldTime.WorldTime worldTime;

    private AudioSource ambientSource;
    private AudioSource activeMusicSource;
    private AudioSource inactiveMusicSource;

    private void Awake()
    {
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.clip = ambientClip;
        ambientSource.loop = true;
        ambientSource.volume = 0f;
        ambientSource.Play();
        StartCoroutine(FadeIn(ambientSource, ambientVolume));

        activeMusicSource = gameObject.AddComponent<AudioSource>();
        inactiveMusicSource = gameObject.AddComponent<AudioSource>();

        activeMusicSource.loop = true;
        inactiveMusicSource.loop = true;

        activeMusicSource.volume = 0f;
        inactiveMusicSource.volume = 0f;

        activeMusicSource.clip = dayMusic;
        activeMusicSource.Play();
        StartCoroutine(FadeIn(activeMusicSource, musicVolume));
    }

    private void OnEnable()
    {
        if (worldTime != null)
        {
            worldTime.DayChanged += OnDayStarted;
            worldTime.NightStarted += OnNightStarted;
        }
    }

    private void OnDisable()
    {
        if (worldTime != null)
        {
            worldTime.DayChanged -= OnDayStarted;
            worldTime.NightStarted -= OnNightStarted;
        }
    }

    private void OnDayStarted()
    {
        StartCoroutine(SwitchMusic(dayMusic));
    }

    private void OnNightStarted()
    {
        StartCoroutine(SwitchMusic(nightMusic));
    }

    private IEnumerator SwitchMusic(AudioClip newClip)
    {
        inactiveMusicSource.clip = newClip;
        inactiveMusicSource.Play();

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            activeMusicSource.volume = Mathf.Lerp(musicVolume, 0f, t);
            inactiveMusicSource.volume = Mathf.Lerp(0f, musicVolume, t);

            yield return null;
        }

        activeMusicSource.volume = 0f;
        inactiveMusicSource.volume = musicVolume;

        AudioSource temp = activeMusicSource;
        activeMusicSource = inactiveMusicSource;
        inactiveMusicSource = temp;

        inactiveMusicSource.Stop();
    }

    private IEnumerator FadeIn(AudioSource source, float targetVolume)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    public void SetAmbientVolume(float volume)
    {
        ambientSource.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        activeMusicSource.volume = volume;
    }
}

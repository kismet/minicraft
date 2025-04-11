using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
        [Range(0f, 1f)] public float volume = 1f;
        public float fadeDuration = 1f;
    }

    public SceneMusic[] sceneMusics;
    public AudioSource audioSource;

    private string currentScene;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        
        audioSource.loop = true;
    }

    void Start()
    {
        CheckSceneAndPlayMusic();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneAndPlayMusic();
    }

    void CheckSceneAndPlayMusic()
    {
        string newScene = SceneManager.GetActiveScene().name;

        if (newScene != currentScene)
        {
            currentScene = newScene;
            SceneMusic sceneMusicConfig = GetSceneMusicConfig(currentScene);

            if (sceneMusicConfig != null && sceneMusicConfig.musicClip != null)
            {
                if (audioSource.clip != sceneMusicConfig.musicClip || !audioSource.isPlaying)
                {
                    PlayMusic(sceneMusicConfig.musicClip, sceneMusicConfig.volume, sceneMusicConfig.fadeDuration);
                }
                else
                {
                    // Stessa musica ma potrebbe cambiare volume
                    SetVolume(sceneMusicConfig.volume, sceneMusicConfig.fadeDuration);
                }
            }
            else
            {
                // Nessuna musica per questa scena
                StopMusic(sceneMusics.Length > 0 ? sceneMusics[0].fadeDuration : 1f);
            }
        }
    }

    SceneMusic GetSceneMusicConfig(string sceneName)
    {
        foreach (SceneMusic sceneMusic in sceneMusics)
        {
            if (sceneMusic.sceneName == sceneName)
            {
                return sceneMusic;
            }
        }
        return null;
    }

    public void PlayMusic(AudioClip clip, float targetVolume = 1f, float fadeDuration = 1f)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeMusic(clip, targetVolume, fadeDuration));
    }

    public void StopMusic(float fadeDuration = 1f)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeOut(fadeDuration));
    }

    public void SetVolume(float targetVolume, float fadeDuration = 1f)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeVolume(targetVolume, fadeDuration));
    }

    private IEnumerator FadeMusic(AudioClip newClip, float targetVolume, float fadeDuration)
    {
        // Fade out della musica corrente se presente
        if (audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0f;
        }

        // Cambia clip e fade in
        audioSource.clip = newClip;
        audioSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
        fadeCoroutine = null;
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        fadeCoroutine = null;
    }

    private IEnumerator FadeVolume(float targetVolume, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
        fadeCoroutine = null;
    }
}
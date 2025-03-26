using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        // Se viene premuto il tasto "P", riproduce l'audio
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayAudio();
        }
    }

    public void PlayAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}

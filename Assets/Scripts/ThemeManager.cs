using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        // Imposta l'audio in loop all'avvio
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.Play(); // Avvia la riproduzione automaticamente
        }
    }
}
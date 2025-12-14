using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    public AudioClip clickSound;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
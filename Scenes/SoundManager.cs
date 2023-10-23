using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static bool isSoundOn = true;
    public AudioClip audioClip;
    private void Start()
    {

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        if (isSoundOn)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }

    }

    public void ToggleSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        isSoundOn = !isSoundOn;
        if (isSoundOn)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}

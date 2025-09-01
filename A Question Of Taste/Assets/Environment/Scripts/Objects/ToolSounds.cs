using UnityEngine;

public class ToolSounds : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] audios;

    public bool canMakeSounds { get; private set; } = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public void PlayAudio(int audioCode)
    {
        audioSource.PlayOneShot(audios[audioCode]);
    }

    public void PlayAudio(int audioCode, bool inLoop)
    {
        audioSource.loop = inLoop;
        audioSource.clip = audios[audioCode];
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}

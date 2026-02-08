using UnityEngine;

public class ToolSounds : MonoBehaviour
{
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();  
    }

    public void PlayAudio(AudioClip audio)
    {
        _audioSource.PlayOneShot(audio);
    }
    public void PlayAudio(AudioClip audio, bool inLoop)
    {
        _audioSource.loop = inLoop;
        _audioSource.clip = audio;
        _audioSource.Play();
    }

    public void StopAudio()
    {
        _audioSource.Stop();
    }
}

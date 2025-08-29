using UnityEngine;

public class ToolSounds : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] audioStage;
    // 2 mixing
    // 1 slapping
    // 0 shaping
    [SerializeField] private AudioClip putSound;

    public bool canMakeSounds { get; private set; } = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    public void MakingSounds(bool sounds)
    {
        canMakeSounds = sounds;
        if (canMakeSounds) 
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void SetSoundStage(int stage)
    {
        audioSource.loop = true;
        audioSource.clip = audioStage[stage];

        if (canMakeSounds) 
        {
            audioSource.Play();
        }
    }

    public void PutSound()
    {
        audioSource.loop = false;
        audioSource.clip = putSound;
        audioSource.Play();
    }
}

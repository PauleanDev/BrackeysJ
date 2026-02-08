using Unity.VisualScripting;
using UnityEngine;

public class OvenAnim : MonoBehaviour
{
    private Animator anim;
    private Oven ovenScript;
    private AudioSource audioSource;

    [SerializeField] private AnimationClip openOven;
    [SerializeField] private AnimationClip closeOven;
    [SerializeField] private AnimationClip doneOven;

    [SerializeField] private AudioClip cookingSound;
    [SerializeField] private AudioClip doneSound;

    private void Awake()
    {
        ovenScript = GetComponent<Oven>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckState();
        CheckAudio();
    }

    public void PlayOpenAnim(bool onOven)
    {
        if (onOven)
        {

            anim.Rebind();
            anim.Play(closeOven.name);
        }
        else
        {

            anim.Rebind();
            anim.Play(openOven.name);
        }
    }

    public void PlayDoneAnim(bool burned)
    {
        if (burned)
        {
            Debug.Log("Burning Animation");

            anim.Rebind();
            anim.SetBool("Burning", true);
        }
        else
        {
            anim.Rebind();
            anim.Play(doneOven.name);
            PlayAudio(doneSound);
        }
    }

    private void CheckState()
    {
        if (ovenScript._onOven)
        {
            anim.SetBool("Cooking", true);
        }
        else
        {
            anim.SetBool("Cooking", false);
        }
    }

    private void PlayAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public void StopAudio()
    {
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.loop = false;
    }

    private void CheckAudio()
    {
        if (audioSource.clip == null && ovenScript._onOven)
        {
            audioSource.loop = true;
            audioSource.clip = cookingSound;
            audioSource.Play();
        }
    }
}

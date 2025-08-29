using UnityEngine;
using UnityEngine.AI;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (agent.remainingDistance != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }

    public void PlayPlayerAudio(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
    }



}

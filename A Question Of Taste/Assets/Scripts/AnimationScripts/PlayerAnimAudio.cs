using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerPCMovement))]
public class PlayerAnimAudio : MonoBehaviour
{
    // Setup
    private Animator _anim;
    private AudioSource _audioSource;
    private NavMeshAgent _agent;

    // State
    private bool _isBaking = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_agent.remainingDistance != 0)
        {
            _anim.SetBool("Walking", true);
        }
        else
        {
            _anim.SetBool("Walking", false);
        }
    }

    public void PlayPlayerAudio(AudioClip audio)
    {
        _audioSource.clip = audio;
        _audioSource.Play();
    }

    public void PlayBakeAnim()
    {
        _isBaking = !_isBaking;

        _anim.SetBool("Working", _isBaking);
    }
}

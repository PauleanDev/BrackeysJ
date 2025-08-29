using UnityEngine;
using UnityEngine.AI;

public class ClientAnimation : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator animator;

    private AnimationClip idle;
    private AnimationClip walk;

    private bool walking = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (agent.isStopped && walk)
        {

            animator.Play(idle.name);
        }
        else if (!agent.isStopped && !walk)
        {
            animator.Play(walk.name);
        }
    }

}

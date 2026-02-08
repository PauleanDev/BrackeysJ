using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerPCMovement : PointClick
{
    // Navigation
    private NavMeshAgent navAgent;

    // Player state
    private bool rightDir = true;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        navAgent.ResetPath();
    }

    void Update()
    {
        Flip();
    }

    public override void OnClick(InputAction.CallbackContext context)
    {
        if (context.started && this.enabled)
        {
            base.OnClick(context);

            navAgent.destination = currentTarget;
        }
    }

    private void Flip()
    {
        if (navAgent.velocity.x > 0 && !rightDir || navAgent.velocity.x < 0 && rightDir)
        {
            rightDir = !rightDir;
            transform.Rotate(0, 180, 0);
        }
    }
}

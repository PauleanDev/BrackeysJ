using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class Client : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector2 ExitPosition;
    private bool rightDir = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ExitPosition = transform.position;
    }

    private void Update()
    {
        Flip();
    }

    public void Setup(Vector3 chairPosition)
    {
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.destination = chairPosition;
    }

    private void Flip()
    {
        if (agent.velocity.x > 0 && !rightDir || agent.velocity.x < 0 && rightDir)
        {
            rightDir = !rightDir;
            transform.Rotate(0, 180, 0);
        }
    }

    public void Leave()
    {
        agent.destination = ExitPosition;
        StartCoroutine("Disappear");
    }

    private IEnumerator Disappear()
    {
        while (agent.remainingDistance >= 0)
        {
            yield return null;
        }

        Destroy(this.gameObject);
    }
}

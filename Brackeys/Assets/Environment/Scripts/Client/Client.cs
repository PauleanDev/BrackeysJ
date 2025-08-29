using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class Client : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector2 ExitPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ExitPosition = transform.position;
    }

    public void Setup(Vector3 chairPosition)
    {
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.destination = chairPosition;
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
        Destroy(gameObject);
    }
}

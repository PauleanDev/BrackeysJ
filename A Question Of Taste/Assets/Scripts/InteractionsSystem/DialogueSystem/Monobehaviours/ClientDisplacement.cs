using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class ClientDisplacement : MonoBehaviour
{
    // Setup
    NavMeshAgent _agent;
    Vector2 _ExitPosition;
    Chair _chair;

    // State
    bool _rightDir = true;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _ExitPosition = transform.position;
    }

    private void Update()
    {
        Flip();
    }

    public void Setup(Chair chair)
    {
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;

        chair.Empty = false;
        _chair = chair;

        _agent.destination = chair.transform.position;
    }

    private void Flip()
    {
        if (_agent.velocity.x > 0 && !_rightDir || _agent.velocity.x < 0 && _rightDir)
        {
            _rightDir = !_rightDir;
            transform.Rotate(0, 180, 0);
        }
    }

    public IEnumerator Leave()
    {
        _chair.Empty = true;
        _agent.SetDestination(_ExitPosition);

        yield return new WaitUntil(() => _agent.remainingDistance != 0);
        yield return new WaitUntil(() => _agent.remainingDistance == 0);

        Destroy(gameObject);
    }
}

using System;
using UnityEngine;

public class InteractableTool : MonoBehaviour, IInteractable
{
    [Header("Interactable")]
    [SerializeField] Transform _playerTarget;
    protected PlayerPcInteract _playerScript;

    // Events
    public virtual event Action PlayerGetIt;

    protected virtual void Awake()
    {
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPcInteract>();
    }

    public virtual void Interact(){}

    public Vector2 GetInteractionPosition()
    {
        return _playerTarget.position;
    }
}

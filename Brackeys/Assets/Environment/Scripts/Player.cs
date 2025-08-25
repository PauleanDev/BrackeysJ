using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Interaction
    [SerializeField] private LayerMask interactableObject;
    private IInteractable currentInteractable;

    // Input variables
    [SerializeField] private InputActionAsset inputActions;
    private InputAction clickAction;
    private InputAction mousePosition;

    // Navegation
    private NavMeshAgent navAgent;

    // Player state
    private bool interacting = false;

    private void Awake()
    {
        clickAction = inputActions.FindAction("MovementClick");
        mousePosition = inputActions.FindAction("MousePosition");

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
    }

    void Update()
    {
        if (clickAction.triggered)
        {
            StartCoroutine("DetectObject");
            Movement();
        }
    }

    private void Movement()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());

        navAgent.destination = mousePos;
    }

    private IEnumerator DetectObject()
    {
        currentInteractable = null;
        interacting = false;

        Ray camRay = Camera.main.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
        RaycastHit2D hitObject = Physics2D.Raycast(camRay.origin, camRay.direction, interactableObject);


        if (hitObject)
        {
            currentInteractable = hitObject.transform.gameObject.GetComponent<IInteractable>();

            while((hitObject.transform.position - transform.position).magnitude > 2)
            {
                yield return null;
            }
            currentInteractable.Interact();
        }
    }
}

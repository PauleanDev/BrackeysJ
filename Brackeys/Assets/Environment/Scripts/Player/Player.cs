using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Interaction
    [SerializeField] private LayerMask interactableObject;
    private IInteractable currentInteractable;
    private GameObject objectKeeped;

    // Input variables
    [SerializeField] private InputActionAsset inputActions;
    private InputAction clickAction;
    private InputAction mousePosition;

    // Navegation
    private NavMeshAgent navAgent;

    // Player state
    public bool holdingObj { get; private set; }
    private bool rightDir = true;

    private bool canClick = true;

    private void Awake()
    {
        clickAction = inputActions.FindAction("MovementClick");
        mousePosition = inputActions.FindAction("MousePosition");

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;

        ClientInteraction.Called += OnCalled;
    }

    void Update()
    {
        if (clickAction.triggered && canClick)
        {
            StartCoroutine("DetectObject");
            Movement();
        }

        Flip();
    }

    private void Movement()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());

        navAgent.destination = mousePos;
    }

    private void Flip()
    {
        if (navAgent.velocity.x > 0 && !rightDir || navAgent.velocity.x < 0 && rightDir)
        {
            rightDir = !rightDir;
            transform.Rotate(0, 180, 0);
        }
    }

    private IEnumerator DetectObject()
    {
        currentInteractable = null;

        Ray camRay = Camera.main.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
        RaycastHit2D hitObject = Physics2D.Raycast(camRay.origin, camRay.direction, interactableObject);


        if (hitObject)
        {
            currentInteractable = hitObject.transform.gameObject.GetComponent<IInteractable>();

            while((hitObject.transform.position - transform.position).magnitude > 2.5f)
            {
                yield return null;
            }

            currentInteractable.Interact();
        }
    }

    public void HoldObject(GameObject objectHold)
    {
        holdingObj = true;
        objectKeeped = objectHold;
    }

    public GameObject DropObject(bool drop)
    {
        GameObject objectHold = objectKeeped;
        if (drop)
        {
            holdingObj = false;
            objectKeeped = null;
        }
        return objectHold;
    }

    private void OnCalled(ClientInteraction client)
    {
        if (client.waiting)
        {
            canClick = true;
        }
        else
        {
            canClick = false;
        }
    }
}

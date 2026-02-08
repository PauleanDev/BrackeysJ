using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerPcInteract : PointClick
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance;

    // Holding object
    private GameObject objectKeeped;

    public override void OnClick(InputAction.CallbackContext context)
    {
        if (context.started && this.enabled)
        {
            base.OnClick(context);

            if (currentInteractable != null)
            {
                StartCoroutine(Interact());
            }
        }
    }

    private IEnumerator Interact()
    {
        while ((currentTarget - (Vector2)transform.position).magnitude > interactionDistance)
        {
            yield return null;
        }

        currentInteractable.Interact();
    }

    public void HoldObject(GameObject objectHold)
    {
        objectKeeped = objectHold;
    }

    public void HoldObject(GameObject objectHold, SFood thatFood)
    {
        objectKeeped = objectHold;
    }

    public GameObject DropObject(bool drop)
    {
        GameObject objectHold = objectKeeped;

        if (drop)
        {
            objectKeeped = null;
        }
        return objectHold;
    }

    public GameObject GetObjectKeeped()
    {
        return objectKeeped;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PointClick : MonoBehaviour
{
    [SerializeField] private LayerMask interactableObject;

    protected Vector2 currentTarget;
    protected IInteractable currentInteractable;

    public virtual void OnClick(InputAction.CallbackContext context)
    {
        Ray camRay = Camera.main.ScreenPointToRay(context.ReadValue<Vector2>());
        RaycastHit2D hitObject = Physics2D.Raycast(camRay.origin, camRay.direction, interactableObject);

        if (hitObject.transform != null)
        {
            if (hitObject.transform.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                currentTarget = interactable.GetInteractionPosition();
                currentInteractable = interactable;
            }
        }
        else
        {
            currentTarget = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            currentInteractable = null;
        }
    }
}

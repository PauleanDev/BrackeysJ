using UnityEngine;

public class TrashBin : MonoBehaviour, IInteractable
{
    private GameObject foodObj;

    public void Interact()
    {
        Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (playerScript.objectKeeped.TryGetComponent<HoldableTray>(out HoldableTray tray))
        {
            tray.ClearTray();
        }
        else
        {
            foodObj = playerScript.DropObject(true);
            Destroy(foodObj);
        }
    }
}

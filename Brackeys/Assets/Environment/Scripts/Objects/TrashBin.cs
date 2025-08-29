using UnityEngine;

public class TrashBin : MonoBehaviour, IInteractable
{
    private GameObject foodObj;

    public void Interact()
    {
        Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        foodObj = playerScript.DropObject(true);

        Destroy(foodObj);
    }
}

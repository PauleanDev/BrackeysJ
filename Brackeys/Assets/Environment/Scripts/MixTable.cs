using UnityEngine;

public class MixTable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject foodObj;
    private bool onTable = false;

    public void Interact()
    {
        Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (!onTable)
        {
            foodObj = playerScript.DropObject();
            foodObj.transform.position = transform.position;
        }
        else
        {
            playerScript.HoldObject(foodObj);
        }

        HoldableFood holdableFood = foodObj.GetComponent<HoldableFood>();
        holdableFood.HoldDrop();

        onTable = !onTable;
    }
}

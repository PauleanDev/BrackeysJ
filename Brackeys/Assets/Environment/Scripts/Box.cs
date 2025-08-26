using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject ingredient;
    [SerializeField] private Food food;
    [SerializeField] private Transform ingredientParent;

    public void Interact()
    {
        Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (!playerScript.holdingObj)
        {
            GameObject ingredientObj = Instantiate(ingredient, transform.position, Quaternion.identity, ingredientParent);
            HoldableFood holdableObject = ingredientObj.GetComponent<HoldableFood>();
            holdableObject.SetFood(food);
            holdableObject.setPlayerParent();
        }
    }
}

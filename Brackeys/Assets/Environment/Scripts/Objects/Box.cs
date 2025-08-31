using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    private BoxAnim boxAnim;

    [SerializeField] private GameObject ingredient;
    [SerializeField] private Food food;
    [SerializeField] private Transform ingredientParent;

    [SerializeField] bool packagingBox = false;

    private void Awake()
    {
        boxAnim = GetComponent<BoxAnim>();
    }

    public void Interact()
    {
        Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (!playerScript.holdingObj)
        {
            GameObject ingredientObj = Instantiate(ingredient, transform.position, Quaternion.identity, ingredientParent);

            HoldableFood holdableObject = ingredientObj.GetComponent<HoldableFood>();
            holdableObject.SetFood(food);
            holdableObject.setPlayerParent();

            if (boxAnim != null)
            {
                boxAnim.PlayOpenAnim();
            }

            if (packagingBox)
            {
                holdableObject.FoodCondition = 2;
            }
        }
    }

    public bool IsPackagingBox()
    {
        return packagingBox;
    }
}

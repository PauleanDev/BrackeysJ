using UnityEngine;

public class HoldableFood : MonoBehaviour
{
    private SpriteRenderer thisSprite;

    private Food food;

    [SerializeField] private Transform linkPlayerPos;

    private int foodStage = 0;
    private bool holding = false;

    private void Awake()
    {
        thisSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (holding)
        {
            transform.position = Vector2.Lerp(transform.position, linkPlayerPos.position, 0.2f);
        }
    }

    public void setPlayerParent()
    {
        linkPlayerPos = GameObject.FindGameObjectWithTag("ObjectPosition").transform;
        GameObject playerPos = GameObject.FindGameObjectWithTag("Player");

        Player playerScript = playerPos.GetComponent<Player>();
        playerScript.HoldObject(gameObject);

        holding = true;
    }

    public void SetFood(Food thatFood)
    {
        food = thatFood;

        thisSprite.sprite = food.sprite;
    }

    public void HoldDrop()
    {
        holding = !holding;
    }

    public int FoodCondition
    {
        get 
        {
            return foodStage;
        }
        set 
        {
            foodStage = value; 
        }
    }
}

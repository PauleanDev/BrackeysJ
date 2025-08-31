using UnityEngine;

public class HoldableFood : MonoBehaviour
{
    protected SpriteRenderer thisSprite;

    public Food food { get; private set; }
    public Tastes taste { get; private set; } = new Tastes();  

    private Transform linkPlayerPos;

    protected int foodStage = 0;
    private bool holding = false;

    protected virtual void Awake()
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

    public virtual void SetFood(Food thatFood)
    {
        thisSprite.sprite = thatFood.sprite;
        taste = thatFood.taste;
        food = thatFood;
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

using UnityEngine;

public class HoldableFood : MonoBehaviour
{
    [Header("Setup")]
    public SFood currentFood { get; protected set; }
    protected SpriteRenderer _thisSpriteR;
    Transform _linkPlayerPos;

    // States
    bool _held = false;

    protected virtual void Awake()
    {
        _thisSpriteR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_held)
        {
            transform.position = Vector2.Lerp(transform.position, _linkPlayerPos.position, 0.2f);
        }
    }

    public void setPlayerParent()
    {
        _linkPlayerPos = GameObject.FindGameObjectWithTag("ObjectPosition").transform;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        PlayerPcInteract playerScript = playerObj.GetComponent<PlayerPcInteract>();
        playerScript.HoldObject(gameObject);

        _held = true;
    }

    public virtual void SetFood(SFood thatFood)
    {
        _thisSpriteR.sprite = thatFood.sprite;

        currentFood = Instantiate(thatFood);
    }

    public void HoldDrop()
    {
        _held = !_held;
    }

    public TFoodState FoodCondition
    {
        get 
        {
            return currentFood.foodState;
        }
        set 
        {
            currentFood.foodState = value; 
        }
    }
}

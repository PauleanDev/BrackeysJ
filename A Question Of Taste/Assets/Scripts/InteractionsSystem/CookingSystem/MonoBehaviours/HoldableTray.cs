using UnityEngine;

public class HoldableTray : HoldableFood
{
    [Header("Tray")]
    public Sprite emptyTray;
    public Sprite cookieTray;

    public void SetFood(TTaste taste)
    {
        currentFood = Instantiate(new SFood());
        currentFood.foodTaste = taste;

        _thisSpriteR.sprite = cookieTray;
    }

    public void ClearTray()
    {
        currentFood = null;

        _thisSpriteR.sprite = emptyTray;
    }
}

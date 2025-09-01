using UnityEngine;

public class HoldableTray : HoldableFood
{
    public Tastes cookieTaste { get; private set; }
    public Sprite emptyTray;
    public Sprite cookieTray;

    public void KeepTaste(Tastes taste)
    {
        cookieTaste = taste;
        thisSprite.sprite = cookieTray;
    }

    public void ClearTray()
    {
        cookieTaste = null;
        foodStage = 0;
        thisSprite.sprite = emptyTray;
    }
}

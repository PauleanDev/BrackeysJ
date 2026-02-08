using System;
using UnityEngine;

[Serializable]
public class TFood
{
    public TFoodState foodState;
    public TTaste foodTaste;
    public Sprite sprite;
}

public enum TTaste
{ 
    noTaste,
    sweet,
    spicy,
    salty
}

[Serializable]
public enum TFoodState
{
    ingredient,
    specialIngredient,
    raw,
    package,
    cooked,
    burnt
}
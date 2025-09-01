using UnityEngine;

[CreateAssetMenu(fileName = "FoodBank", menuName = "Scriptable Objects/FoodBank")]
public class FoodBank : ScriptableObject
{
    public Food[] foods;
}

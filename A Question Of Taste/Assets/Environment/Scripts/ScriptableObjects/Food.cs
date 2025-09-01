using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

[CreateAssetMenu(fileName = "Food", menuName = "Scriptable Objects/Food")]
public class Food : ScriptableObject
{
    public bool isIngredient;
    public int ingredientID;
    public Tastes taste;
    public Sprite sprite;
}

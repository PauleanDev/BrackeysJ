using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

[CreateAssetMenu(fileName = "Food", menuName = "Scriptable Objects/Cooking/Food")]
public class SFood : ScriptableObject
{
    public TFoodState foodState;
    public TTaste foodTaste;
    public Sprite sprite;
}

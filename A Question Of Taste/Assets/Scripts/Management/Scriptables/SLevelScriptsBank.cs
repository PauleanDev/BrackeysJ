using UnityEngine;

[CreateAssetMenu(fileName = "SLevelScriptsBank", menuName = "Scriptable Objects/Scenary/LevelScriptsBank")]
public class SLevelScriptsBank : ScriptableObject
{
    public SLevelScript[] levels;
}

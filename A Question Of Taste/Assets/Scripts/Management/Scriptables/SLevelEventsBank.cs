using UnityEngine;

[CreateAssetMenu(fileName = "SLevelEventsBank", menuName = "Scriptable Objects/Scenary/LevelEventsBank")]
public class SLevelEventsBank : ScriptableObject
{
    public SLevelEvent[] levelEvent;
    public GameObject candlePrefab;
}
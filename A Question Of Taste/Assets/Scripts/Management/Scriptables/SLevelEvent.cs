using UnityEngine;


[CreateAssetMenu(fileName = "SLevelEvent", menuName = "Scriptable Objects/Scenary/LevelEvent")]
public class SLevelEvent : ScriptableObject
{
    public int level;
    public bool finishOnAvaliated;
    public string[] pGetItMassages;
    public TLevelEvents levelEvents;
}

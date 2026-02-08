using UnityEngine;

[CreateAssetMenu(fileName = "SLevelScript", menuName = "Scriptable Objects/Scenary/LevelScript")]
public class SLevelScript : ScriptableObject
{
    public TObjectsSetup[] levelObject;
    public bool isDarkLevel;
    public bool clientsSpawnPerTime;
    public float gameTime;
    public int[] reviewsScore;
    public int[] starsScore;
    public AudioClip music;
}


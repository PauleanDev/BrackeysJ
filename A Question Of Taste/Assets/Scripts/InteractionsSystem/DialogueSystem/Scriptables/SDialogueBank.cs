using UnityEngine;

[CreateAssetMenu(fileName = "SDialogueBank", menuName = "Scriptable Objects/DataBank/DialogueBank")]
public class SDialogueBank : ScriptableObject
{
    public TClient clientData;
    public Questions[] questions;
    public Answers[] questionAnswers;
    public string[] generalNegativeAnswares;
}

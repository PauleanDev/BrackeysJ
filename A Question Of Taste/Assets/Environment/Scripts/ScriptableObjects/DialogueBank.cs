using UnityEngine;

[CreateAssetMenu(fileName = "DialogueBank", menuName = "Scriptable Objects/DialogueBank")]
public class DialogueBank : ScriptableObject
{
    public ClientData clientData;

    public Questions[] questions;
    public Answers[] questionAnswers;
    public string[] generalNegativeAnswares;

}

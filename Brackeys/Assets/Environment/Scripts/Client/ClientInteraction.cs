using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ClientInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueBank dialogueBank;
    private Client clientMove;

    public string clientName { get; private set; }
    public int dialogueLevel { get; private set; }
    public Tastes clientTaste { get; private set; } = new Tastes();
    public bool waiting { get; private set; } = true;
    public bool ordered { get; private set; } = false;
    public AudioClip clientVoice { get; private set; }
    private int male;
    
    public delegate void CalledHandler(ClientInteraction client);
    public static event CalledHandler Called;


    private void OnEnable()
    {
        clientMove = GetComponent<Client>();
        male = Random.Range(0, 1);
        clientName = dialogueBank.clientData.genreInfo[male].clientName[Random.Range(0, dialogueBank.clientData.genreInfo[male].clientName.Length - 1)] ;
        dialogueLevel = Random.Range(0, dialogueBank.clientData.dialogueChances.Length - 1);

        int tasteCode = Random.Range(0, 3);

        switch (tasteCode)
        {
            case 0:
                clientTaste.sweetness = 10;
                break;
            case 1:
                clientTaste.spicy = 10;
                break;
            case 2:
                clientTaste.salty = 10;
                break;
        }
    }

    private bool PositiveAnswer()
    {
        List<bool> chance = new List<bool>();

        for (int i = 0; i < dialogueBank.clientData.dialogueChances[dialogueLevel].positiveChance; i++) 
        {
            chance.Add(true);
        }
        for (int i = 0; i < dialogueBank.clientData.dialogueChances[dialogueLevel].negativeChance; i++) 
        {
            chance.Add(false);
        }

        bool positiveAnsware = chance[Random.Range(0, chance.Count)];

        return positiveAnsware;
    }

    public void Interact()
    {
        Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        if (playerScript.holdingObj && ordered)
        {
            clientMove.Leave();
        }
        else
        {
            ordered = true;
            waiting = !waiting;
            Called(this);
        }
    }

    public string Question(Tastes taste, int questionId)
    {
        if (PositiveAnswer())
        {
            bool tasteQuestion = dialogueBank.questionAnswers[questionId].tasteQ || dialogueBank.questionAnswers[questionId].ingredienQ;

            if (tasteQuestion)
            {
                if (taste.sweetness == clientTaste.sweetness || taste.spicy == clientTaste.spicy || taste.salty == clientTaste.salty)
                {
                    return dialogueBank.questionAnswers[questionId].tasteAnswers.answer[Random.Range(0, dialogueBank.questionAnswers[questionId].tasteAnswers.answer.Length - 1)];
                }
                else
                {
                    return dialogueBank.questionAnswers[questionId].tasteAnswers.wrongTasteAnswers[Random.Range(0, dialogueBank.questionAnswers[questionId].tasteAnswers.wrongTasteAnswers.Length - 1)];
                }
            }
            else
            {
                return dialogueBank.questionAnswers[questionId].answer[Random.Range(0, dialogueBank.questionAnswers[questionId].answer.Length-1)];
            }
        }
        else
        {
            return dialogueBank.generalNegativeAnswares[Random.Range(0, dialogueBank.generalNegativeAnswares.Length - 1)];
        }
    }
}

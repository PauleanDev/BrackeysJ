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
    private int tasteCode;
    public bool waiting { get; private set; } = true;
    public AudioClip clientVoice { get; private set; }
    private int male;
    
    public delegate void CalledHandler (ClientInteraction client);
    public static event CalledHandler Called;
    
    public delegate void AvaliatedHandler (int rating);
    public static event AvaliatedHandler Avaliated;


    private void OnEnable()
    {
        clientMove = GetComponent<Client>();
        male = Random.Range(0, 1);
        clientName = dialogueBank.clientData.genreInfo[male].clientName[Random.Range(0, dialogueBank.clientData.genreInfo[male].clientName.Length - 1)] ;
        dialogueLevel = Random.Range(0, dialogueBank.clientData.dialogueChances.Length - 1);

        tasteCode = Random.Range(0, 3);


        switch (tasteCode)
        {
            case 0:
                clientTaste.sweetness = 100;
                break;
            case 1:
                clientTaste.spicy = 100;
                break;
            case 2:
                clientTaste.salty = 100;
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
        
        if (playerScript.holdingObj && playerScript.objectKeeped.TryGetComponent<HoldableFood>(out HoldableFood holdablefood))
        {
            Tastes atualTaste = playerScript.currentObjectTaste;

            if (holdablefood.FoodCondition == 3)
            {
                if (atualTaste.sweetness == 0 && atualTaste.spicy == 0 && atualTaste.salty == 0)
                {
                    Avaliated(2);
                }
                else if (atualTaste.sweetness == clientTaste.sweetness && atualTaste.spicy == clientTaste.spicy && atualTaste.salty == clientTaste.salty)
                {
                    Avaliated(3);
                }
                else
                {
                    Avaliated(1);
                }

                playerScript.DropObject(true);
                Destroy(holdablefood.gameObject);

                clientMove.Leave();
            }
            else if (holdablefood.FoodCondition == 4)
            {
                Avaliated(0);
                clientMove.Leave();
            }
        }
        else
        {
            waiting = !waiting;
            Called(this);
        }
    }

    public string Question(Tastes taste, int questionId)
    {
        Debug.Log(clientTaste.sweetness + " " + clientTaste.spicy + " " + clientTaste.salty);

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
                return dialogueBank.questionAnswers[questionId].foodAnswares[tasteCode].positiveAnswares[Random.Range(0, dialogueBank.questionAnswers[questionId].foodAnswares[tasteCode].positiveAnswares.Length - 1)];
            }
        }
        else
        {
            return dialogueBank.generalNegativeAnswares[Random.Range(0, dialogueBank.generalNegativeAnswares.Length - 1)];
        }
    }
}

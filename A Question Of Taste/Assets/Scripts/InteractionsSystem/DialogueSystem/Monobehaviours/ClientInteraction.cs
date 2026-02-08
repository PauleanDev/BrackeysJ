using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class ClientInteraction : MonoBehaviour, IInteractable
{
    [Header("Setup")]
    [SerializeField] SDialogueBank _dialogueBank;
    [SerializeField] Transform _target;
    SDialogueBank _currentDialogueBank;
    ClientDisplacement _move;
    ClientAnimation _animation;
    ClientData _clientData;

    [Header("DialogueData")]

    [Header("Modifiers")]
    [SerializeField] float _timeToLeave;
    float _currentTimeToLeave;

    // Dialogue
    Sprite _icon;
    Sprite _hair;
    bool[] _questionsAswered;

    public bool[] QuestionsAswered { get { return _questionsAswered; } set { _questionsAswered = new bool[value.Length];
            _questionsAswered = value;}
    }
    public Sprite Icon { get { return _icon; } set { _icon = value; } }
    public Sprite Hair { get { return _hair; } set { _hair = value; } }

    // Events
    public delegate void CalledHandler(ClientInteraction client);
    public static event CalledHandler Called;
    public delegate void AvaliatedHandler (int rating);
    public static event AvaliatedHandler Avaliated;

    private void OnEnable()
    {
        _currentDialogueBank = Instantiate(_dialogueBank);
        _animation = GetComponent<ClientAnimation>();
        _clientData = new ClientData();
        _move = GetComponent<ClientDisplacement>();
        _clientData.clientInfo = _currentDialogueBank.clientData.clientInfo[UnityEngine.Random.Range(0, _currentDialogueBank.clientData.clientInfo.Length)];
        _clientData.dialogueLevel = UnityEngine.Random.Range(0, _currentDialogueBank.clientData.dialogueChances.Length - 1);
        _clientData.clientTaste = (TTaste)UnityEngine.Random.Range(1, 4);
        _clientData.waiting = true;

        UnityEngine.Random.InitState(_clientData.clientInfo.clientName.Length + _clientData.dialogueLevel);

        int skinID = UnityEngine.Random.Range(0, _currentDialogueBank.clientData.skinAnimators.Length);

        Icon = _currentDialogueBank.clientData.skinIcons[skinID];
        _animation.SetupSkin(_currentDialogueBank.clientData.skinAnimators[skinID]);

        int HairID = UnityEngine.Random.Range(0, _currentDialogueBank.clientData.genreHairComponents[_clientData.clientInfo.isMale ? 1 : 0].bodyComponents.Length);

        Hair = _currentDialogueBank.clientData.genreHairComponents[_clientData.clientInfo.isMale ? 1 : 0].bodyComponents[HairID].sprites[0];
        _animation.SetupHair(_currentDialogueBank.clientData.genreHairComponents[_clientData.clientInfo.isMale ? 1 : 0].bodyComponents[HairID].sprites);
    }

    public ClientInfo GetClientInfo()
    {
        return _clientData.clientInfo;
    }

    public Vector2 GetInteractionPosition()
    {
        return _target.position;
    }

    public void Interact()
    {
        PlayerPcInteract playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPcInteract>();
        
        if (playerScript.GetObjectKeeped() && playerScript.GetObjectKeeped().TryGetComponent<HoldableFood>(out HoldableFood holdablefood))
        {
            if (holdablefood.FoodCondition == TFoodState.cooked)
            {
                if (_clientData.clientTaste == holdablefood.currentFood.foodTaste)
                {
                    Avaliated(3);
                }
                else if (holdablefood.currentFood.foodTaste == TTaste.noTaste)
                {
                    Avaliated(2);
                }
                else
                {
                    Avaliated(1);
                }

                playerScript.DropObject(true);
                Destroy(holdablefood.gameObject);

                StartCoroutine(_move.Leave());
            }
            else if (holdablefood.FoodCondition == TFoodState.burnt)
            {
                Avaliated(0);
                StartCoroutine(_move.Leave());
            }
        }
        else
        {
            _clientData.waiting = !_clientData.waiting;

            Called(this);
        }
    }

    public string Question(TTaste taste, int questionId)
    {
        if (PositiveAnswer())
        {
            bool tasteQuestion = _currentDialogueBank.questionAnswers[questionId].tasteQ || _currentDialogueBank.questionAnswers[questionId].ingredienQ;

            if (tasteQuestion)
            {
                if (taste == _clientData.clientTaste)
                {
                    return _currentDialogueBank.questionAnswers[questionId].tasteAnswers.answer[UnityEngine.Random.Range(0, _currentDialogueBank.questionAnswers[questionId].tasteAnswers.answer.Length - 1)];
                }
                else
                {
                    return _currentDialogueBank.questionAnswers[questionId].tasteAnswers.wrongTasteAnswers[UnityEngine.Random.Range(0, _currentDialogueBank.questionAnswers[questionId].tasteAnswers.wrongTasteAnswers.Length - 1)];
                }
            }
            else
            {
                return _currentDialogueBank.questionAnswers[questionId].foodAnswares[(int)_clientData.clientTaste - 1].positiveAnswares[UnityEngine.Random.Range(0, _currentDialogueBank.questionAnswers[questionId].foodAnswares[(int)_clientData.clientTaste - 1].positiveAnswares.Length - 1)];
            }
        }
        else
        {
            return _currentDialogueBank.generalNegativeAnswares[UnityEngine.Random.Range(0, _currentDialogueBank.generalNegativeAnswares.Length - 1)];
        }
    }

    private bool PositiveAnswer()
    {
        List<bool> chance = new List<bool>();

        for (int i = 0; i < _currentDialogueBank.clientData.dialogueChances[_clientData.dialogueLevel].positiveChance; i++) 
        {
            chance.Add(true);
        }
        for (int i = 0; i < _currentDialogueBank.clientData.dialogueChances[_clientData.dialogueLevel].negativeChance; i++) 
        {
            chance.Add(false);
        }

        bool positiveAnsware = chance[UnityEngine.Random.Range(0, chance.Count)];

        return positiveAnsware;
    }

    public IEnumerator GetBoring()
    {
        _currentTimeToLeave = _timeToLeave;

        while (_currentTimeToLeave > 0)
        {
            _currentTimeToLeave -= Time.deltaTime;
            yield return null;
        }

        _move.Leave();
    }
}

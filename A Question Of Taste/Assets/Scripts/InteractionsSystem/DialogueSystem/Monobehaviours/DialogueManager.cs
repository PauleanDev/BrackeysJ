using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Setup")]
    UIManagement _uIManager;

    [Header("Dialogue")]
    [SerializeField] SDialogueBank _dialogueBank;
    ClientInteraction _currentClient;
    TTaste _currentTaste;
    int _currentOption;
    int _currentQuestion;
    bool[] _canSelectQuestions;

    private void Awake()
    {
        _uIManager = GetComponent<UIManagement>();

        _canSelectQuestions = new bool[_dialogueBank.questions.Length];
    }

    public void SetClientInfo(ClientInteraction client)
    {
        if (_currentClient != null)
        {
            if (client != _currentClient)
            {
                _currentClient.QuestionsAswered = _canSelectQuestions;
                _uIManager.SetClientIcon(client.Icon, client.Hair);

                if (client.QuestionsAswered != null)
                {
                    _canSelectQuestions = _currentClient.QuestionsAswered;
                }
                else
                {
                    ResetSelection();
                }
                _currentClient = client;
                StartDialogue();
            }
            else
            {
                StartDialogue();
            }
        }
        else
        {
            _uIManager.SetClientIcon(client.Icon, client.Hair);
            ResetSelection();

            _currentClient = client;
            StartDialogue();
        }

        _currentClient = client;
    }

    private void ResetSelection()
    {
        for (int i = 0; i < _canSelectQuestions.Length; i++)
        {
            _canSelectQuestions[i] = true;
        }
    }

    public void ClientAnswer()
    {
        _currentOption = 0;
        _canSelectQuestions[_currentQuestion] = false;

        _uIManager.ClientAnswer(_currentClient.Question(_currentTaste, _currentQuestion));
    }

    public void StartDialogue()
    {
        _uIManager.StartDialogue(_currentClient.GetClientInfo());

        _currentOption = 0;
        _uIManager.EnableDialogueActor(true);

        _uIManager.SetInteractableButtons(_canSelectQuestions, _dialogueBank.questions);
    }

    public void SelectOption(int option)
    {
        switch (_currentOption)
        {
            case 0:
                _currentQuestion = option;

                if (_dialogueBank.questions[option].subMenu)
                {
                    if (_dialogueBank.questions[option].subMenu)
                    {
                        if (_dialogueBank.questions[_currentQuestion].ingredientQ)
                        {
                            _uIManager.SetSubInteractableButtons(_dialogueBank.questions[option].ingredientQuestions);
                            _uIManager.ResetButtons();
                        }
                        else if (_dialogueBank.questions[_currentQuestion].tasteQ)
                        {
                            _uIManager.SetSubInteractableButtons(_dialogueBank.questions[option].tasteQuestions);
                            _uIManager.ResetButtons();
                        }
                    }

                    _currentOption++;
                }
                else
                {
                    ClientAnswer();
                }
                break;

            case 1:
                if (_dialogueBank.questions[_currentQuestion].ingredientQ)
                {
                    _currentTaste = _dialogueBank.questions[_currentQuestion].ingredientQuestions[option].food.foodTaste;
                }
                else if (_dialogueBank.questions[_currentQuestion].tasteQ)
                {
                    _currentTaste = _dialogueBank.questions[_currentQuestion].tasteQuestions[option].clientTaste;
                }

                ClientAnswer();
                break;
        }
    }

    public void StopDialogue()
    {
        if (_currentClient != null)
        {
            _currentClient.Interact();
        }

        _uIManager.StopDialogue();
    }
}

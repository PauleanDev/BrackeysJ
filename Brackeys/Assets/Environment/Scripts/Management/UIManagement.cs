using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    [Header("Managers")]

    [Header("GameUI")]
    [SerializeField] private GameObject startGamePanel;
    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private Text timerText;
    [SerializeField] private Text scoreText;

    [Header("Dialogue")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] DialogueBank dialogueBank;
    private Tastes currentTaste;
    private int currentOption;
    private int currentQuestion;

    [Header("Player")]
    [SerializeField] private GameObject playerLayout;
    [SerializeField] private Button[] dialogButtons;
    [SerializeField] private Text[] buttonsText;
    private bool[] canSelect;

    [Header("Client")]
    [SerializeField] private GameObject clientLayout;
    [SerializeField] private Image clientIcon;
    [SerializeField] private Text clientName;
    [SerializeField] private Text clientText;
    private ClientInteraction atualClient;

    [Header("GameEnd")]
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private Image[] stars;
    [SerializeField] private int[] starsPontuation;
    [SerializeField] private Sprite starEmpty;
    [SerializeField] private Sprite starPerfect;


    private void Awake()
    {
        ClientInteraction.Called += OnCalled;
        GameManagement.GameFinished += OnGameFinished;

        canSelect = new bool[dialogButtons.Length];

        Invoke("AfterGameStart", GameManagement.startGame);
    }

    private void OnCalled(ClientInteraction client)
    {
        dialoguePanel.SetActive(!client.waiting);

        if (client != atualClient)
        {
            StartDialogue(false);

            atualClient = client;
        }
    }

    private void AfterGameStart()
    {
        startGamePanel.SetActive(false);
        gameUIPanel.SetActive(true);
    }

    private void OnGameFinished()
    {
        int pontuation = PlayerPrefs.GetInt("LastScore");

        if (pontuation < starsPontuation[0])
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].sprite = starEmpty;
            }
        }
        else if (pontuation >= starsPontuation[0] && pontuation < starsPontuation[1])
        {
            for (int i = 0; i < stars.Length - 1; i++)
            {
                stars[i].sprite = starEmpty;
            }
            for (int i = stars.Length - 1; i < stars.Length; i++)
            {
                stars[i].sprite = starPerfect;
            }
        }
        else if (pontuation >= starsPontuation[1] && pontuation < starsPontuation[2])
        {
            for (int i = 0; i < stars.Length - 2; i++)
            {
                stars[i].sprite = starEmpty;
            }
            for (int i = stars.Length - 2; i < stars.Length; i++)
            {
                stars[i].sprite = starPerfect;
            }
        }
        else if (pontuation >= starsPontuation[2])
        {
            for (int i = stars.Length; i < stars.Length; i++)
            {
                stars[i].sprite = starPerfect;
            }
        }

            gameUIPanel.SetActive(false);
        dialoguePanel.SetActive(false);

        gameEndPanel.SetActive(true);
    }

    public void TimerUpdate(float time)
    {
        timerText.text = ((int)(time / 60)).ToString("00") + ":" + (time % 60).ToString("00");
    }

    public void ScoreUpdate(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SelectOption(int option)
    {
        switch (currentOption)
        {
            case 0:
                currentQuestion = option;

                if (dialogueBank.questions[option].subMenu)
                {
                    if (dialogueBank.questions[currentQuestion].ingredientQ)
                    {
                        for (int i = 0; i < dialogueBank.questions[option].ingredientQuestions.Length; i++)
                        {
                            if (i < buttonsText.Length)
                            {
                                buttonsText[i].text = dialogueBank.questions[option].ingredientQuestions[i].food.name;
                            }
                        }
                        
                    }
                    else if (dialogueBank.questions[currentQuestion].tasteQ)
                    {
                        for (int i = 0; i < dialogueBank.questions[option].tasteQuestions.Length; i++)
                        {
                            if (i < buttonsText.Length)
                            {
                                buttonsText[i].text = dialogueBank.questions[option].tasteQuestions[i].question;
                            }
                        }
                    }

                    for (int i = 0; i < dialogButtons.Length; i++)
                    {
                        dialogButtons[i].interactable = true;
                    }

                    currentOption++;
                }
                else
                {
                    ClientAnswer();
                }
            break;

            case 1:
                if (dialogueBank.questions[currentQuestion].ingredientQ)
                {
                    currentTaste = dialogueBank.questions[currentQuestion].ingredientQuestions[option].food.taste;
                }
                else if (dialogueBank.questions[currentQuestion].tasteQ)
                {
                    currentTaste = dialogueBank.questions[currentQuestion].tasteQuestions[option].taste;
                }

                ClientAnswer();
            break;
        }
    }

    public void ClientAnswer()
    {
        playerLayout.SetActive(false);
        clientLayout.SetActive(true);
        clientName.text = atualClient.clientName;
        clientText.text = atualClient.Question(currentTaste, currentQuestion);

        canSelect[currentQuestion] = false;

        currentOption = 0;
    }

    public void AtualClientInteract()
    {
        atualClient.Interact();
    }

    public void StartDialogue(bool reset)
    {
        if (reset)
        {
            currentOption = 0;
            playerLayout.SetActive(true);
            clientLayout.SetActive(false);

            for (int i = 0; i < canSelect.Length; i++)
            {
                dialogButtons[i].interactable = canSelect[i];
            }
        }
        else
        {
            for (int i = 0; i < canSelect.Length; i++)
            {
                canSelect[i] = true;
            }
        }

        for (int i = 0; i < buttonsText.Length; i++)
        {
            buttonsText[i].text = dialogueBank.questions[i].question;
        }
    }

    public void PauseUI(bool isPaused)
    {
        PausePanel.SetActive(!isPaused);
    }
}

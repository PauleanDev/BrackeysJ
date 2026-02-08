using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    [Header("GameUI")]
    [SerializeField] private GameObject startGamePanel;
    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Text timerText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text[] punctuationsText;
    [SerializeField] UIComponentAnim _scorePanelAnim;

    [Header("Dialogue")]
    [SerializeField] private GameObject dialoguePanel;
    // Used in Level Events
    [SerializeField] private Text interactedText;
    private ClientInfo currentClient;
    private string[] currentInterectMessages;
    private int interactionsCount = 0;
    private Animator interactionAnim;
    private Sprite currentClientIcon;
    private Sprite currentClientHair;

    [Header("Player")]
    [SerializeField] private GameObject playerLayout;
    [SerializeField] private Button[] dialogueButtons;
    [SerializeField] private Text[] buttonsText;

    [Header("Client")]
    [SerializeField] private GameObject clientLayout;
    [SerializeField] private Image clientIcon;
    [SerializeField] private Image clientHairIcon;
    [SerializeField] private Text clientName;
    [SerializeField] private Text clientText;

    [Header("GameEnd")]
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private Image[] stars;
    [SerializeField] private Sprite starEmpty;
    [SerializeField] private Sprite starPerfect;
    private int[] starsPontuation;
    private int _currentLevel;

    private void Awake()
    {
        interactionAnim = interactedText.GetComponent<Animator>();
        _currentLevel = PlayerPrefs.GetInt("Level");

        Invoke("AfterGameStart", GameManager.startTime);
    }

    private void Start()
    {
        _scorePanelAnim.Invoke("AnimOut", GameManager.startTime);
    }

    public void LevelSetup(int[] punctuations)
    {
        starsPontuation = punctuations;

        for (int i = 0; i < punctuations.Length; i++)
        {
            punctuationsText[i].text = "Score: " + punctuations[i].ToString() + " - " + i.ToString() + " stars";
        }
    }

    private void AfterGameStart()
    {
        startGamePanel.SetActive(false);
        gameUIPanel.SetActive(true);
    }

    // Game Methods

    public void TimerUpdate(float time)
    {
        timerText.text = ((int)(time / 60)).ToString("00") + ":" + (time % 60).ToString("00");
    }

    public void ScoreUpdate(int score)
    {
        scoreText.text = score.ToString();
    }

    public void PauseUI(bool isPaused)
    {
        _pausePanel.SetActive(!isPaused);

        if (isPaused)
        {
            _scorePanelAnim.AnimOut();
        }
        else
        {
            _scorePanelAnim.AnimIn();
        }
    }
    
    public void FinishGameUI(int currentScore)
    {
        gameUIPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        gameEndPanel.SetActive(true);

        int starsScore = 0;

        if (currentScore <= starsPontuation[0])
        {
            starsScore = 0;
        }
        else if (currentScore >= starsPontuation[1] && currentScore < starsPontuation[2])
        {
            starsScore = 1;
        }
        else if (currentScore >= starsPontuation[2] && currentScore < starsPontuation[3])
        {
            starsScore = 2;
        }
        else if (currentScore >= starsPontuation[3])
        {
            starsScore = 3;
        }

        for (int i = 0; i < starsScore; i++)
        {
            stars[i].sprite = starPerfect;
        }

        if (PlayerPrefs.GetInt("Level" + _currentLevel.ToString() + "BestStar") < starsScore)
        {
            PlayerPrefs.SetInt("Level" + _currentLevel.ToString() + "BestStar", starsScore);
        }
    }

    // Dialogue Methods

    public void SetClientIcon(Sprite skinIcon, Sprite hairIcon)
    {
        currentClientIcon = skinIcon;
        currentClientHair = hairIcon;
    }

    public void SetInteractMessages(string[] messages, InteractableTool[] tool)
    {
        currentInterectMessages = messages;

        for (int i = 0; i < tool.Length; i++)
        {
            tool[i].PlayerGetIt += OnPlayerGetIt;
        }
    }

    public void OnPlayerGetIt()
    {
        interactedText.text = currentInterectMessages[interactionsCount];
        interactionAnim.SetTrigger("Interacted");

        interactionsCount++;
    }

    public void StartDialogue(ClientInfo clientInfo)
    {
        currentClient = clientInfo;
        dialoguePanel.SetActive(true);
    }

    public void ClientAnswer(string answer)
    {
        EnableDialogueActor(false);
        clientText.text = answer;
    }

    public void EnableDialogueActor(bool isPlayer)
    {
        playerLayout.SetActive(isPlayer);
        clientLayout.SetActive(!isPlayer);

        if (!isPlayer)
        {
            clientName.text = currentClient.clientName;
            clientIcon.sprite = currentClientIcon;
            clientHairIcon.sprite = currentClientHair;
        }   
    }

    public void SetupClientLayout(string name, Sprite icon)
    {
        clientName.text = name;
        clientIcon.sprite = icon;
    }

    public void SetInteractableButtons(bool[] interactable, Questions[] questions)
    {
        for (int i = 0; i < interactable.Length; i++)
        {
            dialogueButtons[i].interactable = interactable[i];
        }

        for (int i = 0; i < buttonsText.Length; i++)
        {
            buttonsText[i].text = questions[i].question;
        }
    }

    public void SetSubInteractableButtons(TasteQuestions[] questions)
    {
        for (int i = 0; i < buttonsText.Length; i++)
        {
            buttonsText[i].text = questions[i].question;
        }
    }

    public void SetSubInteractableButtons(IngredientQuestions[] questions)
    {
        for (int i = 0; i < buttonsText.Length; i++)
        {
            buttonsText[i].text = questions[i].food.name;
        }
    }

    public void ResetButtons()
    {
        for (int i = 0; i < dialogueButtons.Length; i++)
        {
            dialogueButtons[i].interactable = true;
        }
    }

    public void StopDialogue()
    {
        dialoguePanel.SetActive(false);
    }

}

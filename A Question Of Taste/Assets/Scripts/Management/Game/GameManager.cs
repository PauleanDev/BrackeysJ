using System.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Statics
    public static float startTime = 1;

    [Header("Input")]
    [SerializeField] InputActionAsset _inputActions;
    InputAction _pause;

    // Setup
    float _gameTime;
    int[] _reviewsScore;
    int score = 0;

    // External References
    PointClick[] _playerPClick;
    UIManagement _uiManagement;
    DialogueManager _dialogueManager;

    // Pause state
    bool _isPaused = false;
    bool _canPause = true;

    private void Awake()
    {
        _pause = _inputActions.FindAction("Pause");
        _playerPClick = GameObject.FindGameObjectWithTag("Player").GetComponents<PointClick>();
        _uiManagement = GetComponent<UIManagement>();
        _dialogueManager = GetComponent<DialogueManager>();

        ClientInteraction.Avaliated += OnAvaliated;
        ClientInteraction.Called += OnCalled;
    }

    private void Update()
    {
        _gameTime -= Time.deltaTime;

        _uiManagement.TimerUpdate(_gameTime);

        if (_gameTime <= 0)
        {
            FinishLevel();
        }

        if (_pause.triggered && _canPause)
        {
            PauseGame();
        }
    }

    private void OnDestroy()
    {
        ClientInteraction.Avaliated -= OnAvaliated;
        ClientInteraction.Called -= OnCalled;
    }

    // Game

    public void Setup(SLevelScript levelScript)
    {
        _gameTime = levelScript.gameTime;
        _reviewsScore = levelScript.reviewsScore;
    }

    // Dialogue
    private void OnCalled(ClientInteraction client)
    {
        _dialogueManager.SetClientInfo(client);

        for (int i = 0; i < _playerPClick.Length; i++) 
        {
            _playerPClick[i].enabled = false;
        }
    }

    private void OnAvaliated(int rating)
    {
        score += _reviewsScore[rating];
        _uiManagement.ScoreUpdate(score);
    }

    public void StopDialogue()
    {
        _dialogueManager.StopDialogue();

        for (int i = 0; i < _playerPClick.Length; i++)
        {
            _playerPClick[i].enabled = true;
        }
    }

    // Scene
    public void RestartGame()
    {
        Time.timeScale = 1;
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneLoader.Instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneLoader.Instance.LoadSceneAsync("Menu");
    }

    public void FinishLevel()
    {
        _canPause = false;
        Time.timeScale = 1;

        _uiManagement.FinishGameUI(score);
    }

    public void PauseGame()
    {
        _isPaused = !_isPaused;

        Time.timeScale = _isPaused ? 1 : 0;
        _uiManagement.PauseUI(_isPaused);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    private UIManagement uiManagement;
    [SerializeField] private InputActionAsset inputActions;

    private InputAction pause;

    public static float startGame { get; private set; } = 3f;
    [SerializeField] private float gameTime;
    [SerializeField] private int[] reviewsScore;

    private int score = 0;
    private bool paused = false;

    public delegate void GameFinishedHandler();
    public static event GameFinishedHandler GameFinished;

    private void Awake()
    {
        pause = inputActions.FindAction("Pause");

        uiManagement = GetComponent<UIManagement>();

        ClientInteraction.Avaliated += OnAvaliated;
    }

    private void Update()
    {
        gameTime -= Time.deltaTime;

        uiManagement.TimerUpdate(gameTime);

        if (gameTime <= 0)
        {
            FinishLevel();
        }

        if (pause.triggered)
        {
            PauseGame(paused);
        }
    }

    private void OnAvaliated(int rating)
    {
        score += reviewsScore[rating];
        uiManagement.ScoreUpdate(score);
    }
    public void RestartGame()
    {
        SceneLoader.Instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneLoader.Instance.LoadSceneAsync("Menu");
    }

    public void FinishLevel()
    {
        PlayerPrefs.SetInt("LastScore", score);
        GameFinished();
    }

    public void PauseGame(bool isPaused)
    {
        if (isPaused)
        {
            paused = false;
            Time.timeScale = 1;
            uiManagement.PauseUI(isPaused);
        }
        else
        {
            paused = true;
            uiManagement.PauseUI( isPaused);
            Time.timeScale = 0;
        }
    }
}

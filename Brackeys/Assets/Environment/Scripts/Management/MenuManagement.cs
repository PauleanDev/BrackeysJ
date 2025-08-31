using UnityEngine;

public class MenuManagement : MonoBehaviour
{
    [Header("Menu Windows")]
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject Levels;
    [SerializeField] private GameObject Options;
    [SerializeField] private GameObject credits;

    public void OpenMainMenu()
    {
        main.SetActive(true);
        Levels.SetActive(false);
        Options.SetActive(false);
        credits.SetActive(false);
    }

    public void OpenLevelsMenu()
    {
        main.SetActive(false);
        Levels.SetActive(true);
        Options.SetActive(false);
        credits.SetActive(false);
    }

    public void OpenOptionsMenu()
    {
        Options.SetActive(true);
    }

    public void OpenCredits()
    {
        credits.SetActive(true);
        main.SetActive(false);
        Levels.SetActive(false);
        Options.SetActive(false);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

}

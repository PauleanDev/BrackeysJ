using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        PlayerPrefs.SetInt("Level", level);
        SceneLoader.Instance.LoadSceneAsync("DefaultLevel");
    }

    public void Restart()
    {
        SceneLoader.Instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}

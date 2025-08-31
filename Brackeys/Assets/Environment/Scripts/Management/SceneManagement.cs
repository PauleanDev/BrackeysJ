using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadSceneX(string x)
    {
        SceneLoader.Instance.LoadSceneAsync(x);
    }

    public void Restart()
    {
        SceneLoader.Instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}

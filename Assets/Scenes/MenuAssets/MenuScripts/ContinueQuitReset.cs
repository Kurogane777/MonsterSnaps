using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueQuitReset : MonoBehaviour
{
    public string _goToScene;

    public void SceneToGo()
    {
        if (_goToScene != null) { SceneManager.LoadScene(_goToScene); }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

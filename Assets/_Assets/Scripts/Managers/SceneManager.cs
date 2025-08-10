using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneManager : MonoBehaviour
{
    private LoadGame _loadGame;

    [Inject]
    public void Construction(LoadGame loadGame)
    {
        _loadGame = loadGame;
    }
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    public void LoadGame(int sceneIndex)
    {
        _loadGame.LoadGameBtn();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    public void LoadUnitSelectScene()
    {
        SceneManager.LoadScene(1);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}

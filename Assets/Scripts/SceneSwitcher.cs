using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GotoMenuScene()
    {
        SceneManager.LoadScene("_Main");
    }
    public void doExitGame()
    {
        Application.Quit();
    }
}
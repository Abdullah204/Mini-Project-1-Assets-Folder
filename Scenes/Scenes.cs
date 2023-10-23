using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public void GamePlaySceneWithRestart()
    {
        
        SceneManager.LoadScene("Gameplay");
        Time.timeScale = 1;
    }

    public void GamePlaySceneWithResume()
    {
        Time.timeScale = 1 - Time.timeScale;
    }

    public void GameOverScene()
    {
        SceneManager.LoadScene("Game Over");
    }

    public void PauseScene()
    {
        SceneManager.LoadScene("Pause");
    }

    public void TitleScene()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public void Quit()
    {
        Application.Quit();
    }


}
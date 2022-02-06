using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void LoadLevelSelectScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadSettingsScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadMapOne()
    {
        SceneManager.LoadScene(4);
    }
    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

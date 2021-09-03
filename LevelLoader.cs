using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    int currentSceneIndex;
    float delayLoad = 2f;

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(currentSceneIndex == 0)
        {
            StartCoroutine(SplashScreen());
        }
    }

    IEnumerator SplashScreen()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    

    public void LoadNextScene()
    {
        StartCoroutine(LoadNextWithDelay());
    }
    IEnumerator LoadNextWithDelay()
    {
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1;
        SceneManager.LoadScene(currentSceneIndex + 1);       
    }


    public void LoadGameOver()
    {        
        StartCoroutine(LoadGameOverDelay());
    }

    IEnumerator LoadGameOverDelay()
    {        
        yield return new WaitForSeconds(1.5f);        
        SceneManager.LoadScene("GameOver");
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadMainMenuDelay());
    }

    IEnumerator LoadMainMenuDelay()
    {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<GameSession>().DestroyGameSession();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCurrentSceneAgain()
    {
        StartCoroutine(LoadCurrentSceneDelay());
    }

    IEnumerator LoadCurrentSceneDelay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void QuitWeb()
    {
        Application.OpenURL("about:blank");
    }
}

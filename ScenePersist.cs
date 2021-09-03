using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int startSceneIndex;
    int currentSceneIndex;

    private void Awake()
    {       
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersist > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        startSceneIndex = SceneManager.GetActiveScene().buildIndex;        
    }

    void Update()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != startSceneIndex)
        {
            Destroy(gameObject);
        }
    }

    public void DestroyScene()
    {
        Destroy(gameObject);
    }
}

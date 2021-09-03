using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float timeToWait = 3f;
    [SerializeField] float timeScaleExit = 0.4f;
    [SerializeField] GameObject doorOpenInstance;
    [SerializeField] AudioClip levelCompleteSound;

    Player player;
    int numCapturedAnimals;
    int indexCurrentScene;
    AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
        indexCurrentScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        numCapturedAnimals = player.GetNumCapturedAnimals();
        bool isPlayerTouching = GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player"));
        if (isPlayerTouching && numCapturedAnimals >=3)
        {
            AudioSource.PlayClipAtPoint(levelCompleteSound, Camera.main.transform.position);
            StartCoroutine(LoadNextSceneDelay());
        }
    }

    IEnumerator LoadNextSceneDelay()
    {
        GameObject doorOpen = Instantiate(doorOpenInstance, transform.position, Quaternion.identity) as GameObject;
        var myScene = FindObjectOfType<ScenePersist>();
        if (!myScene) { yield break; }
        FindObjectOfType<ScenePersist>().DestroyScene();
        Time.timeScale = timeScaleExit;        
        yield return new WaitForSeconds(timeToWait);
        Time.timeScale = 1;
        Destroy(doorOpen);
        SceneManager.LoadScene(indexCurrentScene + 1);
    }
}

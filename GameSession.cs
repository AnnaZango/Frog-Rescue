using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerNumLives = 3;
    [SerializeField] float score = 0;
    
    [SerializeField] int capturedAnimals = 0;
    [SerializeField] int numExtraJumps = 1;
    

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    

    public void ProcessPlayerDeath()
    {
        if (playerNumLives > 1)
        {
            TakeLife();            
            FindObjectOfType<LevelLoader>().LoadCurrentSceneAgain();
        }
        else
        {
            playerNumLives = 0;                      
            ResetGameSession();
        }
    }

    private void TakeLife()
    {
        playerNumLives--;
    }
    
    private void ResetGameSession()
    {        
        FindObjectOfType<LevelLoader>().LoadGameOver();
    }

    public void DestroyGameSession()
    {
        Destroy(this.gameObject);
    }

    public int GetPlayerLives()
    {
        if (playerNumLives > 0)
        {
            return playerNumLives;
        }
        else
        {
            playerNumLives = 0;
            return playerNumLives;
        }
    }

    public void IncreaseScore(float pointsToAdd)
    {
        score = score + pointsToAdd;
    }

    public void DecreseScore(float cost)
    {
        score = score - cost;
    }

    public float GetScore()
    {
        return score;
    }        

    public int GetNumExtraJumps()
    {
        return numExtraJumps;
    }

    public void IncreaseNumLives()
    {
        if (playerNumLives < 3)
        {
            playerNumLives++;
        }
    }

}

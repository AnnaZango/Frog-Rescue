using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShop : MonoBehaviour
{
    bool isPicked = false;
   
    [SerializeField] float costCoins = 100;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPicked)
        {
            float currentCoins = FindObjectOfType<GameSession>().GetScore();

            if (currentCoins < costCoins) { return; }
            isPicked = true;
            FindObjectOfType<GameSession>().DecreseScore(costCoins);
            FindObjectOfType<GameSession>().IncreaseNumLives();
            
            Destroy(gameObject);
        }
    }
}

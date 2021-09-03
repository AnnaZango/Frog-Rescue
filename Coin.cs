using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    bool isPicked = false;
    AudioSource myAudioSource;
    [SerializeField] AudioClip mySound;
    [SerializeField] float pointsPerCoin = 100;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (!isPicked)
        {
            AudioSource.PlayClipAtPoint(mySound, Camera.main.transform.position);
            isPicked = true;
            FindObjectOfType<GameSession>().IncreaseScore(pointsPerCoin);            
            Destroy(gameObject);            
        }        
    }
}

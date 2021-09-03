using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsDisappear : MonoBehaviour
{
    [SerializeField] Sprite[] spritesHearts;
    [SerializeField] Image heartUI;
    GameSession gameSession;
    
    private void Update()
    {
        gameSession = FindObjectOfType<GameSession>();
        int currentHealth = gameSession.GetPlayerLives();
        heartUI.sprite = spritesHearts[currentHealth];
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI myTMPro;
    GameSession myGameSession;
    float score = 0;

    void Start()
    {
        myTMPro = GetComponent<TextMeshProUGUI>();
        myGameSession = FindObjectOfType<GameSession>();
        myTMPro.text = score.ToString();
    }

    void Update()
    {
        score = FindObjectOfType<GameSession>().GetScore();
        myTMPro.text = score.ToString();
    }
}

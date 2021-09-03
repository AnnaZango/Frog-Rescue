using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumCapturedAnimalsDisplay : MonoBehaviour
{
    Player player;
    TextMeshProUGUI myTMPro;
    int numCapturedAnimals =0;

    void Start()
    {
        player = FindObjectOfType<Player>();
        myTMPro = GetComponent<TextMeshProUGUI>();        
    }

    void Update()
    {
        numCapturedAnimals = player.GetNumCapturedAnimals();
        myTMPro.text = numCapturedAnimals.ToString();
    }
}

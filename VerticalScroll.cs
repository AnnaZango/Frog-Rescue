using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    [Tooltip ("Game units per second")]
    [SerializeField] float speedRising = 1.5f;

    void Update()
    {
        transform.Translate(new Vector2(0f, speedRising * Time.deltaTime));
    }
}

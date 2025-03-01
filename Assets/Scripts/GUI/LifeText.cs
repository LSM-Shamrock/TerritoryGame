using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeText : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        var player = FindObjectOfType<Player>();
        text.text = "Life : " + player.LifeCount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeText : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void LateUpdate()
    {
        var player = FindObjectOfType<Player>();
        text.text = "Life : " + player.LifeCount;
    }
}

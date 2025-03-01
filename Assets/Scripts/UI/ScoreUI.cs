using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        var player = FindObjectOfType<PlayerUnit>();
        text.text = $"Score : {player.Score}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] Text lifeText;
    [SerializeField] Text progressText;

    private void LateUpdate()
    {
        var player = FindObjectOfType<Player>();
        lifeText.text = "Life : " + player.LifeCount;

        var map = FindObjectOfType<Map>();
        var mapArea = map.MapArea.Count;
        var playerArea = map.PlayerArea.Count;
        var percentage = (float)playerArea / mapArea * 100;
        progressText.text = $"{percentage:F2}% ({playerArea} / {mapArea})";
    }
}

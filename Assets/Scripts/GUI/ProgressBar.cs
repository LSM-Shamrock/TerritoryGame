using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    Slider progressSlider;
    Text progressText;

    private void Awake()
    {
        progressSlider = GetComponentInChildren<Slider>();
        progressText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        var map = FindObjectOfType<Map>();
        var mapArea = map.MapArea.Count;
        var playerArea = map.PlayerArea.Count;
        var progress = (float)playerArea / mapArea;
        progressSlider.value = progress;
        progressText.text = $"{(int)(progress * 100f)}%";
    }
}

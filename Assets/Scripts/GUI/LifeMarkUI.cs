using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeMarkUI : MonoBehaviour
{
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    Image[] images;

    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        var player = FindObjectOfType<PlayerUnit>();
        for (int i = 0; i < images.Length; i++)
        {
            if (player.LifeCount > i)
            {
                images[i].sprite = onSprite;
            }
            else
            {
                images[i].sprite = offSprite;
            }
        }
    }
}

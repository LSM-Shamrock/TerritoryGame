using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [SerializeField] Sprite lifeSprite;
    [SerializeField] Sprite lifeOffSprite;
    [SerializeField] Sprite defenseSprite;
    [SerializeField] Sprite invincivilitySprite;

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
                images[i].sprite = lifeSprite;
                if (player.IsDefense && player.LifeCount == i + 1)
                {
                    images[i].sprite = defenseSprite;
                }
                if (player.IsInvincibility)
                {
                    images[i].sprite = invincivilitySprite;
                }
            }
            else
            {
                images[i].sprite = lifeOffSprite;
            }
        }
    }
}

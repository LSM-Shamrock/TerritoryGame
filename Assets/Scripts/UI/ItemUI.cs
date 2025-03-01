using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] Sprite temSprite_speed;
    [SerializeField] Sprite temSprite_defense;
    [SerializeField] Sprite temSprite_invincibility;
    [SerializeField] Sprite temSprite_life;

    Image image;
    Image childImage;

    private void Awake()
    {
        image = GetComponent<Image>();
        childImage = GetComponentsInChildren<Image>().First(a => a != image);
    }

    private void LateUpdate()
    {
        var player = FindObjectOfType<PlayerUnit>();
        if (player.ItemTime > 0)
        {
            image.enabled = true;
            childImage.enabled = true;
            switch (player.Item)
            {
                case ItemType.Speed:
                    childImage.sprite = temSprite_speed;
                    break;
                case ItemType.Defense:
                    childImage.sprite = temSprite_defense;
                    break;
                case ItemType.Invincibility:
                    childImage.sprite = temSprite_invincibility;
                    break;
                case ItemType.Life:
                    childImage.sprite = temSprite_life;
                    break;
                default:
                    childImage.enabled = false;
                    break;
            }
            var a = Mathf.Min(1f, player.ItemTime / 2f);
            image.color = new(0f, 0f, 0f, a);
            childImage.color = new(1f, 1f, 1f, a);
        }
        else
        {
            image.enabled = false;
            childImage.enabled = false;
        }
    }
}

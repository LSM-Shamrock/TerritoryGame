using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemType
{
    Speed,
    Defense,
    Invincibility,
    Life,
    Random,
}

public class Item : MonoBehaviour
{
    public ItemType Type { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.attachedRigidbody.GetComponent<PlayerUnit>();
        if (player != null)
        {
            GetItem();
        }
    }
    protected virtual void GetItem()
    {
        var player = FindObjectOfType<PlayerUnit>();
        if (Type == ItemType.Random)
        {
            var types = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList();
            types.Remove(ItemType.Random);
            Type = types[UnityEngine.Random.Range(0, types.Count)];
        }
        Debug.Log($"{Type}æ∆¿Ã≈€ »πµÊ");
        FindObjectOfType<ItemPickup>().Show(Sprite);
        player.ApplyItem(Type);
        Destroy(gameObject);
    }


    public Sprite sprite_Speed;
    public Sprite sprite_Defense;
    public Sprite sprite_Invincibility;
    public Sprite sprite_Life;
    public Sprite sprite_Random;
    public Sprite Sprite
    {
        get 
        {
            Sprite result;
            switch (Type)
            {
                case ItemType.Speed: 
                    result = sprite_Speed;
                    break;
                case ItemType.Defense: 
                    result = sprite_Defense;
                    break;
                case ItemType.Invincibility: 
                    result = sprite_Invincibility;
                    break;
                case ItemType.Life: 
                    result = sprite_Life;
                    break;
                case ItemType.Random: 
                    result = sprite_Random;
                    break;
                default: 
                    result = null;
                    break;
            }
            return result;
        }
    }
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        spriteRenderer.sprite = Sprite;
    }
}

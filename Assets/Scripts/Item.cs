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
    [SerializeField] ItemType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.attachedRigidbody.GetComponent<PlayerUnit>();
        if (player != null)
        {
            AcquireItem();
        }
    }

    protected virtual void AcquireItem()
    {
        var player = FindObjectOfType<PlayerUnit>();
        if (type == ItemType.Random)
        {
            var types = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList();
            types.Remove(ItemType.Random);
            type = types[UnityEngine.Random.Range(0, types.Count)];
        }
        Debug.Log($"{type}æ∆¿Ã≈€ »πµÊ");
        player.ApplyItem(type);
        Destroy(gameObject);
    }
}

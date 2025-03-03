using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    SpriteRenderer sr;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = null;
    }
    public void Show(Sprite sprite)
    {
        sr.sprite = sprite;
        animator.Rebind();
    }
}

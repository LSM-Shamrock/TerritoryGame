using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShow : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
    }
    public void Show(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        animator.Rebind();
    }
}

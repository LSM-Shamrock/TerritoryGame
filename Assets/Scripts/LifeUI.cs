using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var playerLife = GameManager.instance.playerLife;
            child.gameObject.SetActive(i < playerLife);
        }
    }
}

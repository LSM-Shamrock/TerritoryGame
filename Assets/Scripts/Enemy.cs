using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    private float remainingDist;
    private Vector3 moveDir;

    private void Update()
    {
        if (remainingDist <= 0f)
        {
            if (Random.Range(0, 100) == 0)
            {
                var angle = Random.Range(0, 4) * 90f;
                moveDir.x = Mathf.Cos(angle * Mathf.Deg2Rad);
                moveDir.y = Mathf.Sin(angle * Mathf.Deg2Rad);
                remainingDist = Random.Range(1f, 3f);
            }
        }
        else
        {
            var moveAmount = moveSpeed * Time.deltaTime;
            transform.position += moveDir * moveAmount;
            remainingDist -= moveAmount;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float moveSpeed = 5f;
    private Vector3Int endPos;

    private void Awake()
    {
        endPos = Vector3Int.RoundToInt(transform.position);
    }

    private void Update()
    {
        UpdateMove();
    }

    private void UpdateMove()
    {
        var remainingDist = Vector3.Distance(transform.position, endPos);
        if (remainingDist == 0f)
        {
            var validDirSet = new HashSet<Vector3Int>
            {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right,
            };
            validDirSet.RemoveWhere((dir) => Physics2D.OverlapPoint((Vector3)(endPos + dir), wallLayer) != null);
            if (validDirSet.Count > 0)
            {
                var validDirArr = validDirSet.ToArray();
                endPos += validDirArr[Random.Range(0, validDirArr.Length)];
            }
        }
        var moveAmount = Mathf.Min(remainingDist, moveSpeed * Time.deltaTime);
        var moveDir = (endPos - transform.position).normalized;
        transform.position += moveDir * moveAmount;
    }
}

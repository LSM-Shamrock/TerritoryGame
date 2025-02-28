using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 8f;
    private Vector3Int endPos;
    private Vector3Int nextDir;

    private void Update()
    {
        var map = FindObjectOfType<Map>();
        var p = Vector3Int.RoundToInt(transform.position);
        if (!map.VirusArea.Contains(p))
        {
            Dead();
        }
        UpdateMove();
    }

    private void Dead()
    {
        Destroy(gameObject);
    }

    private void SetRandomNextDir()
    {
        var dirSet = new HashSet<Vector3Int>
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right,
        };
        var map = FindObjectOfType<Map>();
        dirSet.RemoveWhere((dir) => !map.VirusArea.Contains(endPos + dir));
        if (dirSet.Count > 0)
        {
            var dirArr = dirSet.ToArray();
            var randIdex = Random.Range(0, dirArr.Length);
            nextDir = dirArr[randIdex];
        }
    }

    private void UpdateMove()
    {
        var remainingDist = Vector3.Distance(transform.position, endPos);
        var moveAmount = moveSpeed * Time.deltaTime;
        if (remainingDist <= moveAmount)
        {
            transform.position = endPos;
            SetRandomNextDir();
            var map = FindObjectOfType<Map>();
            if (map.MapArea.Contains(endPos + nextDir))
            {
                endPos += nextDir;
            }
        }
        else
        {
            var moveDir = (endPos - transform.position).normalized;
            transform.position += moveDir * moveAmount;
        }
    }
}

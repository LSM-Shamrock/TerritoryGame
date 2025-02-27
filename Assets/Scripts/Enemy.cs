using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Mover mover;

    private void Awake()
    {
        mover = GetComponent<Mover>();
    }

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

    private void UpdateMove()
    {
        if (mover.RemainingDist > 1f)
        {
            var roundPos = Vector3Int.RoundToInt(transform.position);
            mover.endPos = roundPos;
        }
        if (mover.RemainingDist <= 0f)
        {
            transform.position = mover.endPos;
            var validDirSet = new HashSet<Vector3Int>
            {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right,
            };
            var map = FindObjectOfType<Map>();
            validDirSet.RemoveWhere((dir) => !map.VirusArea.Contains(mover.endPos + dir));
            if (validDirSet.Count > 0)
            {
                var validDirArr = validDirSet.ToArray();
                mover.endPos += validDirArr[Random.Range(0, validDirArr.Length)];
            }
        }
        mover.Move();
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}

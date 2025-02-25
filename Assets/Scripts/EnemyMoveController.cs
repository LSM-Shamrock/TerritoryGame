using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMoveController : MonoBehaviour
{
    [SerializeField] 
    private float moveSpeed = 5f;
    private Vector3Int endPos;

    private void Update()
    {
        UpdateMove();
    }

    private void UpdateMove()
    {
        var remainingDist = Vector3.Distance(transform.position, endPos);
        if (remainingDist > 1f)
        {
            endPos = Vector3Int.RoundToInt(transform.position);
        }
        if (remainingDist <= 0)
        {
            transform.position = endPos;
            var validDirSet = new HashSet<Vector3Int>
            {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right,
            };
            var virusAreaTilemap = GameManager.instance.virusAreaTilemap;
            validDirSet.RemoveWhere((dir) => !virusAreaTilemap.HasTile(endPos + dir));
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Virus : MonoBehaviour
{
    public float moveSpeed = 8f;

    public Vector3Int RoundPos => Vector3Int.RoundToInt(transform.position);

    public HashSet<Vector3Int> MoveableArea
    {
        get 
        {
            var map = FindObjectOfType<Map>();
            return map.VirusArea; ;
        }
    }

    private void Start()
    {
        StartCoroutine(RepeatMovement());
    }

    private void Update()
    {
        var p = Vector3Int.RoundToInt(transform.position);
        if (!MoveableArea.Contains(p))
        {
            Dead();
        }
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

    public HashSet<Vector3Int> GetMoveableDir(Vector3Int p)
    {
        var result = new HashSet<Vector3Int>
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right,
        };
        result.RemoveWhere(dir => !MoveableArea.Contains(p + dir));
        return result;
    }

    private IEnumerator Move(Vector3Int endPos)
    {
        var moveDir = (endPos - transform.position).normalized;
        var remainingDist = Vector3.Distance(transform.position, endPos);
        while (remainingDist > 0)
        {
            var moveAmount = moveSpeed * Time.deltaTime;
            transform.position += moveDir * moveAmount;
            remainingDist -= moveAmount;
            yield return null;
        }
        transform.position = endPos;
    }

    private IEnumerator RepeatMovement()
    {
        while (true)
        {
            var moveableDir = GetMoveableDir(RoundPos).ToArray();
            if (moveableDir.Length == 0)
            {
                continue;
            }
            var moveDir = moveableDir[Random.Range(0, moveableDir.Length)];
            var moveDist = Random.Range(1, 4);
            for (int i = 1; i <= moveDist; i++)
            {
                var movePos = RoundPos + moveDir * i;
                if (!MoveableArea.Contains(movePos))
                {
                    break;
                }
                yield return Move(movePos);
            }
            var waitTime = Random.Range(0f, 1f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}

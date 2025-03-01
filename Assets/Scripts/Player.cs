using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 8f;
    private Vector3Int endPos;
    private Vector3Int nextDir;

    public int LifeCount { get; private set; } = 5;

    public HashSet<Vector3Int> MoveableArea
    {
        get
        {
            var map = FindObjectOfType<Map>();
            return map.MapArea;
        }
    }

    private void Start()
    {
        MoveToRandomBorderPos();
    }

    private void Update()
    {
        UpdateInput();
        UpdateMove();
    }

    public void LoseLife()
    {
        LifeCount--;
    }

    private void MoveToRandomBorderPos()
    {
        var map = FindObjectOfType<Map>();
        var minX = map.Min.x;
        var maxX = map.Max.x;
        var minY = map.Min.y;
        var maxY = map.Max.y;
        var pos = Vector3Int.zero;
        switch (Random.Range(0, 4))
        {
            case 0:
                pos.y = maxY;
                pos.x = Random.Range(minX, maxX + 1);
                nextDir = Vector3Int.down;
                break;
            case 1:
                pos.y = minY;
                pos.x = Random.Range(minX, maxX + 1);
                nextDir = Vector3Int.up;
                break;
            case 2:
                pos.x = maxX;
                pos.y = Random.Range(minY, maxY + 1);
                nextDir = Vector3Int.left;
                break;
            case 3:
                pos.x = minX;
                pos.y = Random.Range(minY, maxY + 1);
                nextDir = Vector3Int.right;
                break;
        }
        endPos = pos;
        transform.position = pos;
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

    private void UpdateInput()
    {
        var input = Vector3Int.zero;
        input.x = (int)Input.GetAxisRaw("Horizontal");
        input.y = (int)Input.GetAxisRaw("Vertical");

        var validDir = GetMoveableDir(endPos);
        if (validDir.Contains(input))
        {
            nextDir = input;
        }
    }
    
    private void UpdateMove()
    {
        var remainingDist = Vector3.Distance(transform.position, endPos);
        var moveAmount = moveSpeed * Time.deltaTime;
        if (remainingDist <= moveAmount)
        {
            transform.position = endPos;
            if (MoveableArea.Contains(endPos + nextDir))
            {
                transform.up = nextDir;
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

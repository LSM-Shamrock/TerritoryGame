using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    private float moveSpeed = 8f;
    private Vector3Int endPos;
    private Vector3Int nextDir;

    private void Start()
    {
        MoveToRandomBorderPoint();
    }

    private void Update()
    {
        UpdateInput();
        UpdateMove();
    }

    private void UpdateInput()
    {
        var inputDir = Vector3Int.zero;
        inputDir.x = (int)Input.GetAxisRaw("Horizontal");
        inputDir.y = (int)Input.GetAxisRaw("Vertical");
        var validDirSet = new HashSet<Vector3Int>() 
        { 
            Vector3Int.up, 
            Vector3Int.down, 
            Vector3Int.left, 
            Vector3Int.right, 
        };
        var bounds = GameManager.instance.mapBounds;
        validDirSet.RemoveWhere(dir => !bounds.Contains(endPos + dir));
        if (validDirSet.Contains(inputDir))
        {
            nextDir = inputDir;
        }
    }
    
    private void UpdateMove()
    {
        var remainingDist = Vector3.Distance(transform.position, endPos);
        //if (remainingDist > 1f)
        //{
        //    endPos = Vector3Int.RoundToInt(transform.position);
        //}
        if (remainingDist < 0.001f)
        {
            transform.position = endPos;
            var bounds = GameManager.instance.mapBounds;
            if (bounds.Contains(endPos + nextDir))
            {
                endPos += nextDir;
            }
        }
        var moveAmount = Mathf.Min(remainingDist, moveSpeed * Time.deltaTime);
        var moveDir = (endPos - transform.position).normalized;
        transform.position += moveDir * moveAmount;
    }

    private void MoveToRandomBorderPoint()
    {
        var gameArea = GameManager.instance.mapBounds;
        var minX = gameArea.min.x + 0.5f;
        var maxX = gameArea.max.x - 0.5f;
        var minY = gameArea.min.y + 0.5f;
        var maxY = gameArea.max.y - 0.5f;
        var pos = Vector3.zero;
        switch (Random.Range(0, 4))
        {
            case 0:
                pos.y = maxY;
                pos.x = Random.Range(minX, maxX);
                nextDir = Vector3Int.down;
                break;
            case 1:
                pos.y = minY;
                pos.x = Random.Range(minX, maxX);
                nextDir = Vector3Int.up;
                break;
            case 2:
                pos.x = maxX;
                pos.y = Random.Range(minY, maxY);
                nextDir = Vector3Int.left;
                break;
            case 3:
                pos.x = minX;
                pos.y = Random.Range(minY, maxY);
                nextDir = Vector3Int.right;
                break;
        }
        endPos = Vector3Int.RoundToInt(pos);
        transform.position = endPos;
    }
}

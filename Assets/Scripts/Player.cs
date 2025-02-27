using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Mover mover;
    private Vector3Int nextDir;

    public int LifeCount { get; private set; } = 5;

    private void Awake()
    {
        mover = GetComponent<Mover>();
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

    private void MoveToRandomBorderPos()
    {
        var map = FindObjectOfType<Map>();
        var minX = map.Min.x;
        var maxX = map.Max.x;
        var minY = map.Min.y;
        var maxY = map.Max.y;
        var pos = Vector3.zero;
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
        var roundPos = Vector3Int.RoundToInt(pos);
        mover.endPos = roundPos;
        transform.position = roundPos;
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
        var map = FindObjectOfType<Map>();
        validDirSet.RemoveWhere(dir => !map.MapArea.Contains(mover.endPos + dir));
        if (validDirSet.Contains(inputDir))
        {
            nextDir = inputDir;
        }
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
            mover.endPos += nextDir;
        }
        mover.Move();
    }

    public void LoseLife()
    {
        LifeCount--;
    }
}

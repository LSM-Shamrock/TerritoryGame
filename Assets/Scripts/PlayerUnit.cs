using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Vector3Int endPos;
    private Vector3Int nextDir;


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
        UpdateItemTime();
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
        var moveAmount = moveSpeed * SpeedBoost * Time.deltaTime;
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


    public int Score { get; private set; }

    public int MaxLife { get; private set; } = 5;
    public int LifeCount { get; private set; } = 5;
    public void LifeDown()
    {
        if (!IsInvincibility)
        {
            if (IsDefense)
            {
                ItemTime = 0f;
            }
            else
            {
                LifeCount--;
            }
        }
        if (LifeCount <= 0)
        {
            Dead();
        }
    }
    public bool IsDead { get; private set; }
    public void Dead()
    {
        IsDead = true;
        Time.timeScale = 0f;
    }

    public ItemType Item { get; private set; }
    public float ItemTime { get; private set; } = 0f;
    public bool IsDefense => Item == ItemType.Defense && ItemTime > 0f;
    public bool IsInvincibility => Item == ItemType.Invincibility && ItemTime > 0f;
    public float SpeedBoost => Item == ItemType.Speed ? Mathf.Clamp(ItemTime, 1f, 2f) : 1f;
    public void ApplyItem(ItemType type)
    {
        Item = type;
        switch (type)
        {
            case ItemType.Speed:
                ItemTime = 4f;
                break;
            case ItemType.Defense:
                ItemTime = Mathf.Infinity;
                break;
            case ItemType.Invincibility:
                ItemTime = 10f;
                break;
            case ItemType.Life:
                if (LifeCount < MaxLife) LifeCount++;
                else Score += 35;
                ItemTime = 0f;
                break;
            default:
                ItemTime = 0f;
                break;
        }
    }
    private void UpdateItemTime()
    {
        if (ItemTime > Time.deltaTime)
        {
            ItemTime -= Time.deltaTime;
        }
        else
        {
            ItemTime = 0f;
        }
    }
}

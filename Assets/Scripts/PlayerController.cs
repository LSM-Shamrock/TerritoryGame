using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Vector2Int mapExtents;

    private Vector2Int inputDir = Vector2Int.right;
    private Vector2Int moveDir = Vector2Int.right;
    private float remainingDist;
    private float moveSpeed = 5f;

    private HashSet<Vector2Int> area = new();
    private List<Vector2Int> trail = new();
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        area.Add(Vector2Int.RoundToInt(transform.position));
    }

    private void Update()
    {
        InputUpdate();
        MoveUpdate();
        AddTrail();
        DrawTrail();
    }

    private void InputUpdate()
    {
        var inputVec = Vector2Int.zero;
        var roundPos = Vector2Int.RoundToInt(transform.position);
        inputVec.x = (int)Input.GetAxisRaw("Horizontal");
        inputVec.y = (int)Input.GetAxisRaw("Vertical");
        if (inputVec != Vector2Int.zero && inputVec != Vector2Int.one && 
            Mathf.Abs(roundPos.x + inputVec.x) <= mapExtents.x &&
            Mathf.Abs(roundPos.y + inputVec.y) <= mapExtents.y)
        {
            inputDir = inputVec;
        }
    }

    private void MoveUpdate()
    {
        if (remainingDist == 0f)
        {
            moveDir = inputDir;
            var roundPos = Vector2Int.RoundToInt(transform.position);
            if (Mathf.Abs(roundPos.x + moveDir.x) <= mapExtents.x &&
                Mathf.Abs(roundPos.y + moveDir.y) <= mapExtents.y)
            {
                remainingDist = 1f;
            }
        }
        var moveAmount = Mathf.Min(remainingDist, moveSpeed * Time.deltaTime);
        transform.position += (Vector3)(Vector2)moveDir * moveAmount;
        remainingDist -= moveAmount;
    }

    private void AddTrail()
    {
        var roundPos = Vector2Int.RoundToInt(transform.position);
        if (trail.Contains(roundPos))
        {
            var removeIndex = trail.IndexOf(roundPos);
            var removeCount = trail.Count - removeIndex;
            trail.RemoveRange(removeIndex, removeCount);
        }
        trail.Add(roundPos);
    }

    private void DrawTrail()
    {
        lineRenderer.positionCount = trail.Count;
        for (int i = 0; i < trail.Count; i++)
        {
            lineRenderer.SetPosition(i, (Vector3)(Vector2)trail[i]);
        }
    }

    private void FillArea()
    {
        for (int y = -mapExtents.y; y < mapExtents.y; y++)
        {
            for (int x = -mapExtents.x; x < mapExtents.x; x++)
            {
                bool hasRight = area.Any((p) => p.y == y && p.x > x);
                bool hasLeft = area.Any((p) => p.y == y && p.x < x);
                bool hasTop = area.Any((p) => p.x == x && p.y > y);
                bool hasBottom = area.Any((p) => p.x == x && p.y < y);
                if (hasRight && hasLeft && hasTop && hasBottom)
                {
                    area.Add(new(x, y));
                }
            }
        }
    }
}

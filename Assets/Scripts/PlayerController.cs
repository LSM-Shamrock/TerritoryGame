using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Vector2Int mapHalfSize;

    private Vector2Int inputDir = Vector2Int.right;
    private Vector2Int moveDir = Vector2Int.right;
    private float remainingDist;
    private float moveSpeed = 5f;

    private List<Vector2Int> trail = new();
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateInput();
        UpdateMove();
        UpdateTrail();
        TrailRendering();
    }

    private void UpdateInput()
    {
        var roundPos = Vector2Int.RoundToInt(transform.position);
        var inputVec = Vector2Int.zero;
        inputVec.x = (int)Input.GetAxisRaw("Horizontal");
        inputVec.y = (int)Input.GetAxisRaw("Vertical");
        if (inputVec != Vector2Int.zero && inputVec != Vector2Int.one && 
            Mathf.Abs(roundPos.x + inputVec.x) <= mapHalfSize.x &&
            Mathf.Abs(roundPos.y + inputVec.y) <= mapHalfSize.y)
        {
            inputDir = inputVec;
        }
    }

    private void UpdateMove()
    {
        if (remainingDist == 0f)
        {
            moveDir = inputDir;
            var roundPos = Vector2Int.RoundToInt(transform.position);
            if (Mathf.Abs(roundPos.x + moveDir.x) <= mapHalfSize.x &&
                Mathf.Abs(roundPos.y + moveDir.y) <= mapHalfSize.y)
            {
                remainingDist = 1f;
            }
        }
        var moveAmount = Mathf.Min(remainingDist, moveSpeed * Time.deltaTime);
        transform.position += (Vector3)(Vector2)moveDir * moveAmount;
        remainingDist -= moveAmount;
    }

    private void UpdateTrail()
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

    private void TrailRendering()
    {
        lineRenderer.positionCount = trail.Count;
        for (int i = 0; i < trail.Count; i++)
        {
            lineRenderer.SetPosition(i, (Vector3)(Vector2)trail[i]);
        }
    }
}

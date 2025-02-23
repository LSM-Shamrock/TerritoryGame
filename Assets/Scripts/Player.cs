using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CompositeCollider2D wallCollider;
    [SerializeField]
    private Tilemap virusAreaTilemap;

    private Vector3Int inputDir = Vector3Int.right;
    private Vector3Int moveDir;
    private float remainingDist;
    private float moveSpeed = 8f;
    private List<Vector3Int> trailPoints = new();
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        var minX = wallCollider.bounds.min.x + 1.5f;
        var maxX = wallCollider.bounds.max.x - 1.5f;
        var minY = wallCollider.bounds.min.y + 1.5f;
        var maxY = wallCollider.bounds.max.y - 1.5f;
        var pos = Vector3.zero;
        switch (Random.Range(0, 4))
        {
            case 0:
                pos.y = maxY;
                pos.x = Random.Range(minX, maxX);
                inputDir = Vector3Int.down;
                break;
            case 1:
                pos.y = minY;
                pos.x = Random.Range(minX, maxX);
                inputDir = Vector3Int.up;
                break;
            case 2:
                pos.x = maxX;
                pos.y = Random.Range(minY, maxY);
                inputDir = Vector3Int.left;
                break;
            case 3:
                pos.x = minX;
                pos.y = Random.Range(minY, maxY);
                inputDir = Vector3Int.right;
                break;
        }
        pos = Vector3Int.RoundToInt(pos);
        transform.position = pos;
    }

    private void Update()
    {
        UpdateInput();
        UpdateMove();
        UpdateTrail();
        TrailRendering();
        FillTrail();
    }

    private void UpdateInput()
    {
        var inputVec = Vector3Int.zero;
        inputVec.x = (int)Input.GetAxisRaw("Horizontal");
        inputVec.y = (int)Input.GetAxisRaw("Vertical");
        var validDir = new HashSet<Vector3Int>() 
        {
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.up,
            Vector3Int.down,
        };
        if (validDir.Contains(inputVec) &&
            !wallCollider.OverlapPoint(transform.position + inputVec))
        {
            inputDir = inputVec;
        }
    }
    
    private void UpdateMove()
    {
        if (remainingDist == 0f)
        {
            moveDir = inputDir;
            if (!wallCollider.OverlapPoint(transform.position + moveDir))
            {
                remainingDist = 1f;
            }
        }
        var moveAmount = Mathf.Min(remainingDist, moveSpeed * Time.deltaTime);
        transform.position += (Vector3)moveDir * moveAmount;
        remainingDist -= moveAmount;
    }

    private void UpdateTrail()
    {
        var p = Vector3Int.RoundToInt(transform.position);
        if (!trailPoints.Contains(p))
        {
            if (trailPoints.Count == 0 || 
                trailPoints[^1].x == p.x || trailPoints[^1].y == p.y)
            {
                trailPoints.Add(p);
            }

            if (trailPoints.Count >= 3)
            {
                var ab = trailPoints[^1] - trailPoints[^2];
                var bc = trailPoints[^2] - trailPoints[^3];
                if (ab / (int)ab.magnitude == bc / (int)bc.magnitude ||
                    ab / (int)ab.magnitude == -bc / (int)bc.magnitude)
                {
                    trailPoints.RemoveAt(trailPoints.Count - 2);
                }
            }

            if (trailPoints.Count >= 4)
            {
                var ab = trailPoints[^1] - trailPoints[^2];
                var cd = trailPoints[^3] - trailPoints[^4];
                if (ab / (int)ab.magnitude != -cd / (int)cd.magnitude)
                {
                    trailPoints.RemoveRange(0, trailPoints.Count - 3);
                }
            }

            if (trailPoints.Count >= 5)
            {
                var bc = trailPoints[^2] - trailPoints[^3];
                var de = trailPoints[^4] - trailPoints[^5];
                if (bc.magnitude < de.magnitude)
                {
                    trailPoints.RemoveRange(0, trailPoints.Count - 5);
                    trailPoints[0] = trailPoints[1] + bc;
                }
            }

            if (trailPoints.Count == 5)
            {
                var ab = trailPoints[^1] - trailPoints[^2];
                var cd = trailPoints[^3] - trailPoints[^4];
                if (ab.magnitude > cd.magnitude)
                {
                    trailPoints.RemoveAt(0);
                }
            }

            if (trailPoints.Count == 6)
            {
                var ab = trailPoints[^1] - trailPoints[^2];
                var cd = trailPoints[^3] - trailPoints[^4];
                var ef = trailPoints[^5] - trailPoints[^6];
                if (ab.magnitude + ef.magnitude > cd.magnitude)
                {
                    trailPoints.RemoveAt(0);
                }
            }
        }
    }

    private void FillTrail()
    {
        var p = Vector3Int.RoundToInt(transform.position);
        if (trailPoints.Count >= 5 && p == trailPoints[0]) 
        {
            var p1 = trailPoints[^2];
            var p2 = trailPoints[^4];
            var min = Vector3Int.Min(p1, p2);
            var max = Vector3Int.Max(p1, p2);
            for (int y = min.y; y <= max.y; y++)
            {
                for (int x = min.x; x <= max.x; x++)
                {
                    virusAreaTilemap.SetTile(new(x, y), null);
                }
            }
            trailPoints.Clear();
        }
    }

    private void TrailRendering()
    {
        lineRenderer.positionCount = trailPoints.Count;
        for (int i = 0; i < trailPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, (Vector3)trailPoints[i]);
        }
    }
}

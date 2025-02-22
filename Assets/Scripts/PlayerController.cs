using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Map map;

    private Vector2Int inputDir = Vector2Int.right;
    private Vector2Int moveDir;
    private float remainingDist;
    private float moveSpeed = 5f;

    private List<Vector2Int> trailPoints = new();
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        
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
            Mathf.Abs(roundPos.x + inputVec.x) <= map.halfSize.x &&
            Mathf.Abs(roundPos.y + inputVec.y) <= map.halfSize.y)
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
            if (Mathf.Abs(roundPos.x + moveDir.x) <= map.halfSize.x &&
                Mathf.Abs(roundPos.y + moveDir.y) <= map.halfSize.y)
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
        var p = Vector2Int.RoundToInt(transform.position);
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
                if (ab / (int)ab.magnitude == cd / (int)cd.magnitude)
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
        
        if (trailPoints.Count >= 5 &&
            p == trailPoints[0]) 
        {
            map.Fill(trailPoints[^2], trailPoints[^4]);
            trailPoints.Clear();
        }
    }

    private void TrailRendering()
    {
        lineRenderer.positionCount = trailPoints.Count;
        for (int i = 0; i < trailPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, (Vector3)(Vector2)trailPoints[i]);
        }
    }
}

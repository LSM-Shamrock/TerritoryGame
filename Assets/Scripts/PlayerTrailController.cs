using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTrailController : MonoBehaviour
{
    [SerializeField] 
    private Tilemap virusAreaTilemap;
    private LineRenderer lineRenderer;
    private List<Vector3Int> trailPoints = new();

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateTrail();
        TrailRendering();
        FillTrail();
    }

    private void UpdateTrail()
    {
        var p = Vector3Int.RoundToInt(transform.position);
        trailPoints.Add(p);
        if (trailPoints.Count >= 2)
        {
            var ab = trailPoints[^1] - trailPoints[^2];
            if (ab.x != 0 && ab.y != 0)
            {
                var removeCount = trailPoints.Count - 1;
                trailPoints.RemoveRange(0, removeCount);
                Debug.Log("1");
            }
        }
        // ac bc 비교 또는 a b 비교 필요 (중복 추가 방지) (ab랑 bc비교로는 a == b를 감지 안됨)
        if (trailPoints.Count >= 3)
        {
            var ab = trailPoints[^1] - trailPoints[^2];
            var bc = trailPoints[^2] - trailPoints[^3];
            if (ab.x == 0 && bc.x == 0 ||
                ab.y == 0 && bc.y == 0)
            {
                trailPoints.RemoveAt(trailPoints.Count - 2);
                Debug.Log("2");
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

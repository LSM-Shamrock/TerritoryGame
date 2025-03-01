using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTrail : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private List<Vector3Int> trailPoints = new();

    private void UpdatePoint()
    {
        var player = FindObjectOfType<PlayerUnit>();
        var p = Vector3Int.RoundToInt(player.transform.position);
        if (trailPoints.Contains(p))
        {
            if (trailPoints.Count >= 4 && p == trailPoints[0])
            {
                var p1 = trailPoints[1];
                var p2 = trailPoints[3];
                var map = FindObjectOfType<Map>();
                map.FillPlayerArea(p1, p2);
                trailPoints.Clear();
            }
            else
            {
                var pIndex = trailPoints.IndexOf(p);
                var removeIndex = pIndex + 1;
                var removeCount = trailPoints.Count - removeIndex;
                trailPoints.RemoveRange(removeIndex, removeCount);
            }
        }
        else
        {
            trailPoints.Add(p);
            if (trailPoints.Count >= 2)
            {
                var a = trailPoints[^1];
                var b = trailPoints[^2];
                if (a.x != b.x && a.y != b.y)
                {
                    trailPoints.Clear();
                    trailPoints.Add(a);
                }
            }
            if (trailPoints.Count >= 3)
            {
                var ab = trailPoints[^1] - trailPoints[^2];
                var bc = trailPoints[^2] - trailPoints[^3];
                if (ab.x * bc.y == bc.x * ab.y)
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
            if (trailPoints.Count >= 5)
            {
                var ab = trailPoints[^1] - trailPoints[^2];
                var cd = trailPoints[^3] - trailPoints[^4];
                if (ab.magnitude > cd.magnitude)
                {
                    trailPoints.RemoveAt(0);
                }
            }
        }
    }

    private void UpdateRenderer()
    {
        lineRenderer.positionCount = trailPoints.Count;
        for (int i = 0; i < trailPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, (Vector3)trailPoints[i]);
        }
    }

    private void UpdateCollider()
    {
        var vec2List = new List<Vector2>();
        foreach (var p in trailPoints)
        {
            vec2List.Add((Vector2Int)p);
        }
        edgeCollider.SetPoints(vec2List);
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        UpdatePoint();
        UpdateRenderer();
        UpdateCollider();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.attachedRigidbody.GetComponent<Virus>();
        if (enemy != null)
        {
            trailPoints.Clear();
            var player = FindObjectOfType<PlayerUnit>();
            player.LifeDown();
        }
    }
}

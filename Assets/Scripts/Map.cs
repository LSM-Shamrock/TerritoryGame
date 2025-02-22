using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Vector2Int halfSize;
    public SpriteRenderer tilePrefab;
    public Dictionary<(int, int), SpriteRenderer> tiles = new();

    private void Start()
    {
        for (int y = -halfSize.y; y <= +halfSize.y; y++)
        {
            for (int x = -halfSize.x; x <= +halfSize.x; x++)
            {
                tiles[(x, y)] = Instantiate(tilePrefab, new(x, y), new(), transform);
            }
        }
    }

    public void Fill(Vector2Int p1, Vector2Int p2)
    {
        var min = Vector2Int.Min(p1, p2);
        var max = Vector2Int.Max(p1, p2);
        for (int y = min.y; y <= max.y; y++)
        {
            for (int x = min.x; x <= max.x; x++)
            {
                if (tiles.TryGetValue((x, y), out var tile))
                {
                    tile.color = Color.white;
                }
            }
        }
    }
}

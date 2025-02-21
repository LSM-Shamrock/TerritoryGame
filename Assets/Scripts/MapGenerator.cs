using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tilePrefab;
    [SerializeField] private Vector2Int extents;

    private void Awake()
    {
        var boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = extents * 2 + Vector2Int.one;
    }

    private void Start()
    {
        for (int y = -extents.y; y <= +extents.y; y++)
        {
            for (int x = -extents.x; x <= +extents.x; x++)
            {
                Instantiate(tilePrefab, new(x, y), new(), transform);
            }
        }
    }
}

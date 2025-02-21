using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tilePrefab;
    [SerializeField] private Vector2Int mapHalfSize;

    private void Awake()
    {
        var boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = mapHalfSize * 2 + Vector2Int.one;
    }

    private void Start()
    {
        for (int y = -mapHalfSize.y; y <= +mapHalfSize.y; y++)
        {
            for (int x = -mapHalfSize.x; x <= +mapHalfSize.x; x++)
            {
                Instantiate(tilePrefab, new(x, y), new(), transform);
            }
        }
    }
}

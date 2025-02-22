using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public SpriteRenderer tilePrefab;
    public Vector2Int halfSize;

    private void Start()
    {
        for (int y = -halfSize.y; y <= +halfSize.y; y++)
        {
            for (int x = -halfSize.x; x <= +halfSize.x; x++)
            {
                Instantiate(tilePrefab, new(x, y), new(), transform);
            }
        }
    }
}

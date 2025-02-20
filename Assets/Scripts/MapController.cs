using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private SpriteRenderer tilePrefab;
    [SerializeField] private SpriteRenderer wallPrefab;

    private void Start()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Instantiate(tilePrefab, new(x, y), new(), transform);
            }
        }
    }
}

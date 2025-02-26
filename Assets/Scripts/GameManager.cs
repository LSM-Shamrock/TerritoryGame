using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance;
    #endregion

    [Header("Prefabs")]
    public GameObject enemyPrefab;

    [Header("Scene Objects")]
    public GameObject player;
    public Tilemap virusAreaTilemap;

    [Header("Game Variables")]
    public int playerLife = 5;
    public int enemyKillCount = 0;

    public BoundsInt mapBounds;

    private void Awake()
    {
        instance = this;
        mapBounds = virusAreaTilemap.cellBounds;
    }

    public void ChangeToPlayerArea(Vector3Int p1, Vector3Int p2)
    {
        var min = Vector3Int.Min(p1, p2);
        var max = Vector3Int.Max(p1, p2);
        for (int y = min.y; y <= max.y; y++)
        {
            for (int x = min.x; x <= max.x; x++)
            {
                virusAreaTilemap.SetTile(new(x, y), null);
            }
        }
    }
}

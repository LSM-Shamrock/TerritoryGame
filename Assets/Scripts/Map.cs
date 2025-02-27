using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField] Vector3Int pos1;
    [SerializeField] Vector3Int pos2;
    [SerializeField] TileBase playerTile;
    [SerializeField] TileBase virusTile;
    [SerializeField] TileBase wallTile;
    Tilemap tilemap;

    public Vector3Int Min => Vector3Int.Min(pos1, pos2);
    public Vector3Int Max => Vector3Int.Max(pos1, pos2);

    public HashSet<Vector3Int> MapArea { get; private set; } = new();

    public HashSet<Vector3Int> PlayerArea => new(MapArea.Where((p) => tilemap.GetTile(p) == playerTile));

    public HashSet<Vector3Int> VirusArea => new(MapArea.Where((p) => tilemap.GetTile(p) == virusTile));

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        SetupTiles();
    }

    private void SetupTiles()
    {
        for (int y = Min.y - 1; y <= Max.y + 1; y++)
        {
            for (int x = Min.x - 1; x <= Max.x + 1; x++)
            {
                var p = new Vector3Int(x, y);
                if (Min.x <= x && x <= Max.x && Min.y <= y && y <= Max.y)
                {
                    MapArea.Add(p);
                    tilemap.SetTile(p, virusTile);
                }
                else
                {
                    tilemap.SetTile(p, wallTile);
                }
            }       
        }
    }

    public void FillPlayerArea(Vector3Int p1, Vector3Int p2)
    {
        var min = Vector3Int.Min(p1, p2);
        var max = Vector3Int.Max(p1, p2);
        for (int y = min.y; y <= max.y; y++)
        {
            for (int x = min.x; x <= max.x; x++)
            {
                var p = new Vector3Int(x, y);
                if (MapArea.Contains(p))
                {
                    tilemap.SetTile(p, playerTile);
                }
            }
        }
    }
}

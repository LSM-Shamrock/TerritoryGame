using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField] Vector2Int size;
    [SerializeField] TileBase tile;
    [SerializeField] Tilemap playerTilemap;
    [SerializeField] Tilemap virusTilemap;
    [SerializeField] Tilemap wallTilemap;
    [SerializeField] TileBase obstacleTile;
    [SerializeField] Tilemap obstacleTilemap;


    public Vector3Int Min => Vector3Int.Min(Vector3Int.zero, (Vector3Int)size);
    public Vector3Int Max => Vector3Int.Max(Vector3Int.zero, (Vector3Int)size);

    public HashSet<Vector3Int> MapArea
    {
        get
        {
            var result = new HashSet<Vector3Int>();
            var bounds = new BoundsInt(Min, Max - Min + Vector3Int.one);
            foreach (var p in bounds.allPositionsWithin) result.Add(p);
            return result;
        }
    }
    public HashSet<Vector3Int> PlayerArea => new(MapArea.Where(p => playerTilemap.HasTile(p)));
    public HashSet<Vector3Int> VirusArea => new(MapArea.Where(p => virusTilemap.HasTile(p)));
    public HashSet<Vector3Int> ObstacleArea => new(MapArea.Where(p => obstacleTilemap.HasTile(p)));

    private void Awake()
    {
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
                    virusTilemap.SetTile(p, tile);
                }
                else
                {
                    wallTilemap.SetTile(p, tile);
                }
            }       
        }
    }
    public void FillPlayerArea(Vector3Int p1, Vector3Int p2)
    {
        var min = Vector3Int.Min(p1, p2);
        var max = Vector3Int.Max(p1, p2);
        var bounds = new BoundsInt(min, max - min + Vector3Int.one);
        var fillPos = MapArea.Where((p) => bounds.Contains(p) && !PlayerArea.Contains(p)).ToHashSet();
        if (fillPos.Count > 0)
        {
            foreach (var p in fillPos)
            {
                virusTilemap.SetTile(p, null);
                playerTilemap.SetTile(p, tile);
            }
            Debug.Log(fillPos.Count + "타일 채움");
            RandomItemSpawn();
        }
    }




    [SerializeField] GameObject itemPrefab;
    List<ItemType> items = new()
    {
        ItemType.Speed,
        ItemType.Speed,
        ItemType.Speed,
        ItemType.Defense,
        ItemType.Defense,
        ItemType.Defense,
        ItemType.Invincibility,
        ItemType.Life,
        ItemType.Life,
        ItemType.Random,
        ItemType.Random,
        ItemType.Random,
        ItemType.Random,
        ItemType.Random,
    };
    private Dictionary<Vector3Int, GameObject> itemDict = new();
    private void RandomItemSpawn()
    {
        var possiblePos = MapArea.Where(p => !itemDict.ContainsKey(p) || itemDict[p] == null).ToHashSet();
        if (possiblePos.Count > 0 && items.Count > 0)
        {
            var pos = possiblePos.ToArray()[Random.Range(0, possiblePos.Count)];

            var itemType = items[Random.Range(0, items.Count)];
            items.Remove(itemType);

            var go = Instantiate(itemPrefab, pos, Quaternion.identity);
            go.GetComponent<Item>().Type = itemType;
            itemDict[pos] = go;

            Debug.Log($"{itemType} 아이템 생성됨");
        }
    }
}

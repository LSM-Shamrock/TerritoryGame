using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField] Vector3Int pos1;
    [SerializeField] Vector3Int pos2;
    [SerializeField] TileBase tile;
    [SerializeField] Tilemap playerTilemap;
    [SerializeField] Tilemap virusTilemap;
    [SerializeField] Tilemap wallTilemap;


    public Vector3Int Min => Vector3Int.Min(pos1, pos2);
    public Vector3Int Max => Vector3Int.Max(pos1, pos2);

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

    public HashSet<Vector3Int> PlayerArea => new(MapArea.Where((p) => playerTilemap.HasTile(p)));

    public HashSet<Vector3Int> VirusArea => new(MapArea.Where((p) => virusTilemap.HasTile(p)));

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
            RandomItemSpawn();
        }
    }



    [SerializeField] int tem1Cnt = 3;
    [SerializeField] int tem2Cnt = 3;
    [SerializeField] int tem3Cnt = 1;
    [SerializeField] int tem4Cnt = 2;
    [SerializeField] int tem5Cnt = 5;
    [SerializeField] GameObject tem1Prefab;
    [SerializeField] GameObject tem2Prefab;
    [SerializeField] GameObject tem3Prefab;
    [SerializeField] GameObject tem4Prefab;
    [SerializeField] GameObject tem5Prefab;
    private Dictionary<Vector3Int, GameObject> itemDict = new();
    private void RandomItemSpawn()
    {
        var possiblePos = MapArea;
        possiblePos.RemoveWhere(p => itemDict.ContainsKey(p) && itemDict[p] != null);

        var temPrefabs = new List<GameObject>();
        if(tem1Prefab != null) for (int i = 0; i < tem1Cnt; i++) temPrefabs.Add(tem1Prefab);
        if(tem1Prefab != null) for (int i = 0; i < tem2Cnt; i++) temPrefabs.Add(tem2Prefab);
        if(tem1Prefab != null) for (int i = 0; i < tem3Cnt; i++) temPrefabs.Add(tem3Prefab);
        if(tem1Prefab != null) for (int i = 0; i < tem4Cnt; i++) temPrefabs.Add(tem4Prefab);
        if(tem1Prefab != null) for (int i = 0; i < tem5Cnt; i++) temPrefabs.Add(tem5Prefab);

        if (possiblePos.Count > 0 && temPrefabs.Count > 0)
        {
            var pos = possiblePos.ToArray()[Random.Range(0, possiblePos.Count)];
            var prefab = temPrefabs[Random.Range(0, temPrefabs.Count)];
            if (prefab == tem1Prefab) tem1Cnt--;
            if (prefab == tem2Prefab) tem2Cnt--;
            if (prefab == tem3Prefab) tem3Cnt--;
            if (prefab == tem4Prefab) tem4Cnt--;
            if (prefab == tem5Prefab) tem5Cnt--;
            itemDict[pos] = Instantiate(prefab, pos, Quaternion.identity);
            Debug.Log($"{prefab.name} »ý¼ºµÊ");
        }
    }
}

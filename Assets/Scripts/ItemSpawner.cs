using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnData
{
    public GameObject itemPrefab;
    public int remainingCount;
}

public class ItemSpawner : MonoBehaviour
{
    public List<ItemSpawnData> spawnDatas;


}

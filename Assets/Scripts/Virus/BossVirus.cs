using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossVirus : Virus
{
    public GameObject[] virusPrefabs;
    public int startSpawnCount = 5;
    public float spawnTime = 10f;

    private List<Virus> Viruses => new(FindObjectsOfType<Virus>());
    private HashSet<Vector3Int> SpawnableArea => FindObjectOfType<Map>().VirusArea;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(RepeatSpawn());
    }

    private IEnumerator RepeatSpawn()
    {
        for (int i = 0; i < startSpawnCount; i++)
        {
            RandomVirusSpawn();
        }
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            if (Viruses.Count < startSpawnCount)
            {
                for (int i = 0; i < startSpawnCount; i++)
                {
                    RandomVirusSpawn();
                }
            }
        }
    }

    private void RandomVirusSpawn()
    {
        if (SpawnableArea.Count > 0 && virusPrefabs.Length > 0)
        {
            var prefab = virusPrefabs[Random.Range(0, virusPrefabs.Length)];
            var pos = SpawnableArea.ToArray()[Random.Range(0, SpawnableArea.Count)];
            var rotation = Quaternion.identity;
            Instantiate(prefab, pos, rotation, transform.parent);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossVirus : Virus
{
    public GameObject[] virusPrefabs;
    public int startVirusCount = 2;
    public float refillTime = 3f;

    private List<Virus> Viruses => new(FindObjectsOfType<Virus>());
    private HashSet<Vector3Int> SpawnableArea => FindObjectOfType<Map>().VirusArea;

    protected override void Start()
    {
        base.Start();
        MoveToCenter();
        StartCoroutine(RepeatSpawn());
    }

    private void MoveToCenter()
    {
        var map = FindObjectOfType<Map>();
        var center = (Vector3)(map.Min + map.Max) / 2f;
        transform.position = center;
    }

    private IEnumerator RepeatSpawn()
    {
        for (int i = 0; i < startVirusCount; i++)
        {
            RandomVirusSpawn();
        }
        while (true)
        {
            yield return new WaitForSeconds(refillTime);
            if (Viruses.Count < startVirusCount)
            {
                for (int i = 0; i < startVirusCount; i++)
                {
                    RandomVirusSpawn();
                }
            }
        }
    }

    private void RandomVirusSpawn()
    {
        if (virusPrefabs.Length > 0)
        {
            var prefab = virusPrefabs[Random.Range(0, virusPrefabs.Length)];

            var spawnAblePos = new[]
            {
                transform.position + Vector3.up,
                transform.position + Vector3.down,
                transform.position + Vector3.left,
                transform.position + Vector3.right,
            }.Where(p => SpawnableArea.Contains(Vector3Int.RoundToInt(p))).ToArray();

            if (spawnAblePos.Length > 0 )
            {
                var pos = spawnAblePos[Random.Range(0, spawnAblePos.Length)];
                Instantiate(prefab, pos, Quaternion.identity, transform.parent);
                Debug.Log(prefab.name + "바이러스 스폰됨");
            }
        }
    }
}

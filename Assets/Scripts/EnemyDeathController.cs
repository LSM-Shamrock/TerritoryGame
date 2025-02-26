using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathController : MonoBehaviour
{
    private void Dead()
    {
        GameManager.instance.enemyKillCount++;
        Destroy(gameObject);
    }

    private void Update()
    {
        var virusAreaTilemap = GameManager.instance.virusAreaTilemap;
        var roundPos = Vector3Int.RoundToInt(transform.position);
        if (!virusAreaTilemap.HasTile(roundPos))
        {
            Dead();
        }
    }
}

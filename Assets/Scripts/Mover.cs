using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float moveSpeed = 8f;

    public Vector3Int endPos;
    
    public float RemainingDist => Vector3.Distance(transform.position, endPos);

    public void Move()
    {
        var roundPos = Vector3Int.RoundToInt(transform.position);
        var map = FindObjectOfType<Map>();
        if (roundPos.x != endPos.x && roundPos.y != endPos.y ||
            !map.MapArea.Contains(endPos))
        {
            endPos = roundPos;
        } 
        else
        {
            var moveAmount = Mathf.Min(RemainingDist, moveSpeed * Time.deltaTime);
            var moveDir = (endPos - transform.position).normalized;
            transform.position += moveDir * moveAmount;
        }
    }
}

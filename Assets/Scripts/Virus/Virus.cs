using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Virus : MonoBehaviour
{
    public float moveSpeed = 8f;

    private Vector3Int RoundPos => Vector3Int.RoundToInt(transform.position);
    private HashSet<Vector3Int> MoveableArea => FindObjectOfType<Map>().VirusArea;
     
    virtual protected void Start()
    {
        StartCoroutine(RepeatMovement());
    }
    virtual protected void Update()
    {
        UpdateMove();
        UpdateDead();
    }

    virtual public void Dead()
    {
        Destroy(gameObject);
    }
    private void UpdateDead()
    {
        var p = Vector3Int.RoundToInt(transform.position);
        if (!MoveableArea.Contains(p))
        {
            Dead();
        }
    }





    Vector3 moveDir;
    float remainingDist;
    virtual protected IEnumerator RepeatMovement()
    {
        while (true)
        {
            var angle = Random.Range(0f, 360f);
            var x = Mathf.Cos(angle * Mathf.Rad2Deg);
            var y = Mathf.Sin(angle * Mathf.Rad2Deg);
            moveDir = new(x, y);
            remainingDist = Random.Range(1f, 4f);
            yield return new WaitWhile(() => remainingDist > 0f);

            var waitTime = Random.Range(0f, 1f);
            yield return new WaitForSeconds(waitTime);
        }
    }
    virtual protected void FlipDir()
    {
        moveDir *= -1f;
    }
    private void UpdateMove()
    {
        if (MoveableArea.Contains(Vector3Int.RoundToInt(transform.position + moveDir)))
        {
            var moveAmount = Mathf.Min(1, moveSpeed * Time.deltaTime);
            transform.position += moveDir * moveAmount;
            remainingDist -= moveAmount;
        }
        else
        {
            if (MoveableArea.Contains(Vector3Int.RoundToInt(transform.position - moveDir)))
            {
                FlipDir();
            }
            else
            {
                moveDir = Vector3.zero;
            }
        }
    }


    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        var playerTrail = collision.GetComponent<PlayerTrail>();
        if (playerTrail != null)
        {
            FlipDir();
        }
    }
}

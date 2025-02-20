using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Vector3 inputDir = Vector3.right;
    private Vector3 moveDir = Vector3.right;
    private float remainingDist;
    private float moveSpeed = 2f;

    private List<Vector3Int> points = new();
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        InputUpdate();
        MoveUpdtae();
        AddPoint();
        DrawLine();
    }

    private void InputUpdate()
    {
        var inputVec = Vector3.zero;
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        if (inputVec == Vector3.up || inputVec == Vector3.down ||
            inputVec == Vector3.right || inputVec == Vector3.left)
        {
            inputDir = inputVec;
        }
    }

    private void MoveUpdtae()
    {
        if (remainingDist == 0f)
        {
            moveDir = inputDir;
            remainingDist = 1f;
        }
        transform.position += moveDir * Mathf.Min(remainingDist, moveSpeed * Time.deltaTime);
        remainingDist -= Mathf.Min(remainingDist, moveSpeed * Time.deltaTime);
    }

    private void AddPoint()
    {
        var roundPos = Vector3Int.RoundToInt(transform.position);
        if (points.Contains(roundPos))
        {
            var removeIndex = points.IndexOf(roundPos);
            var removeCount = points.Count - removeIndex;
            points.RemoveRange(removeIndex, removeCount);
        }
        points.Add(roundPos);
    }

    private void DrawLine()
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }
}

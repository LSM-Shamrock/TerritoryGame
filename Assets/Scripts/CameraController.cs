using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget; 
    private Vector3 offset = Vector3.back * 10f; 
    private float moveSpeed = 10f;

    public void Update()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.position + offset, Time.deltaTime * moveSpeed);
    }
}

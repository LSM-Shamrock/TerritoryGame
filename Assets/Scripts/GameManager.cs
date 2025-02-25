using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public Tilemap wallTilemap;
    public Tilemap virusAreaTilemap;

    private void Awake()
    {
        instance = this;
    }
}

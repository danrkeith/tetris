using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tetromino ActiveMino { get; private set; }

    private void Awake()
    {
        SetActiveMino();
    }

    public void SpawnMino()
    {
        Debug.Log("SpawnNewMino");
    }

    private void SetActiveMino()
    {
        ActiveMino = GameObject.FindWithTag(Tetromino.Active).GetComponent<Tetromino>();
    }
}

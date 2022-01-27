using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] tetrominoes;
    private List<int> _minoQueue;

    public Tetromino ActiveMino { get; private set; }

    private void Awake()
    {
        ResetMinoQueue();

        SpawnMino();
    }

    public void SpawnMino()
    {
        int i = Random.Range(0, _minoQueue.Count);
        Instantiate(tetrominoes[_minoQueue[i]]);

        _minoQueue.RemoveAt(i);

        // Refill queue if necessary
        if (_minoQueue.Count == 0)
        {
            ResetMinoQueue();
        }

        SetActiveMino();
    }

    private void SetActiveMino()
    {
        ActiveMino = GameObject.FindWithTag(Tetromino.Active).GetComponent<Tetromino>();
    }

    private void ResetMinoQueue()
    {
        _minoQueue = new List<int>();
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            _minoQueue.Add(i);
        }
    }
}

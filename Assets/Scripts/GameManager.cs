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
        //int i = Random.Range(0, _minoQueue.Count);
        int i = 1;
        ActiveMino = Instantiate(tetrominoes[_minoQueue[i]]).GetComponent<Tetromino>();

        _minoQueue.RemoveAt(i);

        // Refill queue if necessary
        if (_minoQueue.Count == 0)
        {
            ResetMinoQueue();
        }
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

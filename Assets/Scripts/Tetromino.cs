using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public const string Active = "Active";
    private const int DASFrames = 16;
    private const int ASFrames = 2;

    private GameManager _gameManager;
    private int _fallFrames = 48;
    private int _xVelocity = 0;

    private List<Block> _blocks;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _blocks = new List<Block>();

        foreach (Transform child in transform)
        {
            _blocks.Add(child.GetComponent<Block>());
        }
    }

    private void Start()
    {
        StartCoroutine(nameof(Fall));
    }

    public void Rotate(float angle)
    {
        transform.Rotate(0, 0, angle);
        foreach (Transform child in transform)
        {
            child.Rotate(0, 0, -angle);
        }
    }

    public void StartMovement(float newXVelocity)
    {
        _xVelocity = (int)newXVelocity;

        if (_xVelocity != 0)
        {
            StartCoroutine(nameof(AutoShift));
        }
    }

    private IEnumerator AutoShift()
    {
        int coroutineXVelocity = _xVelocity;

        void Move() => transform.Translate(new Vector2(coroutineXVelocity * Block.Size, 0), Space.World);

        // Initial movement
        Move();
        yield return new WaitForSeconds(DASFrames / 60f);

        // Autoshift until velocity has changed
        while (coroutineXVelocity == _xVelocity)
        {
            Move();
            yield return new WaitForSeconds(ASFrames / 60f);
        }
    }
    
    private IEnumerator Fall()
    {
        while (true)
        {
            foreach (Block block in _blocks)
            {
                if (block.CheckForBlock(Vector2.down))
                {
                    // Block has landed
                    Debug.Log("Land");
                    tag = "Untagged";
                    StopAllCoroutines();
                    _gameManager.SpawnMino();
                    yield break;
                }
            }

            transform.Translate(Vector2.down * Block.Size, Space.World);

            yield return new WaitForSeconds(_fallFrames / 60f);
        }
    }
}

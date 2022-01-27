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
    private bool _softDrop = false;

    public bool SoftDrop
    {
        get => _softDrop;
        set
        {
            _softDrop = value;
            StopCoroutine(nameof(Fall));
            StartCoroutine(nameof(Fall));
        }
    }

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

    public bool Move(Vector2 dir)
    {
        // Ensure no blocks are in mino's way
        foreach (Block block in _blocks)
        {
            if (block.CheckForBlock(dir))
            {
                return false;
            }
        }

        // Move tetronimo
        transform.Translate(dir * Block.Size, Space.World);
        return true;
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

    public void HardDrop()
    {
        while (Move(Vector2.down)) { }
        Deactivate();
    }

    private void Deactivate()
    {
        Debug.Log("Deactivated");
        StopAllCoroutines();

        tag = "Untagged";
        foreach (Transform child in transform)
        {
            child.tag = "Untagged";
        }

        _gameManager.SpawnMino();
    }

    private IEnumerator AutoShift()
    {
        int coroutineXVelocity = _xVelocity;

        // Initial movement
        Move(Vector2.right * coroutineXVelocity);
        yield return new WaitForSeconds(DASFrames / 60f);

        // Autoshift until velocity has changed
        while (coroutineXVelocity == _xVelocity)
        {
            Move(Vector2.right * coroutineXVelocity);
            yield return new WaitForSeconds(ASFrames / 60f);
        }
    }

    private IEnumerator Fall()
    {
        while (true)
        {
            if (!Move(Vector2.down))
            {
                Deactivate();
                yield break;
            }

            yield return new WaitForSeconds((_softDrop ? 2 : _fallFrames) / 60f);
        }
    }
}

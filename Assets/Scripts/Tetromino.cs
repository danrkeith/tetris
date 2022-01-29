using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public const string Active = "Active";
    private const int _dasFrames = 16;
    private const int _asFrames = 2;

    private static readonly Vector2[] _wallKicksReg = new Vector2[]
    {
        new Vector2(0,0), 
        new Vector2(-1, 0),
        new Vector2(-1, 1),
        new Vector2(0, -2),
        new Vector2(-1, -2)
    };

    private static readonly Vector2[] _wallKicksI = new Vector2[]
    {
        new Vector2(0,0),
        new Vector2(-2, 0),
        new Vector2(1, 0),
        new Vector2(-2, -1),
        new Vector2(1, 2)
    };

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
        // TODO: Wall Kick

        // Rotate
        transform.Rotate(0, 0, angle);

        // Reorientate block sprites
        foreach (Transform child in transform)
        {
            child.Rotate(0, 0, -angle);
        }
    }

    public void StartAutoShift(float newXVelocity)
    {
        // Set mino's current velocity
        _xVelocity = (int)newXVelocity;

        if (_xVelocity != 0)
        {
            StartCoroutine(nameof(AutoShift));
        }
    }

    public void HardDrop()
    {
        // Move mino down until it can't be moved down
        while (Move(Vector2.down)) { }
        Deactivate();
    }

    private void Deactivate()
    {
        // TODO: Check for line clears

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

        // Autoshift until mino's velocity has changed
        while (coroutineXVelocity == _xVelocity)
        {
            Move(Vector2.right * coroutineXVelocity);
            yield return new WaitForSeconds(_asFrames / 60f);
        }
    }

    private IEnumerator Fall()
    {
        while (true)
        {
            // Fall faster if soft-dropping, otherwise fall at the specified speed
            yield return new WaitForSeconds((_softDrop ? 2 : _fallFrames) / 60f);

            // Move down, otherwise it has landed
            if (!Move(Vector2.down))
            {
                Deactivate();
                yield break;
            }
        }
    }
}

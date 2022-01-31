using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public const string Active = "Active";
    private const int _dasFrames = 16;
    private const int _asFrames = 2;
    private const int _kickCount = 5;

    public bool I = false;
    public bool O = false;

    private static readonly Vector2[][] _wallKicksReg = new Vector2[][]
    {
        new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(-1, 0),
            new Vector2(-1, 1),
            new Vector2(0, -2),
            new Vector2(-1, -2)
        },
        new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(1, 0),
            new Vector2(1, -1),
            new Vector2(0, 2),
            new Vector2(1, 2)
        },
        new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, -2),
            new Vector2(1, -2)
        },
        new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(-1, 0),
            new Vector2(-1, -1),
            new Vector2(0, 2),
            new Vector2(-1, 2)
        }
    };

    private static readonly Vector2[][] _wallKicksI = new Vector2[][]
    {
        new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(-2, 0),
            new Vector2(1, 0),
            new Vector2(-2, -1),
            new Vector2(1, 2)
        },
        new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(-1, 0),
            new Vector2(2, 0),
            new Vector2(-1, 2),
            new Vector2(2, -1)
        },
        new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(2, 0),
            new Vector2(-1, 0),
            new Vector2(2, 1),
            new Vector2(-1, -2)
        },
        new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(1, 0),
            new Vector2(-2, 0),
            new Vector2(1, -2),
            new Vector2(-2, 1)
        },
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
        // O piece doesn't rotate
        if (O)
        {
            return;
        }

        // Wall Kick
        Vector2[] kicks = new Vector2[_kickCount];

        // Find which set of kicks to use based on rotation taking place
        int kickIndex;
        int kickMod;
        if (angle == -90)
        {
            // Regular rotation
            kickMod = 1;
            kickIndex = (int)-transform.rotation.eulerAngles.z / 90;
            if (kickIndex < 0)
            {
                kickIndex += 4;
            }
        }
        else if (angle == 90)
        {
            // Anticlockwise rotation; go to previous rotation and make negative
            kickMod = -1;
            kickIndex = (int)-transform.rotation.eulerAngles.z / 90 - 1;
            if (kickIndex < 0)
            {
                kickIndex += 4;
            }
        }
        else
        {
            throw new System.Exception("Attempting non-90degree rotation");
        }

        // Choose correct kick array for piece
        if (I)
        {
            Array.Copy(_wallKicksI[kickIndex], kicks, _kickCount);
        }
        else
        {
            Array.Copy(_wallKicksReg[kickIndex], kicks, _kickCount);
        }

        // Apply modification to each item in kicks
        for (int i = 0; i < kicks.Length; i++)
        {
            kicks[i] *= kickMod;
        }

        Vector2? kick = null;

        // Check each kick. If any are successful on all blocks, use that kick.
        foreach (Vector2 k in kicks)
        {
            // Check all blocks. If any fail, move on to next kick.
            bool success = true;

            foreach (Block block in _blocks)
            {
                //Debug.Log(k + " outputs " + block.CheckKickForBlock(angle, k));

                if (block.CheckKickForBlock(angle, k))
                {
                    success = false;
                    break;
                }
            }

            if (success)
            {
                kick = k;
                break;
            }
        }

        // If no successful kick was found, don't rotate
        if (kick == null)
        {
            Debug.Log("Rotation Failed");
            return;
        }

        Debug.Log("Applying " + kick + " Kick");

        // Apply kick
        transform.Translate((Vector2)kick * Block.Size, Space.World);

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

        enabled = false;
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

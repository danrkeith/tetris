using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public const string ACTIVE = "Active";
    private const int DASFRAMES = 16;
    private const int ASFRAMES = 2;

    private int fallFrames = 48;
    private int xVelocity = 0;

    private List<Block> blocks;

    private void Start()
    {
        blocks = new List<Block>();

        foreach (Transform child in transform)
        {
            blocks.Add(child.GetComponent<Block>());
        }

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
        xVelocity = (int)newXVelocity;

        if (xVelocity != 0)
        {
            StartCoroutine(nameof(AutoShift));
        }
    }

    private IEnumerator AutoShift()
    {
        int coroutineXVelocity = xVelocity;

        void Move() => transform.Translate(new Vector2(coroutineXVelocity * Block.SIZE, 0), Space.World);

        // Initial movement
        Move();
        yield return new WaitForSeconds(DASFRAMES / 60f);

        // Autoshift until velocity has changed
        while (coroutineXVelocity == xVelocity)
        {
            Move();
            yield return new WaitForSeconds(ASFRAMES / 60f);
        }
    }
    
    private IEnumerator Fall()
    {
        while (true)
        {
            transform.Translate(new Vector2(0, -Block.SIZE), Space.World);

            foreach (Block block in blocks)
            {
                if (block.CheckForBlock(Vector2.down))
                {
                    enabled = false;
                    yield break;
                }
            }

            yield return new WaitForSeconds(fallFrames / 60f);
        }
    }
}

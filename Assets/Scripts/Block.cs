using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public const float SIZE = 0.4f;
    public bool CheckForBlock(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Block.SIZE);

        Debug.DrawRay(transform.position, direction * Block.SIZE, Color.red, 48/60f);

        return hit.collider != null && !hit.collider.CompareTag(Tetromino.ACTIVE);
    }
}

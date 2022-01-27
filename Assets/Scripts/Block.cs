using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public const float Size = 0.4f;

    public bool CheckForBlock(Vector2 direction)
    {
        float length = Size * 2 / 3;
        Vector2 origin = (Vector2)transform.position + (direction * length);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, length);

        Debug.DrawRay(origin, direction * length, Color.red, 48/60f);

        return hit.collider != null && !hit.collider.CompareTag(Tetromino.Active);
    }
}

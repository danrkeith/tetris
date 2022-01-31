using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public const float Size = 0.4f;

    public bool CheckForBlock(Vector2 relativeGridPos)
    {
        Vector2 realPos = (Vector2)transform.position + relativeGridPos * Size;
        Collider2D collider = Physics2D.OverlapBox(realPos, Vector2.one * Size / 2, 0);

        Debug.DrawRay(realPos, Vector2.up, Color.blue, 1);

        return collider != null && !collider.CompareTag(Tetromino.Active);
    }

    public bool CheckKickForBlock(float rotation, Vector2 kick)
    {
        Vector2 realRelativePos = transform.position - transform.parent.position;
        Vector2 newRealRelativePos = Quaternion.AngleAxis(rotation, Vector3.forward) * realRelativePos;
        Vector2 realDisplacement = newRealRelativePos - realRelativePos;
        Vector2 gridDisplacement = realDisplacement * (1 / Size);

        return CheckForBlock(gridDisplacement + kick);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Sprite[] frames;
    private SpriteRenderer sr;
    private int frameIndex = 0;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Rotate(int value)
    {
        transform.Rotate(0, 0, value * 90);

        frameIndex += value;

        // Loop index
        switch (frameIndex)
        {
            case -1:
                frameIndex = 3;
                break;
            case 4:
                frameIndex = 0;
                break;
            default:
                break;
        }
        sr.sprite = frames[frameIndex];
    }
}
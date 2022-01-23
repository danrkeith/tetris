using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInput : MonoBehaviour
{
    const int DASFRAMES = 16;
    const int ASFRAMES = 2;

    private Tetromino activeMino;
    private int dir = 0;

    private void Awake()
    {
        // Initiate components
        activeMino = GameObject.FindWithTag("Active").GetComponent<Tetromino>();
    }

    public void OnRotate(InputValue value)
    {
        activeMino.Rotate((int)value.Get<float>());
    }
    
    public void OnMove(InputValue value)
    {
        dir = (int)value.Get<float>();

        if (dir != 0)
        {
            StartCoroutine(Move(dir));
        }
    }

    private IEnumerator Move(int value)
    {
        // Initial movement w/ DAS
        activeMino.transform.Translate(new Vector2(value * 0.4f, 0), Space.World);
        yield return new WaitForSeconds(DASFRAMES / 60f);

        // Held movement after, check for release
        while (dir == value)
        {
            activeMino.transform.Translate(new Vector2(value * 0.4f, 0), Space.World);
            yield return new WaitForSeconds(ASFRAMES / 60f);
        }
    }
}

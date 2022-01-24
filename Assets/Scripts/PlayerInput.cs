using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInput : MonoBehaviour
{
    private Tetromino activeMino;

    private void Awake()
    {
        // Find active tetromino
        activeMino = GameObject.FindWithTag("Active").GetComponent<Tetromino>();
    }

    private void OnRotate(InputValue value)
    {
        activeMino.Rotate(value.Get<float>() * 90);
    }
    
    private void OnMove(InputValue value)
    {
        activeMino.StartMovement(value.Get<float>());
    }
}

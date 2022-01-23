using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInput : MonoBehaviour
{
    private Tetromino activeTet;

    private void Awake()
    {
        // Initiate components
        GameObject activePiece = GameObject.FindWithTag("Active");
        activeTet = activePiece.GetComponent<Tetromino>();
    }

    public void OnRotate(InputValue value)
    {
        activeTet.Rotate((int)value.Get<float>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInput : MonoBehaviour
{
    private Tetromino activeTet;
    private GameObject activeShadow;

    private void Awake()
    {
        // Initiate components
        GameObject activePiece = GameObject.FindWithTag("Active");
        activeTet = activePiece.GetComponent<Tetromino>();
        activeShadow = GameObject.FindWithTag("Shadow");
    }

    public void OnRotate(InputValue value)
    {
        activeTet.Rotate((int)value.Get<float>());
        activeShadow.transform.Rotate(0, 0, (int)value.Get<float>() * 90);
    }
}

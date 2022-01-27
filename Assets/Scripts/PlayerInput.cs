using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInput : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnRotate(InputValue value)
    {
        _gameManager.ActiveMino.Rotate(value.Get<float>() * 90);
    }
    
    private void OnMove(InputValue value)
    {
        _gameManager.ActiveMino.StartMovement(value.Get<float>());
    }
}

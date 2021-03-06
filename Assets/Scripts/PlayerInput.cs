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
        _gameManager.ActiveMino.Move(Vector2.right * value.Get<float>());
    }

    private void OnAutoShift(InputValue value)
    {
        _gameManager.ActiveMino.StartAutoShift(value.Get<float>());
    }

    private void OnSoftDrop(InputValue value)
    {
        _gameManager.ActiveMino.SoftDrop = value.Get<float>() == 1;
    }

    private void OnHardDrop()
    {
        _gameManager.ActiveMino.HardDrop();
    }

    private void OnHold()
    {
        // TODO: Implement hold interactivity
        Debug.Log("Hold");
    }
}

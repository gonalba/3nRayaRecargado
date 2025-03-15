using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction clickAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        clickAction = playerInput.actions["Click"];
    }

    private void OnEnable()
    {
        clickAction.performed += OnClick;
    }

    private void OnDisable()
    {
        clickAction.performed -= OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Casilla casilla = hit.collider.GetComponent<Casilla>();
            //if (casilla != null)
            //{
            //    //casilla.OnClick();
            //}
        }
    }
}
using AEA;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private InputSystem_Actions _action;

    public Vector2 MoveDirection { get; private set; }
    public event Action<Vector2> OnMoveEvent;
    public event Action OnJumpEvent;

    private void OnEnable()
    {
        if (_action == null)
        {
            _action = new InputSystem_Actions();
            _action.Player.SetCallbacks(this);
        }

        _action.Enable();
        _action.Player.Enable();

        MoveDirection = Vector2.zero;
    }

    private void OnDisable()
    {
        _action.Disable();
        _action.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MoveDirection = context.ReadValue<Vector2>();
            OnMoveEvent?.Invoke(MoveDirection);
        }

        if (context.canceled)
        {
            MoveDirection = Vector2.zero;
        }
    }
    public void OnAttack_1(InputAction.CallbackContext context)
    {
        
    }

    public void OnAttack_2(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpEvent?.Invoke();
        }
    }
}

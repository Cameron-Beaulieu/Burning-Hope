using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Entity))]
public class PlayerInput : MonoBehaviour
{
    private Entity entity;
    private Vector2 prevMoveState;
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 currentMoveState = context.ReadValue<Vector2>();
        entity.SetMovementInput(currentMoveState);

        if (currentMoveState.y < -0.5 && prevMoveState.y > -0.5)
        {
            entity.OnFallInputDown();
        }
        else if (currentMoveState.y > -0.5 && prevMoveState.y < -0.5)
        {
            entity.OnFallInputUp();
        }

        prevMoveState = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            entity.OnJumpInputDown();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            entity.OnJumpInputUp();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        //OnFire is called three times (for "Started", "Performed", and "Cancelled"),
        //but only perform the action once
        if (context.phase == InputActionPhase.Performed)
        {
            entity.ThrowTorch();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("Dash event registered.");
        if (context.phase == InputActionPhase.Performed)
        {
            entity.OnActionInputDown();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public abstract class PlayerState : BaseState<PlayerStateMachine.EPlayerState>
{
    protected PlayerContext Context;

    float moveMultiplier = 1;
    public PlayerState(PlayerContext context,PlayerStateMachine.EPlayerState stateKey) : base(stateKey)
    {
        Context = context;
    }
    public void GroundCheck()
    {
        bool isGroundedLocal = Physics.SphereCast(Context.Transform.position, Context.Data.groundCheckRadius, Vector3.down, out RaycastHit info, Context.Data.groundCheckDistance, Context.GroundMask);
        bool isGroundedGlobal = Context.IsGrounded;
        if (isGroundedLocal && !isGroundedGlobal)
        {
            //Context.animation.Jump(false);
            Context.IsGrounded = true;
            return;
        }
        if (!isGroundedLocal && isGroundedGlobal)
        {
            //Context.animation.Jump();
            Context.IsGrounded = false;
            return;
        }
    }
    public void RotatePlayer()
    {
        float lookX = Context.Input.Look.MouseX.ReadValue<float>();
        float lookY = -Context.Input.Look.MouseY.ReadValue<float>();

        Context.CurrentLookX = Mathf.Lerp(Context.CurrentLookX, lookX, Context.Data.turnSmoothTime * Time.deltaTime);
        Context.CurrentLookY = Mathf.Lerp(Context.CurrentLookY, lookY, Context.Data.turnSmoothTime * Time.deltaTime);

        if (Context.CurrentLookX == 1)
        {
            Debug.Log("Yo");
        }


        Context.XRotation += Context.CurrentLookY * Context.Data.turnSpeed * Time.deltaTime;
        Context.XRotation = Mathf.Clamp(Context.XRotation, -90, 90);

        Context.Transform.Rotate(0f, Context.CurrentLookX * Context.Data.turnSpeed * Time.deltaTime, 0f);
        Context.Camera.transform.localEulerAngles = new Vector3(Context.XRotation, 0, 0);
    }
    public void MovePlayer()
    {
        float moveX = -Context.Input.Movement.Forward.ReadValue<float>();
        float moveZ = Context.Input.Movement.Right.ReadValue<float>();

        //if (!_controller.isGrounded) _context.IsJumping = true;
        Context.IsMoving = moveX != 0 || moveZ != 0;

        Context.CurrentMoveZ = Mathf.Lerp(Context.CurrentMoveZ, moveZ, Context.Data.moveSmoothTime * Time.deltaTime);
        Context.CurrentMoveX = Mathf.Lerp(Context.CurrentMoveX, moveX, Context.Data.moveSmoothTime * Time.deltaTime);


        Context.Velocity = Context.Transform.right * (Context.CurrentMoveZ * moveMultiplier) - Context.Transform.forward * (Context.CurrentMoveX * moveMultiplier);
        if (!Context.IsJumping)
        {
            Context.CurrentGravity = Context.Data.groundGravity;
            float y = Context.Velocity.y + Context.CurrentGravity * Time.deltaTime;
            //velocity.y += Context.CurrentGravity * Time.deltaTime;
            Context.SetVelocityY(y);
        }
        else
        {
            float y = Mathf.Sqrt(Context.Data.jumpHeight * -2f * Context.Data.airGravity);
            Context.SetVelocityY(y);
        }
        Context.Controller.Move(Context.Velocity * Context.Data.moveSpeed * Time.deltaTime);
        float move = Context.IsMoving ? (Context.IsRunning ? 2 : 1) : 0;
        Context.Animation.Move(moveX * moveMultiplier, moveZ * moveMultiplier, move);

    }
    public void SetRun(bool isRun)
    {
        moveMultiplier = isRun ? Context.Data.runMultiplier : 1;
    }
    public void JumpPlayer()
    {
        Context.CurrentGravity = Context.Data.airGravity;
        float y = Context.Velocity.y + Context.CurrentGravity * Time.deltaTime;

        Context.SetVelocityY(y);
        Context.Controller.Move(Context.Velocity * Context.Data.moveSpeed * Time.deltaTime);
    }
    public void LockCamera()
    {
        Context.Camera.transform.position = Context.CameraPosition.position;
    }
}

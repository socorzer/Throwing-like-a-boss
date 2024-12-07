using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey) : base(context, stateKey)
    {
        Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Enter" + this);
    }
    public override void UpdateState()
    {
        //Debug.Log("Update" + this);
        GroundCheck();
        RotatePlayer();
        MovePlayer();
        LockCamera();
    }

    public override void ExitState()
    {
        //Debug.Log("Exit" + this);
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (Context.IsDead)
        {
            return PlayerStateMachine.EPlayerState.Dead;
        }
        else
        {
            if (Context.IsMoving && Context.IsRunning)
            {
                return PlayerStateMachine.EPlayerState.Run;
            }
            else if (!Context.IsMoving)
            {
                return PlayerStateMachine.EPlayerState.Idle;
            }
            else if (!Context.IsGrounded)
            {
                return PlayerStateMachine.EPlayerState.Jump;
            }
            else if (Context.IsFreeze)
            {
                return PlayerStateMachine.EPlayerState.Freese;
            }
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnTriggerExit(Collider other)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        //throw new System.NotImplementedException();
    }
}

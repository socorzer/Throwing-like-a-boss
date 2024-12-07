using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey) : base(context, stateKey)
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
        //MovePlayer();
        RotatePlayer();
        JumpPlayer();
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
            if (Context.IsGrounded)
            {
                if (!Context.IsMoving)
                {
                    return PlayerStateMachine.EPlayerState.Idle;
                }
                else
                {
                    if (!Context.IsRunning)
                    {
                        return PlayerStateMachine.EPlayerState.Walk;
                    }

                    else
                    {
                        return PlayerStateMachine.EPlayerState.Run;
                    }
                }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey) : base(context, stateKey)
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
    }

    public override void ExitState()
    {
        //Debug.Log("Exit" + this);
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (!Context.IsDead)
        {
            return PlayerStateMachine.EPlayerState.Idle;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }
}

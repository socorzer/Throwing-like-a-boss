using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowingState : PlayerState
{
    public PlayerThrowingState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey) : base(context, stateKey)
    {
        Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Enter" + this);
        EnterThrowing();
    }
    public override void UpdateState()
    {
        //Debug.Log("Update" + this);

    }

    public override void ExitState()
    {
        //Debug.Log("Exit" + this);
        ExitThrowing();
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (Context.IsEnd) return PlayerStateMachine.EPlayerState.End;

        else if (!Context.IsThrowing) return PlayerStateMachine.EPlayerState.Idle;
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

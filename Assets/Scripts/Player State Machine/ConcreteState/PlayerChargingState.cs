using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargingState : PlayerState
{
    public PlayerChargingState(PlayerContext context, PlayerStateMachine.EPlayerState stateKey) : base(context, stateKey)
    {
        Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Enter" + this);
        EnterCharging();
    }
    public override void UpdateState()
    {
        //Debug.Log("Update" + this);
        UpdateCharging();
    }

    public override void ExitState()
    {
        //Debug.Log("Exit" + this);
        ExitCharging();
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (Context.IsThrowing) return PlayerStateMachine.EPlayerState.Throwing;
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

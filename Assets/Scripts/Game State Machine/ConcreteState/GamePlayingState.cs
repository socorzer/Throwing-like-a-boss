using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayingState : GameState
{
    public GamePlayingState(GameContext context, GameStateMachine.EGameState stateKey) : base(context, stateKey)
    {
        Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Enter" + this);
        Context.IsPlaying = true;
    }
    public override void UpdateState()
    {
        //Debug.Log("Update" + this);

    }

    public override void ExitState()
    {
        //Debug.Log("Exit" + this);
    }

    public override GameStateMachine.EGameState GetNextState()
    {
       if(Context.IsEnd) return GameStateMachine.EGameState.End;
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

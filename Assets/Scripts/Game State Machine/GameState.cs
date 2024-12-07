using UnityEngine;

public abstract class GameState : BaseState<GameStateMachine.EGameState>
{
    protected GameContext Context;
    public GameState(GameContext context,GameStateMachine.EGameState stateKey) : base(stateKey)
    {
        Context = context;
    }

}

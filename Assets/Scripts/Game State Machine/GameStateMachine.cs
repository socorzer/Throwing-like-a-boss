
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using TMPro;

public class GameStateMachine : StateManager<GameStateMachine.EGameState>
{
    public enum EGameState
    {
        Setup,
        Preparing,
        Playing,
        End
    }
    GameContext _context;

    
    private void Awake()
    {
        ValidationConstrants();
        _context = new GameContext();
        InitializeStates();
    }
    private void ValidationConstrants()
    {

    }
    private void InitializeStates()
    {
        States.Add(EGameState.Setup, new GameSetupState(_context, EGameState.Setup));
        CurrentState = States[EGameState.Setup];
    }
}


using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

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
        States.Add(EGameState.Preparing, new GamePreparingState(_context, EGameState.Preparing));
        States.Add(EGameState.Playing, new GamePlayingState(_context, EGameState.Playing));
        States.Add(EGameState.End, new GameEndState(_context, EGameState.End));
        CurrentState = States[EGameState.Setup];
    }
    public void SetupPlayer(List<PlayerStateMachine> players)
    {
        _context.Players = players;
        _context.CurrentPlayer = _context.Players[1];
        _context.CurrentPlayer.SetPlayerTurn();
        _context.IsPlaying = true;
        UIManager.Instance.SetMarkerPosition(_context.CurrentPlayer.transform.position);

    }
    public void ChangePlayerTurn()
    {
        if (!_context.IsPlaying) return;
        _context.CurrentPlayer = _context.CurrentPlayer == _context.Players[0] ? _context.Players[1] : _context.Players[0];
        _context.CurrentPlayer.SetPlayerTurn();
        UIManager.Instance.SetMarkerPosition(_context.CurrentPlayer.transform.position);
    }
}

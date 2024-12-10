
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using System.Collections;

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

    bool isPlayerCharging;
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
        UIManager.Instance.SetMarkerPosition(_context.CurrentPlayer.transform.position);
        Invoke(nameof(SetPlayerHP),1);
    }
    public void SetPlayerHP()
    {
        foreach (var player in _context.Players)
        {
            player.SetPlayerHP();
        }
        SetAIType(0);
        _context.IsPlaying = true;
    }
    public void ChangePlayerTurn()
    {
        if (!_context.IsPlaying) return;
        _context.CurrentPlayer = _context.CurrentPlayer == _context.Players[0] ? _context.Players[1] : _context.Players[0];
        _context.CurrentPlayer.SetPlayerTurn();
        UIManager.Instance.SetMarkerPosition(_context.CurrentPlayer.transform.position);
    }
    public void SetPlayerCharge(bool isCharging)
    {
        if (!_context.IsPlaying || _context.CurrentPlayer.IsAI) return;
        isPlayerCharging = isCharging;
        if (isPlayerCharging) StartCoroutine(Charging());
    }
    IEnumerator Charging()
    {
        while (isPlayerCharging)
        {
            _context.CurrentPlayer.Charge();
            yield return new WaitForEndOfFrame();
        }

    }
    public void SetAIType(int index)
    {
        AIType type = (AIType)index;
        _context.Players[0].SetAI(type);
    }
}

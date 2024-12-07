
using System.Collections;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public enum EPlayerState
    {
        Idle,
        Charging,
        Throwing,
        Lose
    }
    PlayerContext _context;
    PlayerData _data;
    int _frameForCheckRelease;
    public bool IsYourTurn { get; private set; }
    
    private void Awake()
    {
        

        ValidationConstrants();
        _data = DataHandler.Instance.PlayerData;
        _context = new PlayerContext(_data, this.transform);
        InitializeStates();
    }
    private void ValidationConstrants()
    {

    }
    private void InitializeStates()
    {
        States.Add(EPlayerState.Idle, new PlayerIdleState(_context, EPlayerState.Idle));
        States.Add(EPlayerState.Charging, new PlayerChargingState(_context, EPlayerState.Charging));
        States.Add(EPlayerState.Throwing, new PlayerThrowingState(_context, EPlayerState.Throwing));
        States.Add(EPlayerState.Lose, new PlayerLoseState(_context, EPlayerState.Lose));
        CurrentState = States[EPlayerState.Idle];
    }
    public void SetIsYourTurn(bool isYourTurn)
    {
        IsYourTurn = isYourTurn;
    }
    private void OnMouseDrag()
    {
        //if (!IsYourTurn) return;
        _context.FrameForCheckDrag = 3;
        _context.IsCharging = true;
    }
}


using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public enum EPlayerState
    {
        Idle,
        Preparing,
        Throwing,
        Lose
    }
    PlayerContext _context;

    
    private void Awake()
    {
        

        ValidationConstrants();
        _context = new PlayerContext();
        InitializeStates();
    }
    private void ValidationConstrants()
    {
        Assert.IsNotNull(GetComponent<CharacterController>(), "CharacterController is not assigned.");
        Assert.IsNotNull(GetComponentInChildren<Camera>(), "Camera is not assigned.");
        Assert.IsNotNull(GetComponentInChildren<Animator>(), "Animator is not assigned.");
    }
    private void InitializeStates()
    {
        States.Add(EPlayerState.Idle, new PlayerIdleState(_context, EPlayerState.Idle));
        CurrentState = States[EPlayerState.Idle];
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public enum EPlayerState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Dead,
        Freese
    }
    PlayerContext _context;
    CharacterController _controller;
    Camera _camera;
    PlayerInput _input;

    float stamina = 0.5f;

    Vector3 velocity;

    [SerializeField] Transform _cameraPosition;
    [SerializeField] TextMeshProUGUI interactionText;
    [SerializeField] AnimationController _anim;
    public bool isDead;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask FreezeMask;

    public float Sensitivity { set { _context.Data.turnSpeed = value; } }
    public float TurnSmooth { set { _context.Data.turnSmoothTime = value; } }
    
    private void Awake()
    {
        

        ValidationConstrants();
        _input = GameData.Instance.PlayerInput;
        _controller = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _context = new PlayerContext(_camera,_cameraPosition, _controller,_input,_anim, GameData.Instance.Player,
            this.transform, groundMask);


        _input.Movement.Jump.started += Jump_started;

        _input.Movement.Run.started += Run_started;
        _input.Movement.Run.canceled += Run_canceled;

        InitializeStates();
    }
    private void ValidationConstrants()
    {
        Assert.IsNotNull(GetComponent<CharacterController>(), "CharacterController is not assigned.");
        Assert.IsNotNull(GameData.Instance.Player, "Player Data is not assigned.");
        Assert.IsNotNull(GetComponentInChildren<Camera>(), "Camera is not assigned.");
        Assert.IsNotNull(GetComponentInChildren<Animator>(), "Animator is not assigned.");
    }
    private void InitializeStates()
    {
        States.Add(EPlayerState.Idle, new PlayerIdleState(_context, EPlayerState.Idle));
        States.Add(EPlayerState.Walk, new PlayerWalkState(_context, EPlayerState.Walk));
        States.Add(EPlayerState.Run, new PlayerRunState(_context, EPlayerState.Run));
        States.Add(EPlayerState.Jump, new PlayerJumpState(_context, EPlayerState.Jump));
        States.Add(EPlayerState.Dead, new PlayerDeadState(_context, EPlayerState.Dead));
        States.Add(EPlayerState.Freese, new PlayerFreeseState(_context, EPlayerState.Freese));
        CurrentState = States[EPlayerState.Idle];
    }

    private void Run_started(InputAction.CallbackContext obj)
    {
        _context.IsRunning = true;
    }

    private void Run_canceled(InputAction.CallbackContext obj)
    {
        _context.IsRunning = false;
    }
    private void Jump_started(InputAction.CallbackContext obj)
    {
        //if (!_context.IsGrounded) return;
        //_context.IsJumping = true;
        //_anim.Jump();
        //Invoke("CancelJump", 0.1f);
    }
    public void SetFreeze()
    {
        _context.IsFreeze = !_context.IsFreeze;
        if (_context.IsFreeze)
        {
            _camera.cullingMask = FreezeMask;
        }
        else
        {
            Invoke("DelayChangeMask", 0.2f);
        }
    }
    void DelayChangeMask()
    {
        
        if (_context.IsFreeze)
        {
            _camera.cullingMask = FreezeMask;
        }
        else
        {
            _camera.cullingMask = -1;
        }
    }
    public bool IsFreezing { get { return _context.IsFreeze; } }
}

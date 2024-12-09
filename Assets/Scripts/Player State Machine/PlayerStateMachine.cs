
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public enum EPlayerState
    {
        Idle,
        Charging,
        Throwing,
        End
    }
    PlayerContext _context;
    PlayerData _data;

    [SerializeField] ShootingController _shootingController;
    [SerializeField] List<Collider2D> _hitBoxs;
    private void Awake()
    {
        

        ValidationConstrants();
        _data = DataHandler.Instance.PlayerData;
        _context = new PlayerContext(_data, this.transform, _shootingController, _hitBoxs);
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
        States.Add(EPlayerState.End, new PlayerEndState(_context, EPlayerState.End));
        CurrentState = States[EPlayerState.Idle];
    }
    public void SetPlayerTurn()
    {
        _context.IsMyTurn = true;
        //_context.EnableHitboxs(false);
    }
    public void TakeDamage(bool isHead)
    {
        float damage = 0;
        if (isHead)
        {
            damage = StatsReader.Instance.GetStat("Normal Attack").Damage;
        }
        else
        {
            damage = StatsReader.Instance.GetStat("Small Attack").Damage;
        }
        _context.TakeDamage(damage);
        Debug.Log(damage);
    }
    public void TakeDamage(float damage)
    {
        _context.TakeDamage(damage);
        Debug.Log(damage);
    }
    public void Charge()
    {
        if (!_context.IsMyTurn) return;
        _context.FrameForCheckDrag = 3;
        _context.IsCharging = true;
    }
    public void SetPlayerHP() =>_context.SetPlayerHP();
    public float GetPlayerHPPercent() => _context.HP/_context.MaxHP;
    public bool CanUseItem() => !_context.IsCharging && !_context.IsThrowing && _context.IsMyTurn;
    public void SetItem(string name) => _shootingController.SetItemName(name);
    public void Heal(float heal) => _context.Heal(heal);
    public void ThrowSuccess() => _context.IsThrowing = false;
}

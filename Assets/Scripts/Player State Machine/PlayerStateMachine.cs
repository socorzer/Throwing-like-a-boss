
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

    float _missedChance;
    public bool IsAI { get; private set; }
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
        _context.EnableHitboxs(false);
        if (IsAI) StartCoroutine(AutoCharge());
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
    IEnumerator AutoCharge()
    {
        // คำนวณตัวหารตามความเข้มของลม
        float windDenominator = 0;
        float windRange = Mathf.Abs(GameManager.Instance.CurrentWind);
        if (windRange < 0.5f)
        {
            windDenominator = _context.Data.WindDenominators[0];
        }
        else if (windRange < 0.8f)
        {
            windDenominator = _context.Data.WindDenominators[1];
        }
        else { windDenominator = _context.Data.WindDenominators[2]; }

        // คำนวณ targetCharge โดยใช้ windDenominator ที่คำนวณได้
        float windEffect = GameManager.Instance.CurrentWind / windDenominator;
        float targetCharge = 0.55f + -windEffect;

        // ล็อกค่า targetCharge ให้อยู่ในช่วง 0 ถึง 1
        targetCharge = Mathf.Clamp(targetCharge, 0, 1);

        Debug.Log($"CurrentWind/{windDenominator} = {-GameManager.Instance.CurrentWind / windDenominator}\ntarget = {targetCharge}");
        while (_context.ChargingPower < targetCharge)
        {
            Charge();
            yield return new WaitForEndOfFrame();
        }
    }
    public void SetPlayerHP() => _context.SetPlayerHP();
    public float GetPlayerHPPercent() => _context.HP/_context.MaxHP;
    public bool CanUseItem() => !_context.IsCharging && !_context.IsThrowing && _context.IsMyTurn;
    public void SetItem(string name) => _shootingController.SetItemName(name);
    public void Heal(float heal) => _context.Heal(heal);
    public void ThrowSuccess() => _context.IsThrowing = false;
    public void SetAI(AIType typeOfAI)
    {
        IsAI = true;
        GameStats enemyStat = StatsReader.Instance.GetStat($"Enemy HP ({typeOfAI.ToString()})");
        _context.SetPlayerHP(enemyStat);
        _missedChance = enemyStat.MissedChance;
    } 
}
public enum AIType
{
    easy,
    normal,
    hard
}


using Spine.Unity;
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
    [SerializeField] SkeletonAnimation _skeleton;
    float _missedChance;
    bool _isWin;
    public bool IsAI { get; private set; }
    private void Awake()
    {

        ValidationConstrants();
        _data = DataHandler.Instance.PlayerData;
        _context = new PlayerContext(_data, this.transform, _shootingController, _hitBoxs, _skeleton);
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
        _skeleton.state.SetAnimation(0, "Idle Friendly 1", true);

        if (IsAI) StartCoroutine(AutoCharge());
        else StartCoroutine(CountDown());
    }
    IEnumerator CountDown()
    {
        int thinkTime = StatsReader.Instance.GetStat("Time to think").Second;
        float maxWaringTime = StatsReader.Instance.GetStat("Time to Warning").Second;
        float currentWarningTime = maxWaringTime;

        while (thinkTime > 0 && !_context.IsCharging)
        {
            yield return new WaitForSeconds(1);
            thinkTime--;
        }
        while (currentWarningTime > 0 && !_context.IsCharging)
        {
            yield return new WaitForSeconds(1);
            currentWarningTime--;
            UIManager.Instance.SetWarningGage(currentWarningTime/maxWaringTime);
        }
        UIManager.Instance.SetWarningGage(0);

        if (thinkTime > 0 && currentWarningTime > 0)    yield break;
        _context.IsMyTurn = false;
        _context.EnableHitboxs(true);

        GameManager.Instance.ChangePlayerTurn();
        GameManager.Instance.PlayerTakeDamage();
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
        _skeleton.state.SetAnimation(0, "Drag UnFriendly", false);
    }
    public void TakeDamage(float damage)
    {
        _context.TakeDamage(damage);
        Debug.Log(damage);
        _skeleton.state.SetAnimation(0, "Drag UnFriendly", false);
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
        if (windRange < 0.45f)
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
        if(Random.Range(0,100) < _missedChance)
        {
            targetCharge += Random.Range(-0.2f, 0.2f);
        }
        // ล็อกค่า targetCharge ให้อยู่ในช่วง 0 ถึง 1
        targetCharge = Mathf.Clamp(targetCharge, 0, 1);

        // Debug.Log($"CurrentWind/{windDenominator} = {-GameManager.Instance.CurrentWind / windDenominator}\ntarget = {targetCharge}");
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
        Debug.Log($"HP = {enemyStat.HP}");
    }
    public float GetHP() => _context.HP;
    public void PlayThrowAnimation()
    {
        _skeleton.state.SetAnimation(0, "Cheer UnFriendly", false);
    }
    public void SetPlayerEnd(bool isWin)
    {
        _isWin = isWin;
        if (_isWin)
            _skeleton.state.SetAnimation(0, "Cheer Friendly", true);
        else
            _skeleton.state.SetAnimation(0, "Moody UnFriendly", true);
        Invoke(nameof(PlayEndAnimation),1);
    }
    void PlayEndAnimation()
    {

    }
}
public enum AIType
{
    easy,
    normal,
    hard
}

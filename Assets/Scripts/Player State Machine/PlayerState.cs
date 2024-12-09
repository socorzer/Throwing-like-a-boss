using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : BaseState<PlayerStateMachine.EPlayerState>
{
    protected PlayerContext Context;
    public PlayerState(PlayerContext context,PlayerStateMachine.EPlayerState stateKey) : base(stateKey)
    {
        Context = context;
    }
    public void EnterCharging()
    {
        UIManager.Instance.SetChargingGagePosition(Context.PlayerTransform.position);
        UIManager.Instance.ShowChargingGage(true);
    }
    public void UpdateCharging()
    {
        Context.ChargingPower += Context.Data.ChargeRate * Time.deltaTime;
        UIManager.Instance.SetChargingGageValue(Context.ChargingPower / Context.Data.MaxChargingPower);
        if (Context.ChargingPower >= Context.Data.MaxChargingPower || Context.FrameForCheckDrag < 0)
        {
            Context.IsThrowing = true; 
        }
        Context.FrameForCheckDrag--;

    }
    public void ExitCharging()
    {
        UIManager.Instance.ShowChargingGage(false);
        Context.IsCharging = false;
    }
    public void EnterThrowing()
    {
        Debug.Log($"Throw With Power {Context.ChargingPower}");
        Context.ShootingController.Shoot(Context.Data.BulletPrefab, Context.ChargingPower * Context.Data.ThrowPower);
        Context.IsThrowing = false;
        Context.IsMyTurn = false;
    }
    public void ExitThrowing()
    {
        Context.ChargingPower = 0;
        GameManager.Instance.ChangePlayerTurn();
        GameManager.Instance.RandomWind();
        Context.EnableHitboxs(true);
    }

}

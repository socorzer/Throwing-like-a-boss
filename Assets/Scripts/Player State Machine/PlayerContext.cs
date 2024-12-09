using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContext
{

    public PlayerContext(PlayerData playerData,Transform playerTranform, ShootingController shootingContoller)
    {
        Data = playerData;
        PlayerTransform = playerTranform;
        ShootingController = shootingContoller;
        MaxHP = StatsReader.Instance.GetStat("Player HP").HP;
        HP = MaxHP;
    }
    public void TakeDamage(float damage)
    {
        HP -= damage;
        GameManager.Instance.PlayerTakeDamage();
    }
    public Transform PlayerTransform { get; private set; }
    public ShootingController ShootingController { get; private set; }
    public PlayerData Data { get; private set; }
    public bool IsMyTurn { get; set; }
    public bool IsCharging { get; set; }
    public bool IsThrowing { get; set; }
    public float ChargingPower { get; set; }
    public int FrameForCheckDrag { get; set; }
    public float HP { get; private set; }
    public float MaxHP { get; private set; }
}

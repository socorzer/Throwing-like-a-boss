using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContext
{

    public PlayerContext(PlayerData playerData,Transform playerTranform, ShootingController shootingContoller, List<Collider2D> hitBoxs)
    {
        Data = playerData;
        PlayerTransform = playerTranform;
        ShootingController = shootingContoller;
        HitBoxs = hitBoxs;

    }
    public void TakeDamage(float damage)
    {
        HP -= damage;
        GameManager.Instance.PlayerTakeDamage();
    }
    public void Heal(float healPoint)
    {
        HP += healPoint;
        if (HP > MaxHP) HP = MaxHP;

        IsMyTurn = false;
        GameManager.Instance.ChangePlayerTurn();
        GameManager.Instance.PlayerTakeDamage();

    }
    public void EnableHitboxs(bool isEnable)
    {
        List<Collider2D> hitBox = HitBoxs;
        foreach (Collider2D collider in hitBox)
        {
            collider.enabled = isEnable;
        }
    }
    public void SetPlayerHP()
    {
        MaxHP = StatsReader.Instance.GetStat("Player HP").HP;
        HP = MaxHP;
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
    public List<Collider2D> HitBoxs { get; private set; }
}

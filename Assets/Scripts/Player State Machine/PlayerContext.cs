using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class PlayerContext
{

    public PlayerContext(PlayerData playerData,Transform playerTranform, ShootingController shootingContoller, List<Collider2D> hitBoxs,SkeletonAnimation skeleton)
    {
        Data = playerData;
        PlayerTransform = playerTranform;
        ShootingController = shootingContoller;
        HitBoxs = hitBoxs;
        Skeleton = skeleton;
    }
    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            IsEnd = true;
        }
        GameManager.Instance.PlayerTakeDamage();
    }
    public void Heal(float healPoint)
    {
        HP += healPoint;
        if (HP > MaxHP) HP = MaxHP;

        IsMyTurn = false;
        EnableHitboxs(true);

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
    public void SetPlayerHP(GameStats stat = null)
    {
        if (stat == null)
        {
            MaxHP = StatsReader.Instance.GetStat("Player HP").HP;
        }
        else
        {
            MaxHP = stat.HP;
        }
        HP = MaxHP;
    }
    public Transform PlayerTransform { get; private set; }
    public ShootingController ShootingController { get; private set; }
    public SkeletonAnimation Skeleton { get; private set; }
    public PlayerData Data { get; private set; }
    public bool IsMyTurn { get; set; }
    public bool IsCharging { get; set; }
    public bool IsThrowing { get; set; }
    public bool IsEnd { get; set; }
    public float ChargingPower { get; set; }
    public int FrameForCheckDrag { get; set; }
    public float HP { get; private set; }
    public float MaxHP { get; private set; }
    public List<Collider2D> HitBoxs { get; private set; }
}

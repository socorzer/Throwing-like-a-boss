using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public GameObject BulletPrefab;
    public float MaxChargingPower;
    public float ChargeRate;
    public float ThrowPower;
}

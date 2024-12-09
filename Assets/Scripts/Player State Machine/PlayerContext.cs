using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContext
{

    public PlayerContext(PlayerData playerData,Transform playerTranform)
    {
        Data = playerData;
        PlayerTransform = playerTranform;
    }
    public Transform PlayerTransform { get; private set; }
    public PlayerData Data { get; private set; }
    public bool IsCharging { get; set; }
    public bool IsThrowing { get; set; }
    public float ChargingPower { get; set; }
    public int FrameForCheckDrag { get; set; }
}

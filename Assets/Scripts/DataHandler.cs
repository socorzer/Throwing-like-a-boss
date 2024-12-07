using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : Singleton<DataHandler>
{
    [SerializeField] PlayerData _playerData;

    public PlayerData PlayerData { get { return _playerData; } }
}

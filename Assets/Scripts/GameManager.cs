using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameStateMachine _gameStateMachine;
    [SerializeField] List<PlayerStateMachine> _playerStateMachines;

    [SerializeField] float _currentWind;
    [SerializeField] AreaEffector2D _windEffector;

    public GameStateMachine GameStateMachine { get { return _gameStateMachine; } }
    private void Start()
    {
        _gameStateMachine.SetupPlayer(_playerStateMachines);
    }

    public void ChangePlayerTurn()
    {
        _gameStateMachine.ChangePlayerTurn();
    }
    public void PlayerTakeDamage()
    {
        UIManager.Instance.UpdateHPBar(_playerStateMachines);
    }
    public void RandomWind()
    {
        bool isRightWind = Random.Range(0, 2) == 0 ? false : true;
        float windPower = Random.Range(0f, 1.0f);

        if (windPower < 0.05f && windPower > -0.05f) windPower = 0;
        _currentWind = windPower;
        SetWindEffector();
        UIManager.Instance.SetWindGage(isRightWind ? windPower : -windPower);
    }
    public void SetWindEffector()
    {
        _windEffector.forceMagnitude = _currentWind * 10;
    }
}

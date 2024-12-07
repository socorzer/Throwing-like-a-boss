using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameStateMachine _gameStateMachine;
    [SerializeField] List<PlayerStateMachine> _playerStateMachines;
    [SerializeField] PlayerStateMachine _currentPlayer;
    public GameStateMachine GameStateMachine { get { return _gameStateMachine; } }

}

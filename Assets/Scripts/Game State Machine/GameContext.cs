using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameContext
{

    public GameContext()
    {

    }
    public List<PlayerStateMachine> Players { get; set; }
    public PlayerStateMachine CurrentPlayer { get; set; }
    public bool IsPlaying { get; set; }
}

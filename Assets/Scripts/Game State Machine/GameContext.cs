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
    public bool IsSetUpComplete { get; set; }
    public bool Canplay { get; set; }
    public bool IsPlaying { get; set; }
    public bool IsEnd { get; set; }
    public int PlayMinute { get; set; }
    public int PlaySecond { get; set; }
}

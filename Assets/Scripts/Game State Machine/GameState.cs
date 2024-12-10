using UnityEngine;

public abstract class GameState : BaseState<GameStateMachine.EGameState>
{
    protected GameContext Context;
    public GameState(GameContext context,GameStateMachine.EGameState stateKey) : base(stateKey)
    {
        Context = context;
    }
    public void SetFirstPlayerTurn()
    {
        Context.CurrentPlayer = Context.Players[1];
        Context.CurrentPlayer.SetPlayerTurn();
        //SetPlayerHP();
        UIManager.Instance.SetMarkerPosition(Context.CurrentPlayer.transform.position);
    }
    public void SetPlayerHP()
    {
        foreach (var player in Context.Players)
        {
            player.SetPlayerHP();
        }
    }
    public void EndGame()
    {
        Context.Canplay = false;
        int winnerIndex = 0;
        for (int i = 0;i < Context.Players.Count; i++)
        {
            if (Context.Players[i].GetHP() > 0)
            {
                Context.Players[i].SetPlayerEnd(true);
                winnerIndex = i;
            }
            else Context.Players[i].SetPlayerEnd(false);
        }
        string winnerName = winnerIndex == 0 ? "Rich Pig" : "Aunt Next Door";
        string playTime = Context.PlayMinute.ToString() + "." + Context.PlaySecond.ToString();
        UIManager.Instance.SetEndGameUI(winnerName, playTime);
    }

}

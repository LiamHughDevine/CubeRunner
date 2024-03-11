using System;

public class GameMaster : Master
{
    // Returns the other player (for a 2 player game)
    private Player GetOtherPlayer(string name)
    {
        if (Players[0].GetName() == name)
        {
            return Players[1];
        }
        else if (Players[1].GetName() == name)
        {
            return Players[0];
        }
        throw new Exception();
    }

    // Declares the player that didn't die as the winner
    protected override void ResolveDeath(string name)
    {
        Stats.Winner = GetOtherPlayer(name).GetName();
        Stats.DistanceTravelled = GetOtherPlayer(name).TotalDistance();
        ChangeScene.ChangeToScene(2);
    }
}
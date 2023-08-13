using BattleBitAPI;
using BattleBitAPI.Common;
using BattleBitAPI.Server;
using System.Threading.Channels;
using System.Xml;

class Program
{
    static void Main(string[] args)
    {
        var listener = new ServerListener<MyPlayer, MyGameServer>();
        listener.Start(29294);

        Thread.Sleep(-1);
    }
}
class MyPlayer : Player<MyPlayer>
{
    
}
class MyGameServer : GameServer<MyPlayer>
{

    public override async Task OnRoundStarted()
    {
    }
    public override async Task OnRoundEnded()
    {
    }

    public override async Task OnPlayerConnected(MyPlayer player)
    {
        if(player.Team == Team.TeamA)
            RoundSettings.TeamATickets++;
        else
            RoundSettings.TeamBTickets++;
    }

    public override async Task OnPlayerDisconnected(MyPlayer player)
    {
        if(player.Team == Team.TeamA)
            RoundSettings.TeamATickets--;
        else
            RoundSettings.TeamBTickets--;
    }

    public override async Task OnAPlayerKilledAnotherPlayer(OnPlayerKillArguments<MyPlayer> args)
    {
        if (args.Victim.Team == Team.TeamA)
            RoundSettings.TeamBTickets++;
        else 
            RoundSettings.TeamATickets++;
            
        args.Victim.Message("Switching teams");
        args.Victim.ChangeTeam();
        AnnounceShort("Test");
    }


    public override async Task<OnPlayerSpawnArguments> OnPlayerSpawning(MyPlayer player, OnPlayerSpawnArguments request)
    {
        return request;
    }
    public override async Task OnPlayerSpawned(MyPlayer player)
    {
        player.SetFallDamageMultiplier(0f);
    }

    public override async Task OnConnected()
    {
        await Console.Out.WriteLineAsync("Current state: " + RoundSettings.State);

    }
    public override async Task OnGameStateChanged(GameState oldState, GameState newState)
    {
        await Console.Out.WriteLineAsync("State changed to -> " + newState);
    }
}

using DeadworksManaged.Api;

namespace DeadlockMovementTraining;

public partial class DeadlockMovementTraining
{
    public override void OnClientFullConnect(ClientFullConnectEvent args)
    {
        var controller = args.Controller;
        if (controller == null) return;

        int team0 = 0, team1 = 0;

        foreach (var c in Players.GetAll())
        {
            if (c.EntityIndex == controller.EntityIndex) continue;

            var pawnC = c.GetHeroPawn();
            if (pawnC == null) continue;

            if (pawnC.TeamNum == 2) team0++;
            else if (pawnC.TeamNum == 3) team1++;
        }

        var targetTeam = team0 < team1 ? 2 : team1 < team0 ? 3 : Random.Shared.Next(2) == 0 ? 2 : 3;
        controller.ChangeTeam(targetTeam);

        var heroes = Enum.GetValues<Heroes>()
            .Where(h => h.GetHeroData()?.AvailableInGame == true)
            .ToArray();
        controller.SelectHero(heroes[Random.Shared.Next(heroes.Length)]);
    }

    [GameEventHandler("player_respawned")]
    public HookResult OnPlayerRespawned(PlayerRespawnedEvent args)
    {
        var pawn = args.Userid?.As<CCitadelPlayerPawn>();
        if (pawn == null) return HookResult.Continue;

        pawn.SetCurrency(ECurrencyType.EGold, 50000);
        var spawn = GetRandomSpawn();

        TeleportToTrainingSpot(pawn, spawn);
        return HookResult.Continue;
    }

    [GameEventHandler("player_hero_changed")]
    public HookResult OnHeroChanged(GameEvent args)
    {
        var pawn = args.GetPlayerPawn("player")?.As<CCitadelPlayerPawn>();
        if (pawn == null) return HookResult.Handled;

        ResetTrainingHero(pawn);
        ScheduleFullHeal(pawn);

        return HookResult.Handled;
    }
}

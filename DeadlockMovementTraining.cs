using System.Numerics;
using DeadworksManaged.Api;

namespace DeadlockMovementTraining;

public class DeadlockMovementTraining : DeadworksPluginBase
{
    public override string Name => "DeadlockMovementTraining";
    
    private readonly Vector3[] _spawnPoints =
[
    new(452.31f,  -207.00f, 600.12f),
    new(-446.38f,  147.72f, 576.06f),
    new(-786.72f, -756.62f, 640.03f),

    new(-622.38f, -1501.41f, 384.06f),
    new(138.31f,  -1316.03f, 376.06f),
    new(479.22f,  -1183.59f, 386.27f),

    new(894.78f,   894.59f, 640.06f),
    new(653.09f,  1414.09f, 384.00f),
    new(-474.28f, 1223.75f, 376.06f),
    new(-146.66f, 1213.47f, 376.03f),
    new(-666.75f,  734.28f, 384.03f),
];

    private static void SendChat(int slot, string text)
    {
        var msg = new CCitadelUserMsg_ChatMsg
        {
            PlayerSlot = -1,
            Text = text,
            AllChat = true
        };

        NetMessages.Send(msg, RecipientFilter.Single(slot));
    }

    public override void OnLoad(bool isReload)
    {
        Console.WriteLine($"[{Name}] Loaded! (reload={isReload})");
    }

    public override void OnUnload()
    {
        Console.WriteLine($"[{Name}] Unloaded!");
    }


    public override void OnStartupServer()
    {
        ConVar.Find("citadel_player_spawn_time_max_respawn_time")?.SetInt(5);
        ConVar.Find("citadel_trooper_spawn_enabled")?.SetInt(0);
        ConVar.Find("citadel_npc_spawn_enabled")?.SetInt(0);
        ConVar.Find("citadel_allow_duplicate_heroes")?.SetInt(1);
        ConVar.Find("citadel_voice_all_talk")?.SetInt(1);
        ConVar.Find("citadel_player_starting_gold")?.SetInt(0);
        ConVar.Find("citadel_allow_purchasing_anywhere")?.SetInt(1);
    }

    [ChatCommand("hello")]
    public HookResult OnHello(ChatCommandContext ctx)
    {
        var pawn = ctx.Controller?.GetHeroPawn();
        if (pawn == null)
            return HookResult.Handled;

        // Send a HUD announcement to just this player
        var msg = new CCitadelUserMsg_HudGameAnnouncement
        {
            TitleLocstring = "Bem vindo!",
            DescriptionLocstring = "Digite /tr para treinar movimentação!"
        };
        NetMessages.Send(msg, RecipientFilter.Single(ctx.Message.SenderSlot));

        return HookResult.Handled;
    }

    [ChatCommand("pos")]
    public HookResult OnPos(ChatCommandContext ctx)
    {
        var controller = ctx.Controller;
        var pawn = ctx.Controller?.GetHeroPawn();
        var slot = ctx.Message.SenderSlot;
        if (pawn == null ||  controller == null)
            return HookResult.Handled;
        
        var p = pawn.Position;
        var msg = new CCitadelUserMsg_ChatMsg()
        {
            PlayerSlot = slot,
            Text = $"new Vector3({p.X:F2}f, {p.Y:F2}f, {p.Z:F2}f),",
            AllChat = true
        };
        NetMessages.Send(msg, RecipientFilter.Single(slot));

        return HookResult.Handled;
    }

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
            .Where((h => h.GetHeroData()?.AvailableInGame == true))
            .ToArray();
        controller.SelectHero((heroes[Random.Shared.Next(heroes.Length)]));
    }

    [GameEventHandler("player_respawned")]
    public HookResult OnPlayerRespawned(PlayerRespawnedEvent args)
    {
        var pawn = args.Userid?.As<CCitadelPlayerPawn>();
        if (pawn == null) return HookResult.Continue;

        pawn.SetCurrency(ECurrencyType.EGold, 50000);
        var spawn = GetRandomSpawn();

        pawn.Teleport(position: spawn, angles: null, velocity: null);
        ScheduleFullHeal(pawn);
        return HookResult.Continue;
    }

    [GameEventHandler("player_hero_changed")]
    public HookResult OnHeroChanged(GameEvent args)
    {
        var pawn = args.GetPlayerPawn("player")?.As<CCitadelPlayerPawn>();
        if (pawn == null) return HookResult.Handled;

        pawn.ResetHero();
        pawn.SetCurrency(ECurrencyType.EGold, 50000);
        pawn.ModifyCurrency(ECurrencyType.EAbilityPoints, 17, ECurrencySource.ECheats, false, false, false);
        ScheduleFullHeal(pawn);

        return HookResult.Handled;
    }

    private void ScheduleFullHeal(CCitadelPlayerPawn pawn)
    {
        IHandle? timer = null;
        timer = Timer.Every(1.Ticks(), () =>
        {
            if (!pawn.IsValid)
            {
                timer?.Cancel();
                return;
            }

            if (pawn.MaxHealth <= 0)
                return;

            pawn.Health = pawn.MaxHealth;
            timer?.Cancel();
        });
    }

    private Vector3 GetRandomSpawn()
    {
        return _spawnPoints[Random.Shared.Next(_spawnPoints.Length)];
    }
    
    string[] _garbageEntities = {
        "npc_trooper_boss",
        "npc_boss_tier2",
        "npc_boss_tier3",
        "baseanimgraph",
        "destroyable_building",
        "npc_base_defense_sentry",
        "citadel_herotest_orbspawner",
        "npc_barrack_boss",
        "info_neutral_trooper_camp",
        "npc_trooper",
        "npc_super_neutral",
        "npc_trooper_neutral",
        "npc_neutral_sinners_sacrifice",
        "trigger_item_shop",
        "citadel_trigger_shop_tunnel",
        "trigger_item_shop_safe_zone",
        "func_regenerate",
        "citadel_item_pickup_idol",
        "citadel_item_powerup_spawner",
        "citadel_punchable_powerup",
        "citadel_breakable_prop",
        "item_crate_spawn",
        "citadel_zap_trigger",
        "npc_neutral_bug",
    };
    string[] _garbageEntityNames = {
        "amber_shrine_east",
        "amber_shrine_west",
        "amber_effigy_brush",
        "sapphire_shrine_east",
        "sapphire_shrine_west",
        "sapphire_effigy_brush",
        "sapphire_watcher_broadway_left",
        "sapphire_watcher_broadway_right",
        "sapphire_watcher_york_right",
        "sapphire_watcher_york_left",
        "sapphire_watcher_park_left",
        "sapphire_watcher_park_right",
        "amber_watcher_broadway_left",
        "amber_watcher_broadway_right",
        "amber_watcher_york_right",
        "amber_watcher_york_left",
        "amber_watcher_park_left",
        "amber_watcher_park_right",
    };

    public override void OnEntitySpawned(EntitySpawnedEvent args)
    {
        if (_garbageEntities.Contains(args.Entity.DesignerName))
            args.Entity.Remove();

        if (_garbageEntityNames.Contains(args.Entity.Name))
            args.Entity.Remove();
    }
}

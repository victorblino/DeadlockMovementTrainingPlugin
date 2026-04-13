using System.Numerics;
using DeadworksManaged.Api;

namespace DeadlockMovementTraining;

public partial class DeadlockMovementTraining : DeadworksPluginBase
{
    public override string Name => "DeadlockMovementTraining";

    private readonly Dictionary<string, int> _trainingSpotBySlot = new();
    private readonly Dictionary<int, Vector3> _lastTrainingSpotByController = new();
    private readonly Dictionary<int, Vector3> _lastTrainingSpotByPlayerSlot = new();

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
        Server.ExecuteCommand("ai_disable 1");
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

    private void TeleportToTrainingSpot(CCitadelPlayerPawn pawn, Vector3 spot)
    {
        var controller = pawn.Controller;
        if (controller != null)
        {
            _lastTrainingSpotByController[controller.EntityIndex] = spot;
            if (TryGetPlayerSlot(controller, out var slot))
                _lastTrainingSpotByPlayerSlot[slot] = spot;
        }

        pawn.Teleport(position: spot, angles: null, velocity: Vector3.Zero);
        ScheduleFullHeal(pawn);
    }

    private void ResetTrainingAttempt(CCitadelPlayerPawn pawn, Vector3 spot)
    {
        TeleportToTrainingSpot(pawn, spot);
    }

    private static void ResetTrainingHero(CCitadelPlayerPawn pawn)
    {
        pawn.ResetHero();
        pawn.SetCurrency(ECurrencyType.EGold, 50000);
        pawn.ModifyCurrency(ECurrencyType.EAbilityPoints, 17, ECurrencySource.ECheats, false, false, false);
    }

    private Vector3 GetRandomSpawn()
    {
        return _spawnPoints[Random.Shared.Next(_spawnPoints.Length)];
    }
}

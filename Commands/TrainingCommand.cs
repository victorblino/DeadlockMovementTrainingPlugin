using DeadworksManaged.Api;

namespace DeadlockMovementTraining;

public partial class DeadlockMovementTraining
{
    [ChatCommand("tr")]
    public HookResult OnTrainingTeleport(ChatCommandContext ctx)
    {
        var pawn = ctx.Controller?.GetHeroPawn();
        var slot = ctx.Message.SenderSlot;
        if (pawn == null)
            return HookResult.Handled;

        if (ctx.Args.Length == 0 || (ctx.Args.Length == 1 && ctx.Args[0].Equals("help", StringComparison.OrdinalIgnoreCase)))
        {
            ShowTrainingHelp(slot);
            return HookResult.Handled;
        }

        if (!TryGetTrainingSpot(ctx.Args, slot, out var spotIndex, out var error))
        {
            SendChat(slot, error);
            return HookResult.Handled;
        }

        var spot = _spawnPoints[spotIndex];
        _lastTrainingSpotByPlayerSlot[slot] = spot;
        TeleportToTrainingSpot(pawn, spot);
        SendChat(slot, error);
        ShowTrainingSpotInfo(slot, spotIndex);

        return HookResult.Handled;
    }

    private void ShowTrainingHelp(int slot)
    {
        SendHudAnnouncement(slot, "Olhe o chat!", "Veja os comandos disponiveis.");
        SendChat(slot, "Comandos /tr:");
        SendChat(slot, "/tr {tipo}");
        SendChat(slot, "/tr {tipo} next|prev|random|numero");
        SendChat(slot, "Tipos: zip, vent, mboost, basezip, urn");
        SendChat(slot, "Use /tr help para ver isso novamente.");
    }

    private void ShowTrainingSpotInfo(int slot, int spotIndex)
    {
        if (!TryGetTrainingGroupBySpot(spotIndex, out var group, out var localIndex))
            return;

        var title = $"{group.DisplayName} {localIndex + 1}/{group.Count}";
        var description = TrainingSpotHints.TryGetValue(spotIndex, out var hint) && !string.IsNullOrWhiteSpace(hint)
            ? LimitHudText(hint)
            : group.LoadedMessage;

        SendHudAnnouncement(slot, title, description);
    }

    private bool TryGetTrainingSpot(string[] args, int slot, out int spotIndex, out string error)
    {
        error = string.Empty;
        spotIndex = 0;

        var typeArg = args[0].Trim().ToLowerInvariant();
        if (!TryGetTrainingGroup(typeArg, out var group))
        {
            error = "Tipo invalido. Use: zip, vent, mboost, basezip ou urn";
            return false;
        }

        var stateKey = GetTrainingStateKey(slot, group.Name);
        var currentLocalIndex = -1;
        if (_trainingSpotBySlot.TryGetValue(stateKey, out var savedLocalIndex))
            currentLocalIndex = savedLocalIndex;

        var actionArg = args.Length > 1 ? args[1].Trim().ToLowerInvariant() : "next";
        int localIndex;

        switch (actionArg)
        {
            case "next":
            case "n":
                localIndex = (currentLocalIndex + 1 + group.Count) % group.Count;
                break;
            case "prev":
            case "back":
            case "b":
                if (currentLocalIndex < 0) currentLocalIndex = 0;
                localIndex = (currentLocalIndex - 1 + group.Count) % group.Count;
                break;
            case "random":
            case "rand":
            case "r":
                localIndex = Random.Shared.Next(group.Count);
                break;
            default:
                if (!int.TryParse(actionArg, out var inputIndex) || inputIndex < 1 || inputIndex > group.Count)
                {
                    error = $"Spot invalido para {group.Name}. Use 1-{group.Count}, next, prev ou random.";
                    return false;
                }
                localIndex = inputIndex - 1;
                break;
        }

        _trainingSpotBySlot[stateKey] = localIndex;
        spotIndex = group.Start + localIndex;
        error = $"TP -> {group.Name} {localIndex + 1}/{group.Count}";
        return true;
    }

    private static bool TryGetTrainingGroupBySpot(int spotIndex, out TrainingGroup group, out int localIndex)
    {
        foreach (var candidate in TrainingGroups)
        {
            if (spotIndex < candidate.Start || spotIndex >= candidate.Start + candidate.Count)
                continue;

            group = candidate;
            localIndex = spotIndex - candidate.Start;
            return true;
        }

        group = default;
        localIndex = -1;
        return false;
    }

    private static bool TryGetTrainingGroup(string type, out TrainingGroup group)
    {
        group = type switch
        {
            "zip" or "zips"                              => TrainingGroups[0],
            "vent" or "vents"                            => TrainingGroups[1],
            "mboost" or "boost" or "boosts" or "mantle" => TrainingGroups[2],
            "basezip" or "bzip" or "bzips" or "basezipm" => TrainingGroups[3],
            "urn" or "urns"                              => TrainingGroups[4],
            _                                            => default,
        };

        return group.Count > 0;
    }

    private static string GetTrainingStateKey(int slot, string groupName)
    {
        return $"{slot}:{groupName}";
    }
}

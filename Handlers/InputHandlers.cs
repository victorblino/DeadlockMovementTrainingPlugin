using DeadworksManaged.Api;

namespace DeadlockMovementTraining;

public partial class DeadlockMovementTraining
{
    public override void OnAbilityAttempt(AbilityAttemptEvent args)
    {
        if (!args.IsChanged(InputButton.Ability1) || !args.IsHeld(InputButton.Ability1))
            return;

        var controller = args.Controller;
        if (controller == null)
            return;

        if (!_lastTrainingSpotByPlayerSlot.TryGetValue(args.PlayerSlot, out var spot)
            && !_lastTrainingSpotByController.TryGetValue(controller.EntityIndex, out spot))
            return;

        args.Block(InputButton.Ability1);

        var pawn = controller.GetHeroPawn();
        if (pawn == null || !pawn.IsValid)
            return;

        ResetTrainingAttempt(pawn, spot);
    }

    public override void OnProcessUsercmds(ProcessUsercmdsEvent args)
    {
        var pawn = args.Controller?.GetHeroPawn();
        if (pawn == null || !pawn.IsValid)
            return;

        ClearAbilityCooldowns(pawn);
    }

    private static void ClearAbilityCooldowns(CCitadelPlayerPawn pawn)
    {
        foreach (var slot in NoCooldownSlots)
        {
            var ability = pawn.GetAbilityBySlot(slot)?.As<CCitadelBaseAbility>();
            if (ability == null)
                continue;

            ability.CooldownStart = 0;
            ability.CooldownEnd = 0;
        }
    }
}

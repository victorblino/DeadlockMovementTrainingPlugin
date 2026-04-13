using DeadworksManaged.Api;

namespace DeadlockMovementTraining;

public partial class DeadlockMovementTraining
{
    [ChatCommand("hello")]
    public HookResult OnHello(ChatCommandContext ctx)
    {
        var pawn = ctx.Controller?.GetHeroPawn();
        if (pawn == null)
            return HookResult.Handled;

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
        if (pawn == null || controller == null)
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
}

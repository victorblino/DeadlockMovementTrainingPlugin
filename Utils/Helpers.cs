using DeadworksManaged.Api;

namespace DeadlockMovementTraining;

public partial class DeadlockMovementTraining
{
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

    private static void SendHudAnnouncement(int slot, string title, string description)
    {
        var msg = new CCitadelUserMsg_HudGameAnnouncement
        {
            TitleLocstring = title,
            DescriptionLocstring = description
        };

        NetMessages.Send(msg, RecipientFilter.Single(slot));
    }

    private static string LimitHudText(string text)
    {
        const int maxLength = 220;
        if (string.IsNullOrWhiteSpace(text) || text.Length <= maxLength)
            return text;

        return $"{text[..(maxLength - 3)].TrimEnd()}...";
    }

    private static bool TryGetPlayerSlot(CBasePlayerController controller, out int slot)
    {
        for (var i = 0; i < Players.MaxSlot; i++)
        {
            var player = Players.FromSlot(i);
            if (player?.EntityIndex != controller.EntityIndex)
                continue;

            slot = i;
            return true;
        }

        slot = -1;
        return false;
    }
}

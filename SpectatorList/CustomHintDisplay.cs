using Exiled.API.Features;
using NorthwoodLib.Pools;
using PlayerRoles.Spectating;
using System.Linq;
using System.Text;

namespace SpectatorList;

// With love by Jesus-QC <3
public class CustomHintDisplay
{
    private readonly StringBuilder _stringBuilder = StringBuilderPool.Shared.Rent();

    public void Clear() => _stringBuilder.Clear();

    public string DrawHuman(Player player, Player[] spectators)
    {
        _stringBuilder.AppendLine("<align=right><size=45%><color=" + player.Role.Color.ToHex() + '>' + EntryPoint.Instance.Config.SpectatorListTitle);

        foreach (var spectator in spectators)
        {
            if (EntryPoint.Instance.Config.SpectatorNames.Contains("(NONE)"))
                break;

            _stringBuilder.AppendLine(EntryPoint.Instance.Config.SpectatorNames.Replace("(NAME)", spectator.Nickname));
        }

        for (int i = spectators.Length; i < 30; i++)
            _stringBuilder.AppendLine();

        _stringBuilder.Append("</color></size></align>");

        string ret = _stringBuilder.Replace("(COUNT)", spectators.Length.ToString()).ToString();
        _stringBuilder.Clear();
        return ret;
    }

    public string DrawScp(Player player, Player[] scps)
    {
        _stringBuilder.AppendLine("<align=right><size=45%><color=" + player.Role.Color.ToHex() + '>' + EntryPoint.Instance.Config.ScpListTitle);

        foreach (var scp in scps)
        {
            if (EntryPoint.Instance.Config.ScpNames.Contains("(NONE)"))
                break;

            _stringBuilder.AppendLine(EntryPoint.Instance.Config.ScpNames
                .Replace("(NAME)", scp.Nickname)
                .Replace("(ROLE)", scp.Role.ToString()));
        }

        for (int i = scps.Length; i < 30; i++)
            _stringBuilder.AppendLine();

        _stringBuilder.Append("</color></size></align>");

        string ret = _stringBuilder.Replace("(COUNT)", scps.Length.ToString()).ToString();
        _stringBuilder.Clear();
        return ret;
    }
}
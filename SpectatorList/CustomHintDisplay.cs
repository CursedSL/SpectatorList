using Exiled.API.Features;
using System.Text;

namespace SpectatorList;

// With love by Jesus-QC <3
public class CustomHintDisplay
{
    private readonly StringBuilder _stringBuilder = new();

    public string Draw(Player player, int spectators)
    {
        _stringBuilder.Append("<color=" + player.Role.Color.ToHex() + '>' + EntryPoint.Instance.Config.Spectators);
        _stringBuilder.Append("</color>");

        string ret = _stringBuilder.Replace("(COUNT)", spectators.ToString()).ToString();
        _stringBuilder.Clear();
        return ret;
    }

    private static StringBuilder _scpStringBuilder = new();
    public static string _drawScpsCache;

    public static string DrawScp(Player[] scps)
    {
        if (_drawScpsCache == null)
        {
            _scpStringBuilder.Append(EntryPoint.Instance.Config.Scps);

            foreach (var scp in scps)
            {
                if (EntryPoint.Instance.Config.ScpNames.Contains("(NONE)"))
                    break;

                _scpStringBuilder.Append(EntryPoint.Instance.Config.ScpNames
                    .Replace("(NAME)", scp.Nickname)
                    .Replace("(ROLE)", scp.Role.Type.ToString()));
            }

            string ret = _scpStringBuilder.Replace("(COUNT)", scps.Length.ToString()).ToString();
            _scpStringBuilder.Clear();
            _drawScpsCache = ret;
        }

        return _drawScpsCache;
    }
}
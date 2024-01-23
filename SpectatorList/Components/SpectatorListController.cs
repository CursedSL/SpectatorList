using Exiled.API.Features;
using Hints;
using PlayerRoles.Spectating;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SpectatorList.Components;

using System.Linq;

// With love by Jesus-QC <3
public class SpectatorListController : MonoBehaviour
{
    public static float RefreshRate = 1;

    public static readonly Dictionary<Player, SpectatorListController> Controllers = new();

    private Player _player;
    private float _counter;

    private CustomHintDisplay _display;

    public void Init(Player player) => _player = player;

    private void OnDestroy()
    {
        Controllers.Remove(_player);
        _display.Clear();
        _display = null;
    }

    private void Start()
    {
        _display = new CustomHintDisplay();
        Controllers.Add(_player, this);
    }

    private void Update()
    {
        _counter += Time.deltaTime;

        if (_counter < RefreshRate)
            return;

        _counter = 0;
        if (!Round.IsStarted || !_player.IsAlive)
            return;
        if (_player.IsScp)
            DrawHudScp();
        else
            DrawHudHuman();
    }

    private IEnumerable<Player> GetSpectators()
    {
        foreach (var player in Player.List)
        {
            if (!_player.ReferenceHub.IsSpectatedBy(player.ReferenceHub))
                continue;

            if (player.Role.Base is not SpectatorRole spectatorRole || spectatorRole.SyncedSpectatedNetId != _player.NetId)
                continue;

            if (!EntryPoint.ShouldShowPlayer(player))
                continue;

            yield return player;
        }
    }

    private async void DrawHudHuman()
    {
        var spectators = GetSpectators().ToArray();
        if (spectators.Length <= 0)
            return;

        string hint = await Task.Run(() => _display.DrawHuman(_player, spectators));
        _player.Connection.Send(new HintMessage(new TextHint(hint, new[] { new StringHintParameter(string.Empty) })));
    }

    private IEnumerable<Player> GetScps()
    {
        foreach (var player in Player.List)
        {
            if (!_player.ReferenceHub.IsSpectatedBy(player.ReferenceHub))
                continue;

            if (player.Role.Base is not SpectatorRole spectatorRole || spectatorRole.SyncedSpectatedNetId != _player.NetId)
                continue;

            if (!EntryPoint.ShouldShowPlayer(player))
                continue;

            yield return player;
        }
    }

    private async void DrawHudScp()
    {
        var scps = GetScps().ToArray();
        string hint = await Task.Run(() => _display.DrawScp(_player, scps));
        _player.Connection.Send(new HintMessage(new TextHint(hint, new[] { new StringHintParameter(string.Empty) })));
    }
}
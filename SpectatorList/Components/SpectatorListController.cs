using Exiled.API.Features;
using PlayerRoles.Spectating;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SpectatorList.Components;

using System.Linq;
using AdvancedHints;

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
        DrawHud();
    }

    private int GetSpectators()
    {
        int spectators = 0;
        foreach (var player in Player.List)
        {
            if (!_player.ReferenceHub.IsSpectatedBy(player.ReferenceHub))
                continue;

            if (player.Role.Base is not SpectatorRole spectatorRole || spectatorRole.SyncedSpectatedNetId != _player.NetId)
                continue;

            if (!EntryPoint.ShouldShowPlayer(player))
                continue;

            spectators++;
        }

        return spectators;
    }

    private async void DrawHud()
    {
        var spectators = GetSpectators();
        if (spectators <= 0)
            return;

        string hint = await Task.Run(() => _display.Draw(_player, spectators));
        _player.ShowManagedHint(hint, displayLocation: EntryPoint.Instance.Config.SpectatorsLocation);
    }

    private IEnumerable<Player> GetScps()
    {
        foreach (var player in Player.List)
        {
            if (!player.IsScp)
                continue;

            yield return player;
        }
    }

    private async void DrawHudScp()
    {
        var scps = GetScps().ToArray();
        _player.ShowManagedHint(CustomHintDisplay.DrawScp(scps), displayLocation: EntryPoint.Instance.Config.ScpsLocation);
    }
}
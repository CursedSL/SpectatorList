using Exiled.API.Features;
using SpectatorList.Components;
using System;

namespace SpectatorList;

using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;

public class EntryPoint : Plugin<SpectatorListConfig>
{
    public static EntryPoint Instance { get; private set; }

    public override string Name => "SpectatorList";
    public override string Author => "TTypiarz & Jesus-QC";
    public override Version Version => new(2, 1, 0);
    public override Version RequiredExiledVersion => new(8, 0, 0);

    public override void OnEnabled()
    {
        SpectatorListController.RefreshRate = Config.RefreshRate;
        Instance = this;

        Exiled.Events.Handlers.Player.Verified += OnPlayerJoined;
        Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
    }

    public override void OnDisabled()
    {
        Exiled.Events.Handlers.Player.Verified -= OnPlayerJoined;
        Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;

        Instance = null;
    }

    private static void OnPlayerJoined(VerifiedEventArgs ev)
    {
        ev.Player.GameObject.AddComponent<SpectatorListController>().Init(ev.Player);
    }

    private static void OnChangingRole(ChangingRoleEventArgs ev)
    {
        CustomHintDisplay._drawScpsCache = null;
    }

    public static bool ShouldShowPlayer(Player player)
    {
        if (player.IsGlobalModerator || player.IsOverwatchEnabled && Instance.Config.IgnoreOverwatch || player.IsNorthwoodStaff && Instance.Config.IgnoreNorthwood)
            return false;

        if (Instance.Config.IgnoredRoles.Contains(player.ReferenceHub.serverRoles.GetUncoloredRoleString()))
            return false;

        return true;
    }
}
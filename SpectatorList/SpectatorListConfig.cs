using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace SpectatorList;

using AdvancedHints.Enums;

public sealed class SpectatorListConfig : IConfig
{
    [Description("Whether or not the plugin is enabled on this server")]
    public bool IsEnabled { get; set; } = true;

    [Description("Whether or not debug mode is enabled.")]
    public bool Debug { get; set; } = false;

    [Description("Whether or not people with Overwatch enabled should be ignored")]
    public bool IgnoreOverwatch { get; set; } = true;

    [Description("Whether or not Northwood staff should be ignored (Global Moderators will ALWAYS be ignored.)")]
    public bool IgnoreNorthwood { get; set; } = false;

    [Description("List of server roles that should be ignored")]
    public HashSet<string> IgnoredRoles { get; set; } = new();

    [Description("Set the spectators text. Use (COUNT) to get the number of spectators")]
    public string Spectators { get; set; } = "<b>👥 Spectators ((COUNT))</b>";

    [Description("Where the Spectator count should be displayed")]
    public DisplayLocation SpectatorsLocation { get; set; } = DisplayLocation.Top;

    [Description("Set the Scps text. Use (COUNT) to get the number of scps")]
    public string Scps { get; set; } = "<b>👥 Scps ((COUNT)):</b>";

    [Description("Where the Scp count should be displayed")]
    public DisplayLocation ScpsLocation { get; set; } = DisplayLocation.MiddleBottom;

    [Description("How names should be displayed. Use (NAME) to get the player's name; (ROLE) for role.")]
    public string ScpNames { get; set; } = "(NAME): (ROLE)";

    [Description("The refresh rate of the hint")]
    public float RefreshRate { get; set; } = 1;
}
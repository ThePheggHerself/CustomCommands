using CommandSystem;
using Cryptography;
using PluginAPI.Core;
using PluginAPI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace CustomCommands.Commands.Teleport
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SendToPlayer : ICustomCommand
    {
        public string Command => "sendtoplayer";

        public string[] Aliases { get; } = { "send" };

        public string Description => "Teleports a player to another";

        public string[] Usage { get; } = { "%player%", "%target%" };

        public PlayerPermissions? Permission => null;
        public string PermissionString => "cuscom.teleporting";

        public bool RequirePlayerSender => false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CanRun(this, arguments, out response, out var players, out var pSender))
                return false;

            var index = Usage.IndexOf("%target%");
            var hubs = RAUtils.ProcessPlayerIdOrNamesList(arguments, index, out string[] newArgs, false);
            Player target;

            if (hubs.Count < 1)
            {
                response = $"No player(s) found for: {arguments.ElementAt(index)}";
                return false;
            }
            else
            {
                target = Player.Get(hubs.First());
            }

            foreach(var plr in players)
            {
                plr.Position = target.Position;
            }

            response = $"Teleported {players.Count} {(players.Count == 1 ? "player" : "players")} to position {target.Nickname} ({target.PlayerId})";
            return true;
        }
    }
}

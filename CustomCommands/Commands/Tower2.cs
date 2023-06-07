using CommandSystem;
using NWAPIPermissionSystem;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class Tower2 : ICommand, IUsageProvider
	{
		public string Command => "tower2";

		public string[] Aliases => null;

		public string Description => "Teleports the player to a second tower on the surface";

		public string[] Usage { get; } = { "player" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!Extensions.CanRun(sender, arguments, Usage, out response, out List<Player> players))
				return false;

			foreach (Player plr in players)
			{
				if (plr.Role == PlayerRoles.RoleTypeId.Spectator)
					plr.SetRole(PlayerRoles.RoleTypeId.Tutorial, PlayerRoles.RoleChangeReason.RemoteAdmin);

				plr.Position = new Vector3(-15.5f, 1014.5f, -31.5f);
			}

			response = $"Teleported {players.Count} {(players.Count == 1 ? "player" : "players")} to tower 2";
			return true;
		}
	}
}

using CommandSystem;
using InventorySystem;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class DropCommand : ICustomCommand
	{
		public string Command => "drop";

		public string[] Aliases { get; } = { "dropall", "dropinv", "strip" };

		public string Description => "Drops all items and ammo from the specified player(s)";

		public string[] Usage { get; } = { "%player%" };

		public PlayerPermissions? Permission => PlayerPermissions.PlayersManagement;
		public string PermissionString => string.Empty;

		public bool RequirePlayerSender => false;

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CanRun(this, arguments, out response, out var players, out var pSender))
				return false;

			foreach (Player plr in players)
				plr.DropEverything();

			response = $"Player {(players.Count > 1 ? "inventories" : "inventory")} dropped";

			return true;
		}
	}
}

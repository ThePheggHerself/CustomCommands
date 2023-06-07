using CommandSystem;
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
	public class Pocket : ICommand, IUsageProvider
	{
		public string Command => "pocket";

		public string[] Aliases => null;

		public string Description => "Teleports the player into the pocket dimention";

		public string[] Usage { get; } = { "%player%" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!Extensions.CanRun(sender, arguments, Usage, out response, out List<Player> Players))
				return false;

			foreach(Player player in Players)
				player.Position = Vector3.down * 1998.5f;

			response = $"Teleported {Players.Count} {(Players.Count == 1 ? "player" : "players")} to the pocket dimension";
			return true;
		}
	}
}

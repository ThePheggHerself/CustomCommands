using CommandSystem;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.ThrowableProjectiles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class PlayerLockCommand : ICommand, IUsageProvider
	{
		public static List<int> ToggledPlayers = new List<int>();

		public string Command => "playerlock";

		public string[] Aliases => null;

		public string Description => "Temporarily locks any door the specified player interacts with";

		public string[] Usage { get; } = { "%player%" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!Extensions.CanRun(sender, PlayerPermissions.PlayersManagement, arguments, Usage, out response, out List<Player> Plrs))
				return false;

			if (Plrs.Count > 1)
			{
				response = $"Only 1 player can be selected";
				return true;
			}

			if (ToggledPlayers.Contains(Plrs[0].PlayerId))
				ToggledPlayers.Remove(Plrs[0].PlayerId);
			else
				ToggledPlayers.Add(Plrs[0].PlayerId);

			response = $"Doors {(ToggledPlayers.Contains(Plrs[0].PlayerId) ? "will" : "will no longer")} temporarily lock when {Plrs[0].Nickname} interacts with them.";
			return true;
		}
	}
}

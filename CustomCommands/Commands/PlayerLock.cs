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
		public string Command => "playerlock";

		public string[] Aliases => null;

		public string Description => "Temporarily locks any door the specified player interacts with";

		public string[] Usage { get; } = { "%player%" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!Extensions.CanRun(sender, PlayerPermissions.PlayersManagement, arguments, Usage, out response, out List<Player> Plrs))
				return false;

			foreach(var plr in Plrs)
			{
				if (plr.TemporaryData.Contains("plock"))
				{
					plr.TemporaryData.Remove("plock");
				}
				else
					plr.TemporaryData.Add("plock", string.Empty);
			}

			response = $"Playerlock toggled for {Plrs.Count} {(Plrs.Count != 1 ? "players" :"player")}.";
			return true;
		}
	}
}

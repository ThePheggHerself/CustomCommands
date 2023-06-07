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
	public class DropCommand : ICommand, IUsageProvider
	{
		public string Command => "drop";

		public string[] Aliases { get; } = { "dropall", "dropinv", "strip" };

		public string Description => "Drops all items and ammo from the specified player(s)";

		public string[] Usage { get; } = { "%player%" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if(!Extensions.CanRun(sender, arguments, Usage, out response, out List<Player> plrs))
				return false;

			foreach (Player plr in plrs)
				plr.DropEverything();

			response = $"Player {(plrs.Count > 1 ? "inventories" : "inventory")} dropped";


			return true;
		}
	}
}

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
	public class PlayerDestroyCommand : ICommand, IUsageProvider
	{
		public string Command => "playerdestroy";

		public string[] Aliases => null;

		public string Description => "Destroys any door the specified player ineracts with";

		public string[] Usage { get; } = { "%player%" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
            if (!Extensions.CanRun(sender, PlayerPermissions.PermissionsManagement, arguments, Usage, out response, out List<Player> Plrs))
                return false;

            foreach (var plr in Plrs)
            {
                if (plr.TemporaryData.Contains("pdest"))
                {
                    plr.TemporaryData.Remove("pdest");
                }
                else
                    plr.TemporaryData.Add("pdest", string.Empty);
            }

            response = $"Playerdestroy toggled for {Plrs.Count} {(Plrs.Count != 1 ? "players" : "player")}.";
            return true;
        }
	}
}

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
	public class PlayerDestroyCommand : ICustomCommand
	{
		public string Command => "playerdestroy";

		public string[] Aliases => null;

		public string Description => "Destroys any door the specified player ineracts with";

		public string[] Usage { get; } = { "%player%" };

		public PlayerPermissions? Permission => null;
		public string PermissionString => "cuscom.playerdoorcontrol";

		public bool RequirePlayerSender => false;

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CanRun(this, arguments, out response, out var players, out var pSender))
				return false;

			foreach (var plr in players)
            {
                if (plr.TemporaryData.Contains("pdest"))
                {
                    plr.TemporaryData.Remove("pdest");
                }
                else
                    plr.TemporaryData.Add("pdest", string.Empty);
            }

            response = $"Playerdestroy toggled for {players.Count} {(players.Count != 1 ? "players" : "player")}.";
            return true;
        }
	}
}

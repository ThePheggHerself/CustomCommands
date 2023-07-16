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
	public class PlayerLockCommand : ICustomCommand
	{
		public string Command => "playerlock";

		public string[] Aliases => null;

		public string Description => "Temporarily locks any door the specified player interacts with";

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
				if (plr.TemporaryData.Contains("plock"))
				{
					plr.TemporaryData.Remove("plock");
				}
				else
					plr.TemporaryData.Add("plock", string.Empty);
			}

			response = $"Playerlock toggled for {players.Count} {(players.Count != 1 ? "players" :"player")}.";
			return true;
		}
	}
}

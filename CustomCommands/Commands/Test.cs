using CommandSystem;
using CustomCommands.Misc;
using InventorySystem;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Pickups;
using MapGeneration;
using MapGeneration.Distributors;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	[CommandHandler(typeof(ClientCommandHandler))]
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class TestCommand : ICustomCommand
	{
		public string Command => "nevergonna";

		public string[] Aliases => null;

		public string Description => "Test command. Try it :)";

		public string[] Usage => new[] { "give"/*, "you", "up"*/ };

		public PlayerPermissions? Permission => null;

		public string PermissionString => "test";

		public bool RequirePlayerSender => true;

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CanRun(this, arguments, out response, out var players, out var pSender))
				return false;

			if(sender is PlayerCommandSender psender)
			{
				Log.Info("AA");

				int index = 0;

				Log.Info($"{psender?.ReferenceHub == null} {psender?.ReferenceHub?.gameObject == null}");

				foreach (var a in psender.ReferenceHub.roleManager.CurrentRole.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
				{
					Log.Info($"{index++} {a.Name} | {a.GetType()} |");
				}
			}

			

			response = $"Never gonna give you up,\nNever gonna let you down.\nNever gonna run around,\nAnd desert you.\nNever gonna make you cry,\nNever gonna say goodbye.\nNever gonna tell a lie,\nAnd hurt you.";
			return true;
		}
	}
}

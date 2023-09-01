using CommandSystem;
using InventorySystem;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	[CommandHandler(typeof(ClientCommandHandler))]
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class TestCommand : ICommand, IUsageProvider
	{
		public string Command => "nevergonna";

		public string[] Aliases => null;

		public string Description => "Test command. Try it :)";

		public string[] Usage => new[] { "give", "you", "up" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			//foreach(var a in Map.Rooms)
			//{
			//	a.ApiRoom.Lights.LightColor = Color.yellow;
			//}

			//if(sender is PlayerCommandSender pSender)
			//{
			//	Firearm gun = (Firearm)pSender.ReferenceHub.inventory.ServerAddItem(ItemType.GunE11SR);
			//	gun.ValidateAttachmentsCode(9572898);
			//	gun.ApplyAttachmentsCode(9572898, true);

			//	gun.Status = new FirearmStatus(gun.AmmoManagerModule.MaxAmmo, FirearmStatusFlags.Cocked | FirearmStatusFlags.MagazineInserted | FirearmStatusFlags.FlashlightEnabled | FirearmStatusFlags.Chambered, gun.GetCurrentAttachmentsCode());
			//}

			response = $"Never gonna give you up,\nNever gonna let you down.\nNever gonna run around,\nAnd desert you.\nNever gonna make you cry,\nNever gonna say goodbye.\nNever gonna tell a lie,\nAnd hurt you.";
			return true;
		}
	}
}

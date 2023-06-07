using CommandSystem;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RoundRestarting;
using GameCore;
using MapGeneration;
using InventorySystem.Items.Usables.Scp330;
using InventorySystem.Items.Firearms;
using Log = PluginAPI.Core.Log;
using InventorySystem.Items.Firearms.Attachments;

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class BattleEventCommand : ICommand, IUsageProvider
	{
		public static ItemType[] items = new ItemType[]
		{
			ItemType.MicroHID,
			ItemType.GunRevolver,
			ItemType.Jailbird,
			ItemType.ParticleDisruptor,
		};

		public string Command => "battle";

		public string[] Aliases => null;

		public string Description => "Spawns everyone as a D-class, teleports them to 914, and gives them a random weapon";

		public string[] Usage { get; } = { };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!Extensions.CanRun(sender, PlayerPermissions.PlayersManagement, arguments, Usage, out response))
				return false;

			Plugin.EventInProgress = true;
			Round.IsLocked = true;

			var room = RoomIdentifier.AllRoomIdentifiers.Where(r => r.Name == RoomName.Lcz914);
			var room914 = room.First();

			ItemType item = items[UnityEngine.Random.Range(0, items.Length - 1)];

			foreach (Player plr in Player.GetPlayers())
			{
				if (plr.IsServer || plr.Role == PlayerRoles.RoleTypeId.Overwatch)
					continue;

				plr.SetRole(PlayerRoles.RoleTypeId.ClassD, PlayerRoles.RoleChangeReason.RemoteAdmin);
				plr.Position = new Vector3(room914.ApiRoom.Position.x, room914.ApiRoom.Position.y + 1, room914.ApiRoom.Position.z);

				plr.ClearInventory();
				var itemBase = plr.AddItem(item);

				plr.ReferenceHub.inventory.ServerSelectItem(itemBase.ItemSerial);
				plr.ReferenceHub.inventory.CmdSelectItem(itemBase.ItemSerial);

				if (item == ItemType.GunRevolver)
				{
					var firearm = itemBase as Firearm;

					firearm.ValidateAttachmentsCode(1170);
					firearm.ApplyAttachmentsCode(1170, true);
					firearm.Status = new FirearmStatus(firearm.AmmoManagerModule.MaxAmmo, FirearmStatusFlags.Chambered, 1170);
				}

				plr.ReceiveHint("Kill them all!!!");
			}

			response = $"D-class battle has begun";
			return true;
		}
	}
}

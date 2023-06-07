using CommandSystem;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Firearms;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp049;
using PluginAPI.Core;
using PluginAPI.Core.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FacilityZone = MapGeneration.FacilityZone;

namespace CustomCommands.Commands
{
	//[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class HushEventCommand : ICommand, IUsageProvider
	{
		public string Command => "hush";

		public string[] Aliases => null;

		public string Description => "Starts the Hush event";

		public string[] Usage { get; } = { };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!Extensions.CanRun(sender, PlayerPermissions.PlayersManagement, arguments, Usage, out response))
				return false;

			Plugin.EventInProgress = true;
			Round.IsLocked = true;

			var spawn939 = RoomIdentifier.AllRoomIdentifiers.Where(r => r.Name == RoomName.HczArmory).First();
			var HczRooms = RoomIdentifier.AllRoomIdentifiers.Where(r => r.Name == RoomName.HczServers || r.Name == RoomName.HczMicroHID || r.Name == RoomName.Hcz939).ToList();
			int toSpawn939 = Mathf.Clamp(Player.Count / 8, 1, 4);

			Facility.TurnOffAllLights(10000);

			foreach (Player plr in Player.GetPlayers())
			{
				if (plr.IsServer || plr.Role == PlayerRoles.RoleTypeId.Overwatch)
					continue;

				if ((toSpawn939 > 0 && UnityEngine.Random.Range(0, 2) == 1) || !(toSpawn939 < Player.Count))
				{
					plr.SetRole(PlayerRoles.RoleTypeId.Scp939, PlayerRoles.RoleChangeReason.RemoteAdmin);
					plr.Position = new Vector3(spawn939.ApiRoom.Position.x, spawn939.ApiRoom.Position.y + 1, spawn939.ApiRoom.Position.z);

					plr.ReceiveHint("Hunt and kill the humans", 8);
					toSpawn939--;
				}
				else
				{
					plr.SetRole(PlayerRoles.RoleTypeId.Scientist, PlayerRoles.RoleChangeReason.RemoteAdmin);
					var room = HczRooms[UnityEngine.Random.Range(0, HczRooms.Count - 1)];
					plr.Position = new Vector3(room.ApiRoom.Position.x, room.ApiRoom.Position.y + 1, room.ApiRoom.Position.z);

					plr.AddItem(ItemType.KeycardNTFCommander);
					var itemBase = plr.AddItem(ItemType.Flashlight);

					plr.ReferenceHub.inventory.ServerSelectItem(itemBase.ItemSerial);
					plr.ReferenceHub.inventory.CmdSelectItem(itemBase.ItemSerial);

					plr.ReceiveHint("Survive and enable the generators", 8);
				}
			}

			Round.IsLocked = false;

			foreach (var door in DoorVariant.DoorsByRoom[spawn939])
			{
				if (door.RequiredPermissions.RequiredPermissions != Interactables.Interobjects.DoorUtils.KeycardPermissions.None)
					(door as IDamageableDoor).ServerDamage(10000, DoorDamageType.ServerCommand);
			}

			response = $"Hush event has begun";
			return true;
		}
	}
}

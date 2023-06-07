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

namespace CustomCommands.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class GrenadeCommand : ICommand, IUsageProvider
	{
		public string Command => "grenade";

		public string[] Aliases => null;

		public string Description => "Spawns a grenade at a player's location";

		public string[] Usage { get; } = { "%player%" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!Extensions.CanRun(sender, PlayerPermissions.PlayersManagement, arguments, Usage, out response, out List<Player> Players))
				return false;

			string[] args = arguments.ToArray();

			foreach (Player plr in Players)
			{
				if (plr.Role == PlayerRoles.RoleTypeId.Spectator || plr.Role == PlayerRoles.RoleTypeId.Overwatch)
					continue;

				ThrowableItem item = (ThrowableItem)plr.ReferenceHub.inventory.CreateItemInstance(new ItemIdentifier(ItemType.GrenadeHE, ItemSerialGenerator.GenerateNext()), false);
				Vector3 Pos = plr.Position;
				Pos.y += 1;

				TimeGrenade grenade = (TimeGrenade)UnityEngine.Object.Instantiate(item.Projectile, Pos, Quaternion.identity);
				grenade.NetworkInfo = new InventorySystem.Items.Pickups.PickupSyncInfo(item.ItemTypeId, Pos, Quaternion.identity, item.Weight, item.ItemSerial);
				grenade.RigidBody.velocity = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

				grenade.PreviousOwner = new Footprinting.Footprint(plr.ReferenceHub);
				Mirror.NetworkServer.Spawn(grenade.gameObject);
				grenade.ServerActivate();
			}

			response = $"Spawned a grenade on {Players.Count} {(Players.Count > 1 ? "players" : "player")}";
			return true;
		}
	}
}

using AdminToys;
using CustomCommands.Commands;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using MapGeneration.Distributors;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCommands
{
	public class Events
	{
		[PluginEvent(ServerEventType.RoundRestart)]
		public void OnRoundRestart()
		{
			Plugin.EventInProgress = false;
			PlayerLockCommand.ToggledPlayers.Clear();
			PlayerDestroyCommand.ToggledPlayers.Clear();
			generators.Clear();
		}

		[PluginEvent(ServerEventType.PlayerInteractDoor)]
		public bool OnPlayerDoorInteract(Player Player, DoorVariant Door, bool canOpen)
		{
			if (Plugin.EventInProgress)
			{
				if (Door.RequiredPermissions.RequiredPermissions == KeycardPermissions.None)
					return true;
				else return false;
			}
			else
			{
				if (PlayerLockCommand.ToggledPlayers.Contains(Player.PlayerId) && Door.RequiredPermissions.RequiredPermissions == KeycardPermissions.None)
				{
					Door.ServerChangeLock(DoorLockReason.AdminCommand, true);
					Door.UnlockLater(1, DoorLockReason.AdminCommand);

					canOpen = false;
					return false;
				}
				else if (PlayerDestroyCommand.ToggledPlayers.Contains(Player.PlayerId))
				{
					if (Door is IDamageableDoor dmgDoor && Door.RequiredPermissions.RequiredPermissions == KeycardPermissions.None)
					{
						dmgDoor.ServerDamage(10000, DoorDamageType.ServerCommand);

						canOpen = false;
						return false;
					}
				}

				return canOpen;
			}
		}

		public List<Scp079Generator> generators = new List<Scp079Generator>();
		[PluginEvent(ServerEventType.GeneratorActivated)]
		public void GeneratorActivated(Scp079Generator generator)
		{
			if (Plugin.EventInProgress)
			{
				generators.Add(generator);

				if(generators.Count == 3)
				{
					var plrs = Player.GetPlayers().Where(r => r.Role == RoleTypeId.Scp939);
					foreach(var plr in plrs)
					{
						plr.Kill();
					}

					Round.End();
				}
			}
		}

		[PluginEvent(ServerEventType.Scp914Activate)]
		public bool SCP914Activate(Player Player, Scp914KnobSetting Setting)
		{
			if (Plugin.EventInProgress)
				return false;
			else return true;
		}


		[PluginEvent(ServerEventType.PlayerInteractElevator)]
		public bool PlayerInteractElevator(Player Player, ElevatorChamber Elevator)
		{
			if (Plugin.EventInProgress)
			{
				if (Elevator.AssignedGroup != ElevatorManager.ElevatorGroup.Nuke)
					return false;
				else return true;
			}
			else
			{
				if (PlayerLockCommand.ToggledPlayers.Contains(Player.PlayerId))
				{
					return false;
				}

				return true;
			}
		}

		[PluginEvent(ServerEventType.PlayerDamagedShootingTarget)]
		public void TargetDamagedEvent(Player attacker, ShootingTarget target, DamageHandlerBase damageHandler, float amount)
		{
			if (attacker.CurrentItem is Firearm firearm)
			{
				Log.Info($"{firearm.GetCurrentAttachmentsCode()}");
			}
		}

		[PluginEvent(ServerEventType.PlayerSpawn)]
		public void PlayerSpawn(Player player, RoleTypeId role)
		{
			if(player.IsSCP && Round.Duration < TimeSpan.FromMinutes(1))
			{
				player.SendBroadcast("You can change your SCP by using the \".scpswap\" command in your console", 6);
			}
		}
	}
}

using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MapGeneration.Distributors;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System.Collections.Generic;
using System.Linq;
using PluginAPI.Events;
using InventorySystem.Items;
using InventorySystem;
using Utils;
using CustomCommands.Commands.Misc;
using System;
using UnityEngine;

namespace CustomCommands
{
	public class Events
	{
		public List<Scp079Generator> generators = new List<Scp079Generator>();

		[PluginEvent(ServerEventType.RoundRestart)]
		public void OnRoundRestart()
		{
			Plugin.CurrentEvent = EventType.NONE;

			generators.Clear();
		}

		[PluginEvent(ServerEventType.TeamRespawn)]
		public bool RespawningEvent(TeamRespawnEvent args)
		{
			if (Plugin.EventInProgress)
				return false;
			else return true;
		}

		[PluginEvent(ServerEventType.PlayerInteractDoor)]
		public bool OnPlayerDoorInteract(PlayerInteractDoorEvent args)
		{
			if (Plugin.EventInProgress)
			{
				if (args.Door.RequiredPermissions.RequiredPermissions == KeycardPermissions.None)
					return true;
				else return false;
			}
			else
			{
				if (args.Player.TemporaryData.Contains("plock") && args.Door.RequiredPermissions.RequiredPermissions == KeycardPermissions.None)
				{
					args.Door.ServerChangeLock(DoorLockReason.AdminCommand, true);
					args.Door.UnlockLater(1, DoorLockReason.AdminCommand);

					args.CanOpen = false;
					return false;
				}
				else if (args.Player.TemporaryData.Contains("pdest") && args.Door.RequiredPermissions.RequiredPermissions == KeycardPermissions.None)
				{
					if (args.Door is IDamageableDoor dmgDoor && args.Door.RequiredPermissions.RequiredPermissions == KeycardPermissions.None)
					{
						dmgDoor.ServerDamage(10000, DoorDamageType.ServerCommand);

						args.CanOpen = false;
						return false;
					}
				}

				return args.CanOpen;
			}
		}

		[PluginEvent(ServerEventType.GeneratorActivated)]
		public void GeneratorActivated(GeneratorActivatedEvent args)
		{
			if (Plugin.EventInProgress)
			{
				if (Plugin.CurrentEvent == EventType.Hush)
				{
					generators.Add(args.Generator);

					if (generators.Count == 3)
					{
						var plrs = Player.GetPlayers().Where(r => r.Role == RoleTypeId.Scp939);
						foreach (var plr in plrs)
						{
							plr.Kill();
						}

						Round.End();
					}
				}
			}
		}


		[PluginEvent(ServerEventType.PlayerDeactivatedGenerator)]
		public void GeneratorDeactivated(PlayerDeactivatedGeneratorEvent args)
		{
			if (Plugin.EventInProgress)
			{
				if (Plugin.CurrentEvent == EventType.Hush)
				{
					if (args.Player.IsSCP)
					{
						args.Player.Damage(100, "Generator Disabled");
					}
					else
					{
						args.Player.Damage(50, "Do not disable the generators");
					}
				}
			}
		}

		[PluginEvent(ServerEventType.Scp914Activate)]
		public bool SCP914Activate(Scp914ActivateEvent args)
		{
			if (Plugin.EventInProgress)
				return false;
			else return true;
		}

		[PluginEvent(ServerEventType.PlayerThrowItem)]
		public bool ThrowItem(PlayerThrowItemEvent args)
		{
			if (Plugin.EventInProgress)
			{
				if (Plugin.CurrentEvent == EventType.Hush)
					return false;
				else return true;
			}
			return true;
		}

		[PluginEvent(ServerEventType.PlayerInteractElevator)]
		public bool PlayerInteractElevator(PlayerInteractElevatorEvent args)
		{
			if (Plugin.EventInProgress)
			{
				if (Plugin.CurrentEvent == EventType.Hush)
				{
					if (args.Elevator.AssignedGroup == ElevatorManager.ElevatorGroup.Nuke)
						return true;

					else if (args.Elevator.AssignedGroup == ElevatorManager.ElevatorGroup.Scp049 && args.Player.Role == RoleTypeId.Scp939)
						return true;

					else return false;
				}
				else return false;
			}
			else
			{
				if (args.Player.TemporaryData.Contains("plock"))
				{
					return false;
				}

				return true;
			}
		}

		[PluginEvent(ServerEventType.PlayerDying), PluginPriority(LoadPriority.Highest)]
		public bool PlayerDeath(PlayerDyingEvent args)
		{
			if (Plugin.CurrentEvent == EventType.Infection && args.DamageHandler is AttackerDamageHandler aDH)
			{
				args.Player.ReferenceHub.roleManager.ServerSetRole(args.Attacker.Role, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.AssignInventory);

				return false;
			}
			return true;
		}

		[PluginEvent(ServerEventType.PlayerCoinFlip)]
		public void CoinFlip(PlayerCoinFlipEvent args)
		{
			if (args.Player.Role == RoleTypeId.Tutorial)
			{
				MEC.Timing.CallDelayed(2, () =>
				{
					if (!args.IsTails)
					{
						ExplosionUtils.ServerExplode(args.Player.ReferenceHub);
					}
				});
			}
		}

		[PluginEvent(ServerEventType.RoundRestart)]
		public void RoundRestart(RoundRestartEvent args)
		{
			Plugin.SetVote(VoteType.NONE, string.Empty);
		}

		[PluginEvent(ServerEventType.RoundEnd)]
		public void RoundEnd(RoundEndEvent args)
		{
			Plugin.EndVote();
		}
	}
}

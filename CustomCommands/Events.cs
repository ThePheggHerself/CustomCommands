using AdminToys;
using CustomCommands.Commands;
using CustomPlayerEffects;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
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
using UnityStandardAssets.Effects;
using InventorySystem.Disarming;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items;
using Utils;
using Footprinting;
using InventorySystem.Items.Pickups;
using MapGeneration;
using Respawning;

namespace CustomCommands
{
    public class Events
    {
        public List<Scp079Generator> generators = new List<Scp079Generator>();







        [PluginEvent(ServerEventType.RoundRestart)]
        public void OnRoundRestart()
        {
            Plugin.CurrentEvent = EventType.NONE;
            PlayerLockCommand.ToggledPlayers.Clear();
            PlayerDestroyCommand.ToggledPlayers.Clear();

            generators.Clear();
        }

        [PluginEvent(ServerEventType.TeamRespawn)]
        public bool RespawningEvent(SpawnableTeamType team)
        {
            if (Plugin.EventInProgress)
                return false;
            else return true;
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

        [PluginEvent(ServerEventType.GeneratorActivated)]
        public void GeneratorActivated(Scp079Generator generator)
        {
            if (Plugin.EventInProgress)
            {
                if (Plugin.CurrentEvent == EventType.Hush)
                {
                    generators.Add(generator);

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
        public void GeneratorDeactivated(Player player, Scp079Generator generator)
        {
            if (Plugin.EventInProgress)
            {
                if (Plugin.CurrentEvent == EventType.Hush)
                {
                    if (player.IsSCP)
                    {
                        player.Damage(100, "Generator Disabled");
                    }
                    else
                    {
                        player.Damage(50, "Do not disable the generators");
                    }
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

        [PluginEvent(ServerEventType.PlayerThrowItem)]
        public bool ThrowItem(Player player, ItemBase item, Rigidbody rigidbody)
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
        public bool PlayerInteractElevator(Player Player, ElevatorChamber Elevator)
        {
            if (Plugin.EventInProgress)
            {
                if (Plugin.CurrentEvent == EventType.Hush)
                {
                    if (Elevator.AssignedGroup == ElevatorManager.ElevatorGroup.Nuke)
                        return true;

                    else if (Elevator.AssignedGroup == ElevatorManager.ElevatorGroup.Scp049 && Player.Role == RoleTypeId.Scp939)
                        return true;

                    else return false;
                }
                else return false;
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

        [PluginEvent(ServerEventType.PlayerDying), PluginPriority(LoadPriority.Highest)]
        public bool PlayerDeath(Player target, Player attacker, DamageHandlerBase damageHandler)
        {
            if (Plugin.CurrentEvent == EventType.Infection && damageHandler is AttackerDamageHandler aDH)
            {
                target.ReferenceHub.roleManager.ServerSetRole(attacker.Role, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.AssignInventory);

                return false;
            }

            return true;
        }
       

        [PluginEvent(ServerEventType.PlayerHandcuff)]
        public void OnDisarm(Player player, Player target)
        {
            if (!target.IsDisarmed)
            {
                //target.ReceiveHint("RUN!", 5);
                target.EffectsManager.EnableEffect<MovementBoost>(5);
                target.EffectsManager.GetEffect<MovementBoost>().Intensity = 255;

                target.EffectsManager.EnableEffect<DamageReduction>(5);
                target.EffectsManager.GetEffect<DamageReduction>().Intensity = 190;
            }
        }
    }
}

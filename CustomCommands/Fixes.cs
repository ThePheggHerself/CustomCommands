using PlayerRoles;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminToys;
using InventorySystem.Items.Firearms;
using PlayerStatsSystem;
using InventorySystem.Items.Firearms.Attachments;
using Mirror;
using UnityEngine;

namespace CustomCommands
{
    public class Fixes
    {
        [PluginEvent(ServerEventType.PlayerJoined)]
        public void PlayerJoin(Player player)
        {
            if (!Plugin.EventInProgress && Round.IsRoundStarted && Round.Duration.TotalSeconds < 30)
                player.SetRole(RoleTypeId.ClassD, RoleChangeReason.LateJoin);
        }

        [PluginEvent(ServerEventType.Scp173NewObserver)]
        public bool New173Target(Player player, Player target)
        {
            if (target.Role == RoleTypeId.Tutorial)
            {
                return false;
            }
            else return true;
        }

        [PluginEvent(ServerEventType.Scp096AddingTarget)]
        public bool New096Target(Player player, Player target, bool isForLook)
        {
            if (target.Role == RoleTypeId.Tutorial)
                return false;
            else return true;
        }

        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void PlayerSpawn(Player player, RoleTypeId role)
        {
            if (player.IsSCP && Round.Duration < TimeSpan.FromMinutes(1) && !Plugin.EventInProgress)
            {
                player.SendBroadcast("You can change your SCP by using the \".scpswap\" command in your console", 6);
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

        //[PluginEvent(ServerEventType.WaitingForPlayers)]
        public void WaitingForPlayers()
        {
            foreach (var a in NetworkClient.prefabs)
            {
                Log.Info(a.Value.name);
            }

            var lightPrefab = NetworkClient.prefabs.Where(r => r.Value.name == "LightSourceToy").First();

            var lightGO = GameObject.Instantiate(lightPrefab.Value);
            lightGO.transform.position = new UnityEngine.Vector3(135, 995, -50);
            lightGO.GetComponent<Light>().intensity = 100;
            NetworkServer.Spawn(lightGO);

            lightGO = GameObject.Instantiate(lightPrefab.Value);
            lightGO.transform.position = new UnityEngine.Vector3(135, 995, -48);
            lightGO.GetComponent<Light>().intensity = 100;
            NetworkServer.Spawn(lightGO);

            lightGO = GameObject.Instantiate(lightPrefab.Value);
            lightGO.transform.position = new UnityEngine.Vector3(135, 995, -46);
            lightGO.GetComponent<Light>().intensity = 100;
            NetworkServer.Spawn(lightGO);

            lightGO = GameObject.Instantiate(lightPrefab.Value);
            lightGO.transform.position = new UnityEngine.Vector3(135, 995, -44);
            lightGO.GetComponent<Light>().intensity = 100;
            NetworkServer.Spawn(lightGO);

            lightGO = GameObject.Instantiate(lightPrefab.Value);
            lightGO.transform.position = new UnityEngine.Vector3(135, 995, -42);
            lightGO.GetComponent<Light>().intensity = 100;
            NetworkServer.Spawn(lightGO);

            lightGO = GameObject.Instantiate(lightPrefab.Value);
            lightGO.transform.position = new UnityEngine.Vector3(135, 995, -40);
            lightGO.GetComponent<Light>().intensity = 100;
            NetworkServer.Spawn(lightGO);

            lightGO = GameObject.Instantiate(lightPrefab.Value);
            lightGO.transform.position = new UnityEngine.Vector3(135, 995, -38);
            lightGO.GetComponent<Light>().intensity = 100;
            NetworkServer.Spawn(lightGO);
        }
    }
}

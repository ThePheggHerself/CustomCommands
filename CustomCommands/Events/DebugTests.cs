using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.ThrowableProjectiles;
using MapGeneration;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace CustomCommands.Events
{
    public class DebugTests
    {
        [PluginEvent(ServerEventType.PlayerDamagedShootingTarget)]
        public void TargetDamagedEvent(PlayerDamagedShootingTargetEvent args)
        {
            if (args.Player.CurrentItem is Firearm firearm)
            {
                Log.Info($"{firearm.GetCurrentAttachmentsCode()}");
            }
        }

        //[PluginEvent(ServerEventType.MapGenerated)]
        public void MapGenEvent(MapGeneratedEvent args)
        {
            foreach (var a in NetworkClient.prefabs)
            {
                Log.Info($"{a.Value.name}");
            }
        }

        //[PluginEvent(ServerEventType.PlayerShotWeapon)]
        public void RagdollGun(PlayerShotWeaponEvent args)
        {
            var plr = args.Player;

            PlayerRoleLoader.TryGetRoleTemplate(RoleTypeId.ClassD, out HumanRole ragdollRole);

            var dh = new FirearmDamageHandler(args.Firearm, 10);

            Vector3 velocity = Vector3.zero;
            velocity += args.Player.Camera.transform.forward * UnityEngine.Random.Range(5, 10);

            if (UnityEngine.Random.Range(1, 2) % 2 == 0)
                velocity += args.Player.Camera.transform.right * UnityEngine.Random.Range(1, 2.5f);
            else
                velocity -= args.Player.Camera.transform.right * UnityEngine.Random.Range(1, 2.5f);

            velocity += args.Player.Camera.transform.up * UnityEngine.Random.Range(0.75f, 4.5f);

            typeof(StandardDamageHandler).GetField("StartVelocity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(dh, velocity);
            //.SetValue(dh, new Vector3(10, 1, 0));

            BasicRagdoll basicRagdoll = UnityEngine.Object.Instantiate<BasicRagdoll>(ragdollRole.Ragdoll);
            basicRagdoll.NetworkInfo = new RagdollData(null, dh, RoleTypeId.ClassD, plr.Position, plr.GameObject.transform.rotation, plr.Nickname, NetworkTime.time);
            basicRagdoll.gameObject.AddComponent<FakeRagdoll>();
            NetworkServer.Spawn(basicRagdoll.gameObject, (NetworkConnection)null);
        }

        //[PluginEvent(ServerEventType.PlayerShotWeapon)]
        public void GrenadeLauncher(PlayerShotWeaponEvent args)
        {
            var plr = args.Player;

            ThrowableItem item = (ThrowableItem)plr.ReferenceHub.inventory.CreateItemInstance(new ItemIdentifier(ItemType.GrenadeHE, ItemSerialGenerator.GenerateNext()), false);
            Vector3 Pos = plr.Position;

            TimeGrenade grenade = (TimeGrenade)UnityEngine.Object.Instantiate(item.Projectile, Pos, Quaternion.identity);
            grenade.NetworkInfo = new InventorySystem.Items.Pickups.PickupSyncInfo(item.ItemTypeId, item.Weight, item.ItemSerial);
            grenade.Position = Pos;
            grenade.Rotation = plr.GameObject.transform.rotation;

            grenade.GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(5, 10), UnityEngine.Random.Range(0, 0.75f), UnityEngine.Random.Range(-2, 2));

            grenade.PreviousOwner = new Footprinting.Footprint(plr.ReferenceHub);
            NetworkServer.Spawn(grenade.gameObject);
            grenade.ServerActivate();
        }
    }
}

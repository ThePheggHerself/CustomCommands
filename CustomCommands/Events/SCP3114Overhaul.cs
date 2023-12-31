using MapGeneration;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.Ragdolls;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using UnityEngine;

namespace CustomCommands.Events
{
    public class SCP3114Overhaul
    {
        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void PlayerSpawn(PlayerSpawnEvent args)
        {
            if (args.Player.Role == RoleTypeId.Scp3114)
            {
                Timing.CallDelayed(0.2f, () =>
                {
                    var role = Disguise3114(args.Player);

                    role.SpawnpointHandler.TryGetSpawnpoint(out Vector3 pos, out float rot);

                    args.Player.Position = pos;
                    var scpid = (args.Player.RoleBase as Scp3114Role);
                    scpid.FpcModule.MouseLook.CurrentHorizontal = rot;
                });

                //Timing.CallDelayed(45f, () =>
                //{

                //})
            }
        }

        public static HumanRole Disguise3114(Player plr)
        {
            #region Spawns Ragdoll

            RoomIdentifier roomIdentifier;
            RoomIdUtils.TryFindRoom(RoomName.EzEvacShelter, FacilityZone.Entrance, RoomShape.Endroom, out roomIdentifier);
            Transform transform = roomIdentifier.transform;

            RoleTypeId role = new System.Random().Next(0, 2) == 1 ? RoleTypeId.ClassD : RoleTypeId.Scientist;

            HumanRole humanRole;
            PlayerRoleLoader.TryGetRoleTemplate<HumanRole>(role, out humanRole);

            BasicRagdoll basicRagdoll = UnityEngine.Object.Instantiate<BasicRagdoll>(humanRole.Ragdoll);
            basicRagdoll.NetworkInfo = new RagdollData(null, new Scp3114DamageHandler(basicRagdoll, true), role, transform.position, transform.rotation, plr.Nickname, NetworkTime.time);
            NetworkServer.Spawn(basicRagdoll.gameObject, (NetworkConnection)null);

            #endregion

            var scpid = (plr.ReferenceHub.roleManager.CurrentRole as Scp3114Role);
            scpid.CurIdentity.Ragdoll = basicRagdoll;
            scpid.Disguised = true;

            return humanRole;
        }
    }
}

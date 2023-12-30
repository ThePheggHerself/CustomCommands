using CommandSystem;
using MEC;
using PlayerRoles;
using PlayerRoles.RoleAssign;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using RemoteAdmin;
using RoundRestarting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands.Features
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class cmdSCPList : ICommand
    {
        public string Command => "scplist";

        public string[] Aliases { get; } = { "slist" };
        public string Description => "Lists all current SCPs";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender pSender && pSender.ReferenceHub.IsSCP())
            {
                var plr = Player.Get(pSender.ReferenceHub);

                var scps = Player.GetPlayers().Where(r => r.IsSCP);

                if (!scps.Any())
                {
                    response = "There are no other SCPs";
                    return false;
                }

                List<string> scpString = new List<string>();
                foreach (var a in scps)
                {
                    scpString.Add(a.Role.ToString().ToLower().Replace("scp", string.Empty));
                }

                response = $"Current SCPs: {string.Join(", ", scpString)}";
                return true;
            }

            response = "You must be an SCP to run this command";
            return false;
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class cmdSCPSwap : ICustomCommand
    {
        public string Command => "scpswap";

        public string[] Aliases { get; } = { "sswap" };
        public string Description => "Changes your current SCP";

        public string[] Usage { get; } = { "scp" };

        public PlayerPermissions? Permission => null;

        public string PermissionString => string.Empty;

        public bool RequirePlayerSender => true;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender pSender && pSender.ReferenceHub.IsSCP() && pSender.ReferenceHub.roleManager.CurrentRole.RoleTypeId != RoleTypeId.Scp0492)
            {
                var player = Player.Get(pSender.ReferenceHub);

                if (player.Health != player.MaxHealth)
                {
                    response = "You cannot swap as you have taken damage";
                    return false;
                }
                if (Round.Duration > TimeSpan.FromMinutes(1))
                {
                    response = "You can only swap your SCP within the first minute of a round";
                    return false;
                }

                var role = Extensions.GetRoleFromString($"SCP" + arguments.Array[1]);

                if (role == RoleTypeId.None)
                {
                    response = "No valid SCP provided";
                    return false;
                }

                if (!Player.GetPlayers().Where(r => r.Role == role).Any())
                {
                    player.SetRole(role);
                    response = "You have now swapped SCP";
                    return true;
                }
                else
                {
                    response = "There is already a player playing as that SCP";
                    return false;
                }
            }

            response = "You must be an SCP to run this command";
            return false;
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class cmdSwapToHuman : ICustomCommand
    {
        public string Command => "human";

        public string[] Aliases => null;
        public string Description => "Changes you back to a human role";

        public string[] Usage => null;

        public PlayerPermissions? Permission => null;

        public string PermissionString => string.Empty;

        public bool RequirePlayerSender => true;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender pSender && pSender.ReferenceHub.IsSCP() && pSender.ReferenceHub.roleManager.CurrentRole.RoleTypeId != RoleTypeId.Scp0492)
            {
                var player = Player.Get(pSender.ReferenceHub);

                if (player.Health != player.MaxHealth)
                {
                    response = "You cannot swap as you have taken damage";
                    return false;
                }
                if (Round.Duration > TimeSpan.FromSeconds(30))
                {
                    response = "You can only swap from SCP within the first 30 seconds of a round";
                    return false;
                }

                SCPSwap.SCPsToReplace++;
                HumanSpawner.SpawnLate(pSender.ReferenceHub);
                player.TemporaryData.Add("startedasscp", true.ToString());

                var count = SCPSwap.SCPsToReplace;
                SCPSwap.ReplaceBroadcast();

                response = "You have now swapped to Human from SCP";
                return true;
            }

            response = "You must be an SCP to run this command";
            return false;
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class cmdSwapFromHuman : ICustomCommand
    {
        public string Command => "scp";

        public string[] Aliases => null;
        public string Description => "Replaces you with an SCP who disconnected or swapped to human";

        public string[] Usage => null;

        public PlayerPermissions? Permission => null;

        public string PermissionString => string.Empty;

        public bool RequirePlayerSender => true;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender pSender && !pSender.ReferenceHub.IsSCP() && pSender.ReferenceHub.IsAlive())
            {
                var player = Player.Get(pSender.ReferenceHub);

                if (SCPSwap.SCPsToReplace < 1)
                {
                    response = "There are no SCPs to replace";
                    return false;
                }
                if (player.TemporaryData.Contains("startedasscp"))
                {
                    response = "You were already an SCP this round";
                    return false;
                }
                if (Round.Duration > TimeSpan.FromSeconds(30))
                {
                    response = "You can only swap within the first 30 seconds of the round";
                    return false;
                }

                var scps = SCPSwap.AvailableSCPs;

                player.SetRole(scps[new Random().Next(0, scps.Length)], RoleChangeReason.LateJoin);

                SCPSwap.SCPsToReplace--;

                if (SCPSwap.Cooldown.ContainsKey(player.UserId))
                    SCPSwap.Cooldown[player.UserId] = RoundRestart.UptimeRounds;
                else
                    SCPSwap.Cooldown.Add(player.UserId, RoundRestart.UptimeRounds);

                response = "You have replaced an SCP";
                return true;
            }

            response = "You must be a living human role to run this command";
            return false;
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TriggerScpReplace : ICustomCommand
    {
        public string Command => "replacescp";

        public string[] Aliases => null;
        public string Description => "Manually triggers the SCP replacement broadcast";

        public string[] Usage => null;

        public PlayerPermissions? Permission => null;

        public string PermissionString => string.Empty;

        public bool RequirePlayerSender => false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Round.Duration > TimeSpan.FromSeconds(30))
            {
                response = "You can only use this command within the first 30 seconds of the round";
                return false;
            }
            SCPSwap.SCPsToReplace++;
            SCPSwap.ReplaceBroadcast();
            response = "SCP replace triggered";
            return true;
        }
    }



    public class SCPSwap
	{
        public static int SCPsToReplace = 0;
        public static void ReplaceBroadcast() => Server.SendBroadcast($"There {(SCPsToReplace == 1 ? "is" : "are")} now {SCPsToReplace} SCP spot{(SCPsToReplace == 1 ? "" : "s")} available. Run \".scp\" to queue for an SCP", 5);


        public static RoleTypeId[] AvailableSCPs
        {
            get
            {
                var Roles = new List<RoleTypeId>() { RoleTypeId.Scp049, RoleTypeId.Scp079, RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939 };

                foreach(var r in Player.GetPlayers().Where(r => r.ReferenceHub.IsSCP()).Select(r => r.Role))
                {
                    if (Roles.Contains(r))
                        Roles.Remove(r);
                }

                return Roles.ToArray();
            }
        }

		public static Dictionary<string, int> Cooldown = new Dictionary<string, int>();

		[PluginEvent(ServerEventType.PlayerSpawn)]
		public void PlayerSpawn(PlayerSpawnEvent args)
		{
			if (args.Player.Role.IsValidSCP() && Round.Duration < TimeSpan.FromMinutes(1) && !Plugin.EventInProgress)
			{
				args.Player.SendBroadcast("You can change your SCP by using the \".scpswap\" command in your console", 5);
				args.Player.SendBroadcast("You can change back to a human role by running the \".human\" command", 5);
			}
		}

		[PluginEvent(ServerEventType.RoundStart)]
		public void RoundStart(RoundStartEvent args)
		{
            SCPsToReplace = 0;
		}

		[PluginEvent(ServerEventType.RoundEnd)]
		public void RoundEnd(RoundEndEvent args)
		{
            SCPsToReplace = 0;
        }

        [PluginEvent(ServerEventType.PlayerLeft)]
        public void PlayerLeave(PlayerLeftEvent args)
        {
            if (Round.Duration < TimeSpan.FromSeconds(30) && args.Player.IsSCP)
            {
                SCPsToReplace++;
                ReplaceBroadcast();
            }
        }
	}
}

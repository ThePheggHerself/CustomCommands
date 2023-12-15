using CommandSystem;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands.Features
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class cmdYesVote : ICommand
    {
        public string Command => "yes";

        public string[] Aliases => null;
        public string Description => "Vote yes on the current vote";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender pSender)
            {
                if (!Plugin.VoteInProgress)
                {
                    response = "There is no vote in progress";
                    return false;
                }

                var plr = Player.Get(pSender.ReferenceHub);

                if (plr.TemporaryData.Contains("vote_yes") || plr.TemporaryData.Contains("vote_no"))
                {
                    response = "You have already voted";
                    return false;
                }

                plr.TemporaryData.Override("vote_yes", string.Empty);

                response = "You have voted yes";
                return false;
            }

            response = "You must be a player to run this command";
            return false;
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class cmdNoVote : ICommand
    {
        public string Command => "no";
        public string[] Aliases => null;
        public string Description => "Vote no on the current vote";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender pSender)
            {
                if (!Plugin.VoteInProgress)
                {
                    response = "There is no vote in progress";
                    return false;
                }

                var plr = Player.Get(pSender.ReferenceHub);

                if (plr.TemporaryData.Contains("vote_yes") || plr.TemporaryData.Contains("vote_no"))
                {
                    response = "You have already voted";
                    return false;
                }

                plr.TemporaryData.Override("vote_no", string.Empty);

                response = "You have voted no";
                return false;
            }

            response = "You must be a player to run this command";
            return false;
        }
    }

    public class Voting
	{
		[PluginEvent(ServerEventType.PlayerJoined), PluginPriority(LoadPriority.Lowest)]
		public void PlayerJoin(PlayerJoinedEvent args)
		{
			if (!Plugin.EventInProgress && Round.IsRoundStarted && Round.Duration.TotalSeconds < 30)
				args.Player.SetRole(RoleTypeId.ClassD, RoleChangeReason.LateJoin);

			if (Plugin.VoteInProgress)
			{
				args.Player.SendBroadcast($"<b><color=#fa886b>[VOTE]</color></b> <color=#79b7ed>{Plugin.CurrentVoteString}</color>\nUse your console to vote now!", 15);
				args.Player.SendConsoleMessage("A vote has been started. Run the `.yes` command to vote yes, or `.no` command to vote no");
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

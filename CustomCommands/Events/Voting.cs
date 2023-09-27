using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommands.Events
{
	internal class Voting
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

using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace CustomCommands.Events
{
    public class TutorialFixes
    {
        [PluginEvent(ServerEventType.PlayerHandcuff)]
        public bool OnDisarm(PlayerHandcuffEvent args)
        {
            if (args.Target.Role == RoleTypeId.Tutorial)
                return false;
            else
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

        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool PlayerDamage(PlayerDamageEvent args)
        {
            if (args.DamageHandler is AttackerDamageHandler adh)
            {
                if (args.Player.Role == RoleTypeId.Tutorial)
                {
                    if (adh is SnowballDamageHandler)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

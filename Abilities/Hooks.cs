using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace Abilities
{

    public class Hooks
    {
        /// <summary>
        /// Register this into ServerApi.Hooks.NetSendData in order to have respawn buff timers.
        /// </summary>
        public static void RespawnCooldownBuffAdder(SendDataEventArgs args)
        {
            if (args.MsgId != PacketTypes.PlayerSpawn ||
                !Extensions.Cooldowns.ContainsKey((byte)args.number) ||
                (DateTime.UtcNow - Extensions.Cooldowns[(byte)args.number]).TotalSeconds < 0)
            {
                return;
            }

            TShock.Players[(byte)args.number].SetBuff(307, (int)((Extensions.CooldownLengths[(byte)args.number] - (DateTime.UtcNow - Extensions.Cooldowns[(byte)args.number]).TotalSeconds) * 60), true);
        }

        /// <summary>
        /// Register this into ServerApi.Hooks.NpcStrike in order to activate HyperCrit ability.
        /// </summary>
        /// <param name="args"></param>
        public static void OnNpcStrike_HyperCrit(NpcStrikeEventArgs args, bool isFactionAbility)
        {
            HyperCrit.HyperCritActive[(byte)args.Player.whoAmI] = Math.Max(HyperCrit.HyperCritActive[(byte)args.Player.whoAmI] - 1, 0);
            if (isFactionAbility)
            {
                if (Extensions.Random.Next(1, 101) > (HyperCrit.HyperCritActive[(byte)args.Player.whoAmI] / 2) + 4) return;
            }
            else if (Extensions.Random.Next(1, 101) > (HyperCrit.HyperCritActive[(byte)args.Player.whoAmI] / 2) - 5) return;

            int DMG = (int)(args.Damage * 1.25);

            HyperCrit.HyperCritHit(args.Npc.position + new Vector2(args.Npc.width / 2, args.Npc.height / 2), DMG);
        }
    }
}

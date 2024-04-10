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
        public static void OnNpcStrike_HyperCrit(NpcStrikeEventArgs args)
        {
            if (HyperCrit.HyperCritCooldown.ContainsKey((byte)args.Player.whoAmI) && HyperCrit.HyperCritCooldown[(byte)args.Player.whoAmI])
            {
                return;
            }

            if (HyperCrit.HyperCritActive.ContainsKey((byte)args.Player.whoAmI))
            {
                if (Extensions.Random.Next(1, 101) > 3 + HyperCrit.HyperCritActive[(byte)args.Player.whoAmI]) return;
            }
            else if (Extensions.Random.Next(1, 101) > 3)
            {
                return;
            }

            if (!HyperCrit.HyperCritCooldown.ContainsKey((byte)args.Player.whoAmI))
            {
                HyperCrit.HyperCritCooldown.Add((byte)args.Player.whoAmI, true);
            }

            HyperCrit.HyperCritCooldown[(byte)args.Player.whoAmI] = true;
            int DMG = args.Damage * 3;

            if (args.Npc.type == 14 || args.Npc.type == 135 || args.Npc.type == 267)
            {
                DMG = args.Damage * 2;
            }
            HyperCrit.HyperCritHit(args.Npc.position + new Vector2(args.Npc.width / 2, args.Npc.height / 2), DMG, args.Player.whoAmI);

            Task.Run(async () =>
            {
                await Task.Delay(500);
                HyperCrit.HyperCritCooldown[(byte)args.Player.whoAmI] = false;
            });
        }
    }
}

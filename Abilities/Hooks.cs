using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Terraria.GameContent.Drawing;

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
    }
}

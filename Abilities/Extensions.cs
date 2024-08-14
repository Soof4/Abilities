using Terraria;
using TShockAPI;

namespace Abilities
{
    public static class Extensions
    {
        public static void AddBuff(this NPC npc, params (int, int)[] typeAndTimePair)
        {
            foreach ((int, int) pair in typeAndTimePair)
            {
                npc.AddBuff(pair.Item1, pair.Item2);
            }

            TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
        }

        public static bool IsAlive(this TSPlayer? player)
        {
            return player != null && player.Active && !player.Dead;
        }

        public static bool IsAlive(this NPC? npc)
        {
            return npc != null && npc.active && npc.type != 0;
        }
    }
}

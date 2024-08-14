using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using IL.Terraria.GameContent.UI.ResourceSets;
using Terraria;
using Terraria.GameContent.Drawing;
using TShockAPI;

namespace Abilities
{
    public class TheBound : Ability
    {
        public static Dictionary<TSPlayer, TSPlayer> Pairs = new Dictionary<TSPlayer, TSPlayer>();
        public int HealAmount;
        public int MaxDistance;

        public TheBound(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                HealAmount = AbilityLevel;
                MaxDistance = 100 * 16;
            };
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            TSPlayer? tp = null;
            float distance = MaxDistance;

            // Find the target
            foreach (TSPlayer p in TShock.Players)
            {
                if (p.IsAlive() && p != plr && (tp == null || Utils.GetDistance(plr.TPlayer, p.TPlayer) < distance))
                {
                    tp = p;
                    distance = Utils.GetDistance(plr.TPlayer, p.TPlayer);
                }
            }

            if (tp == null || (Pairs.ContainsKey(tp) && Pairs[tp].Index == plr.Index) || (Pairs.ContainsKey(plr) && Pairs[plr] == tp))
            {
                return;
            }

            if (!Pairs.TryAdd(plr, tp))
            {
                Pairs[plr] = tp;
            }

            Task.Run(async () =>
            {
                while (Pairs[plr].Index == tp.Index &&
                    plr.IsAlive() && tp.IsAlive() &&
                    plr.TPlayer.position.WithinRange(tp.TPlayer.position, MaxDistance))
                {
                    PlayVisuals(plr, tp);
                    plr.Heal(HealAmount);
                    tp.Heal(HealAmount);
                    await Task.Delay(1000);
                }

                Pairs.Remove(plr);
            });
        }

        protected override void PlayVisuals(params object[] args)
        {
            TSPlayer p1 = (TSPlayer)args[0];
            TSPlayer p2 = (TSPlayer)args[1];

            Shapes.DrawLine(p1.X + 16, p1.Y + 16, p2.X + 16, p2.Y + 16, 64, ParticleOrchestraType.Keybrand, (byte)p1.Index);
        }
    }
}
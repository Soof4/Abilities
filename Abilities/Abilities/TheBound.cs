using System.Numerics;
using Terraria;
using Terraria.GameContent.Drawing;
using TShockAPI;

namespace Abilities
{
    public class TheBound : Ability
    {
        internal static Dictionary<int, int> BoundedPlayersHPPairs = new Dictionary<int, int>();
        private int BarrierAmount;

        public TheBound(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }

        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                BarrierAmount = 5 + abilityLevel * 5;
            }
        }

        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {

            TSPlayer? tp = null;
            float distance = float.MaxValue;

            foreach (TSPlayer p in TShock.Players)
            {
                if (p != null && p != plr && p.Active && !p.Dead && (tp == null || Extensions.GetDistance(plr.TPlayer, p.TPlayer) < distance))
                {
                    tp = p;
                    distance = Extensions.GetDistance(plr.TPlayer, p.TPlayer);
                }
            }

            if (tp == null)
            {
                return;
            }

            int curBarrier = BarrierAmount;
            plr.TPlayer.statLifeMax += curBarrier;
            tp.TPlayer.statLifeMax += curBarrier;

            if (!BoundedPlayersHPPairs.TryAdd(plr.Index, curBarrier))
            {
                BoundedPlayersHPPairs[plr.Index] += curBarrier;
            }

            if (!BoundedPlayersHPPairs.TryAdd(tp.Index, curBarrier))
            {
                BoundedPlayersHPPairs[tp.Index] += curBarrier;
            }

            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: plr.Index);
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: tp.Index);

            Task.Run(async () =>
            {
                while (plr.Active && tp.Active && !plr.Dead && !tp.Dead && tp.LastNetPosition.WithinRange(plr.LastNetPosition, 100 * 16))
                {
                    PlayVisuals(plr, tp);
                    await Task.Delay(1000);
                }

                plr.TPlayer.statLifeMax -= curBarrier;
                tp.TPlayer.statLifeMax -= curBarrier;

                if (!BoundedPlayersHPPairs.TryAdd(plr.Index, curBarrier))
                {
                    BoundedPlayersHPPairs[plr.Index] -= curBarrier;
                }

                if (!BoundedPlayersHPPairs.TryAdd(tp.Index, curBarrier))
                {
                    BoundedPlayersHPPairs[tp.Index] -= curBarrier;
                }

                TSPlayer.All.SendData(PacketTypes.PlayerHp, number: plr.Index);
                TSPlayer.All.SendData(PacketTypes.PlayerHp, number: tp.Index);
            });
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            TSPlayer plr2 = (TSPlayer)args[1];

            Shapes.DrawLine(plr.X + 16, plr.Y + 16, plr2.X + 16, plr2.Y + 16, 64, ParticleOrchestraType.Keybrand, (byte)plr.Index);
        }
    }
}
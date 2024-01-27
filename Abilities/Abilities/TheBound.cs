using System.Numerics;
using Terraria;
using Terraria.GameContent.Drawing;
using TShockAPI;

namespace Abilities
{
    public class TheBound : Ability
    {
        internal override void CalculateProperties(params object[] args)
        {

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

            int seconds = 30;

            Task.Run(async () =>
            {
                while (seconds > 0 && plr.Active && tp.Active && !plr.Dead && !tp.Dead && tp.LastNetPosition.WithinRange(plr.LastNetPosition, 100*16))
                {
                    PlayVisuals(plr, tp);

                    plr.Heal(2);
                    tp.Heal(2);

                    seconds--;
                    await Task.Delay(1000);
                }
            });


        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            TSPlayer plr2 = (TSPlayer)args[1];

            Shapes.DrawLine(plr.X, plr.Y, plr2.X, plr2.Y, 64, ParticleOrchestraType.StardustPunch, (byte)plr.Index);
        }
    }
}
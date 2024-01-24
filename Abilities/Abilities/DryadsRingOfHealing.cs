using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using TShockAPI;
using dw = Terraria.GameContent.Drawing;

namespace Abilities
{
    public class DryadsRingOfHealing : Ability
    {
        private int BuffDurationInTicks;
        private double HealPercentage;


        public DryadsRingOfHealing(int abilityLevel) {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                BuffDurationInTicks = 400 + 20 * abilityLevel;
                HealPercentage = 0.04 + abilityLevel * 0.01;
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);
            PlayVisuals(plr);

            foreach (TSPlayer p in TShock.Players)
            {
                if (p != null && p.Active && !p.Dead && p.TPlayer.position.WithinRange(plr.TPlayer.position, 384))
                {
                    p.SetBuff(BuffID.DryadsWard, BuffDurationInTicks, true);
                    p.Heal((int)(p.PlayerData.maxHealth * HealPercentage));
                }
            }
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];

            dw.ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(plr.X, plr.Y),
                UniqueInfoPiece = 1
            };

            Shapes.DrawCircle(plr.X + 16, plr.Y + 24, 384, 0.1963, ParticleOrchestraType.ChlorophyteLeafCrystalPassive, (byte)plr.Index);
        }
    }
}
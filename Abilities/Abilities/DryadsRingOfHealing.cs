using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using TShockAPI;
using dw = Terraria.GameContent.Drawing;

namespace Abilities.Abilities
{
    public class DryadsRingOfHealing : Ability
    {
        public int BuffDurationInTicks { get; set; }
        public double HealPercentage { get; set; }

        public DryadsRingOfHealing(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                BuffDurationInTicks = 400 + 20 * AbilityLevel;
                HealPercentage = 0.04 + AbilityLevel * 0.01;
            };

            UpdateStats();
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr);

            foreach (TSPlayer p in TShock.Players)
            {
                if (p.IsAlive() && p.TPlayer.position.WithinRange(plr.TPlayer.position, 384))
                {
                    p.SetBuff(BuffID.DryadsWard, BuffDurationInTicks, true);
                    p.Heal((int)(p.PlayerData.maxHealth * HealPercentage));
                }
            }
        }

        protected override void PlayVisuals(params object[] args)
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
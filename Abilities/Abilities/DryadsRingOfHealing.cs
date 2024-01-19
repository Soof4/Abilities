using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using TShockAPI;
using dw = Terraria.GameContent.Drawing;

namespace Abilities
{
    public class DryadsRingOfHealing : Ability
    {
        int buffDurationInTicks = 400;
        double healPercentage = 0.04;

        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            foreach (TSPlayer p in TShock.Players) {
                if (p != null && p.Active && !p.Dead && p.TPlayer.position.WithinRange(plr.TPlayer.position, 384)) {
                    p.SetBuff(BuffID.DryadsWard, buffDurationInTicks, true);
                    p.Heal((int)(p.PlayerData.maxHealth * healPercentage));
                }
            }
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer caster = (TSPlayer)args[0];
            
            dw.ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X, caster.Y),
                UniqueInfoPiece = 1
            };

            Shapes.DrawCircle(caster.X + 16, caster.Y + 24, 384, 0.1963, ParticleOrchestraType.ChlorophyteLeafCrystalPassive, (byte)caster.Index);
        }
    }

}
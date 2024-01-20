using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using TShockAPI;
using dw = Terraria.GameContent.Drawing;

namespace Abilities
{
    public class Adrenaline : Ability
    {
        private int BuffDurationInTicks;


        public Adrenaline(int abilityLevel) {
            CalculateProperties(abilityLevel);
        }
        
 
        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                BuffDurationInTicks = (int)(10 + 1.5 * (abilityLevel - 1)) * 60;
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);
            PlayVisuals(plr);

            plr.SetBuff(BuffID.Swiftness, BuffDurationInTicks, true);
            plr.SetBuff(BuffID.Panic, BuffDurationInTicks, true);
            plr.SetBuff(BuffID.SugarRush, BuffDurationInTicks, true);
            plr.SetBuff(BuffID.Sunflower, BuffDurationInTicks, true);
            plr.SetBuff(BuffID.NebulaUpDmg1, BuffDurationInTicks, true);
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];

            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(plr.X + 16, plr.Y + 16),
                UniqueInfoPiece = 1171
            };

            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.WaffleIron, settings);
            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings);
            settings.PositionInWorld = new(plr.X + 8, plr.Y - 16);
            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
        }
    }
}
using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Abilities
{
    public class Harvest : Ability
    {
        private int Damage, ProjSpawnInterval;

        public Harvest(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                Damage = (int)((20 + abilityLevel * 15) * (1 + abilityLevel / 10f));
                ProjSpawnInterval = (int)(1000 / Math.Sqrt(abilityLevel));
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);
            PlayVisuals(plr);

            Task.Run(async () =>
            {
                int ms = 0;
                while (ms < 10000)
                {
                    float randX = plr.X + 16 + WorldGen.genRand.Next(-64, 64);
                    float randY = plr.Y + 16 + WorldGen.genRand.Next(-64, 64);
                    AbilityExtentions.SpawnProjectile(randX, randY, 0, 0, ProjectileID.ScytheWhipProj, Damage, 6, plr.Index);
                    ms += ProjSpawnInterval;
                    await Task.Delay(ProjSpawnInterval);
                }
            });
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, -8),
                PositionInWorld = new(plr.X + 16, plr.Y - 16),
                UniqueInfoPiece = ItemID.ScytheWhip
            };

            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
            settings.PositionInWorld.Y = plr.Y + 16;
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
        }
    }
}
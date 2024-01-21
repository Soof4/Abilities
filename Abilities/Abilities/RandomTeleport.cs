using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Abilities
{
    public class RandomTeleport : Ability
    {
        private int RangeInBlocks;

        public RandomTeleport(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                RangeInBlocks = 50 + abilityLevel * 10;
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);
            PlayVisuals(plr, 0, 0);

            float x = 0;
            float y = 0;

            for (int i = 0; i < 100; i++)
            {
                int d = WorldGen.genRand.Next(160, RangeInBlocks * 16);
                double tetha = WorldGen.genRand.NextDouble() * Math.Tau;
                x = d * (float)Math.Cos(tetha);
                y = d * (float)Math.Sin(tetha);

                if (Extensions.IsPosTeleportable((int)((x + plr.X) / 16), (int)((y + plr.Y) / 16)))
                {
                    break;
                }
            }

            plr.Teleport(x + plr.X, y + plr.Y);

            PlayVisuals(plr, x, y);
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            float x = (float)args[1];
            float y = (float)args[2];

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(plr.X + 16 + x, plr.Y + 16 + y),
                UniqueInfoPiece = 1
            };

            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPCSend, settings);
        }
    }
}
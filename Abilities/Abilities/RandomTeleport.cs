using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

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
            PlayVisuals(plr, 0f, 0f);

            float x = 0;
            float y = 0;

            for (int i = 0; i < 100; i++)
            {
                int d = Extensions.Random.Next(160, RangeInBlocks * 16);
                double tetha = Extensions.Random.NextDouble() * Math.Tau;
                x = d * (float)Math.Cos(tetha);
                y = d * (float)Math.Sin(tetha);

                if (Extensions.IsPosTeleportable((int)((x + plr.X) / 16), (int)((y + plr.Y) / 16)))
                {
                    break;
                }
            }

            Vector2 inPortalPos = plr.TPlayer.Bottom;
            Vector2 outPortalPos = plr.TPlayer.Bottom + new Vector2(x, y);

            foreach (TSPlayer p in TShock.Players)
            {
                if (p != null && p.Active && !p.Dead && p.TPlayer.position.WithinRange(plr.TPlayer.position, 20 * 16))
                {
                    p.TPlayer.PotionOfReturnHomePosition = inPortalPos;
                    p.TPlayer.PotionOfReturnOriginalUsePosition = outPortalPos;

                    p.SendData(PacketTypes.PlayerUpdate, number: p.Index);
                }
            }
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
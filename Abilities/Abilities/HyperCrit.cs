using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace Abilities
{
    // Author of this ability is @strangelad on Discord
    public class HyperCrit : Ability
    {
        public static Dictionary<byte, int> HyperCritActive = new();

        public int Uses;
        public HyperCrit(int abilityLevel) : base(abilityLevel) { }


        internal override void CalculateProperties()
        {
            Uses = 50 + ((AbilityLevel - 1) * 10);
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr);
            foreach (TSPlayer aplr in TShock.Players)
            {
                if (aplr != null)
                {
                    if (aplr.Active)
                    {
                        if (!aplr.Dead && aplr.TPlayer.position.WithinRange(plr.LastNetPosition - new Vector2(16, 40), 250))
                        {
                            HyperCritActive[(byte)aplr.TPlayer.whoAmI] = Math.Min(HyperCritActive[(byte)aplr.TPlayer.whoAmI] + Uses, 200);
                        }
                    }
                }
            }
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            makeCircle2(plr.X, plr.Y, 20, 0.785415617, 0, (byte)plr.Index, ParticleOrchestraType.PaladinsHammer);
        }

        public static void HyperCritHit(Vector2 pos, int DMG)
        {
            makeCircle2(pos.X, pos.Y, 17, 0.785415617, 0, 0, ParticleOrchestraType.PaladinsHammer);
            Extensions.MakeSound(pos, 105, 68, 1.5f, 0f);
            foreach (NPC npc in Main.npc)
            {
                if (Extensions.CanDamageThisEnemy(npc) &&
                npc.position.WithinRange(pos - new Vector2(npc.width / 2, npc.height / 2), 150))
                {
                    BurnDebuff(DMG, npc);
                }
            }
        }

        public static void BurnDebuff(int DMG, NPC npc)
        {
            int tickDMG = DMG / 5;
            Task.Run(async () =>
            {
                for (int i = 0; i < 5 && npc.active; i++)
                {
                    ParticleOrchestraSettings settings = new()
                    {
                        PositionInWorld = new(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2)),
                        MovementVector = new Vector2(0, -8)
                    };
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PaladinsHammer, settings);
                    TSPlayer.Server.StrikeNPC(npc.whoAmI, tickDMG, 0, 0);
                    await Task.Delay(1000);
                }
            });
        }
        //agony
        public static void makeCircle2(float posX, float posY, float size, double deltaRadian, double startRadian, byte indexOfPlayerWhoInvokedThis, ParticleOrchestraType particleType, int uniqueInfoPiece = 1)
        {
            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = indexOfPlayerWhoInvokedThis,
                PositionInWorld = new(posX, posY),
                UniqueInfoPiece = uniqueInfoPiece
            };

            for (double i = 0 - startRadian; i < Math.Tau - startRadian; i += deltaRadian)
            {
                settings.MovementVector = new Vector2(
                    new Vector2(posX, posY).DirectionTo(new Vector2(posX + 10 * (float)Math.Cos(i), posY + 10 * (float)Math.Sin(i))).X * size,
                    new Vector2(posX, posY).DirectionTo(new Vector2(posX + 10 * (float)Math.Cos(i), posY + 10 * (float)Math.Sin(i))).Y * size);
                ParticleOrchestrator.BroadcastParticleSpawn(particleType, settings);
            }
        }

    }
}

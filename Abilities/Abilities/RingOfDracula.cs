using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace Abilities.Abilities
{
    public class RingOfDracula : Ability
    {
        public int RangeInBlocks = 24;

        public RingOfDracula(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats();
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr, false);

            int healAmount = 0;
            int targetIndex = -1;

            foreach (NPC npc in Main.npc)
            {
                if (npc.IsAlive() && npc.type != NPCID.TargetDummy && npc.position.WithinRange(plr.TPlayer.position, RangeInBlocks * 16))
                {

                    int stolenHealth = (int)(600 / (1 + Math.Pow(1.0001, -npc.life)) - 300);

                    if (stolenHealth > healAmount)
                    {
                        targetIndex = npc.whoAmI;
                        healAmount = stolenHealth;
                    }
                }
            }

            if (targetIndex != -1)
            {
                NPC npc = Main.npc[targetIndex];
                npc.life -= healAmount;
                NetMessage.SendData((int)PacketTypes.NpcUpdate, -1, -1, null, targetIndex);
                plr.Heal((int)healAmount);

                PlayVisuals(plr, true, npc);
            }
        }

        protected override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            bool spawnSkull = (bool)args[1];

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, -8),
                PositionInWorld = new(plr.X + 16, plr.Y - 16),
                UniqueInfoPiece = ItemID.ObsidianSkullRose
            };

            if (spawnSkull)
            {
                NPC npc = (NPC)args[2];
                settings.PositionInWorld = new Vector2(npc.position.X + npc.width / 2 + 8, npc.position.Y - 8);
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);

            }
            else
            {
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.WaffleIron, settings);
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                settings.PositionInWorld = new(plr.X + 8, plr.Y - 24);
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);

                Shapes.DrawCircle(plr.X + 16, plr.Y + 16, 384, 0.1963, ParticleOrchestraType.BlackLightningSmall, (byte)plr.Index);
            }
        }
    }
}
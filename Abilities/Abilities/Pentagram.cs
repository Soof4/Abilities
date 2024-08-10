using Terraria;
using Terraria.GameContent.Drawing;
using TShockAPI;

namespace Abilities
{

    public class Pentagram : Ability
    {
        public int RangeInBlocks;
        public Pentagram(int abilityLevel) : base(abilityLevel) { }

        internal override void CalculateProperties()
        {
            RangeInBlocks = 15 + AbilityLevel * 5;
        }

        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            int rangeInPixels = RangeInBlocks * 16;
            PlayVisuals(true, plr.TPlayer.position.X, plr.TPlayer.position.Y);

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.boss && !npc.friendly && npc.position.WithinRange(plr.TPlayer.position, rangeInPixels) && !npc.IsABestiaryIconDummy)
                {
                    npc.IsABestiaryIconDummy = false;
                    npc.active = false;
                    PlayVisuals(false, npc.position.X, npc.position.Y);
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, number: npc.whoAmI);
                }
            }
        }

        internal override void PlayVisuals(params object[] args)
        {
            bool drawPentagram = (bool)args[0];
            float x = (float)args[1];
            float y = (float)args[2];


            if (drawPentagram)
            {
                Shapes.DrawCircle(x + 16, y + 16, RangeInBlocks * 16, 0.1963, ParticleOrchestraType.FlameWaders, 0);
                Shapes.DrawStar(x + 16, y + 16, RangeInBlocks * 16, 32, ParticleOrchestraType.FlameWaders, 0);
            }
            else
            {
                ParticleOrchestraSettings settings = new()
                {
                    IndexOfPlayerWhoInvokedThis = 0,
                    MovementVector = new(0, 0),
                    PositionInWorld = new(x + 16, y + 16),
                    UniqueInfoPiece = 1
                };

                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.TownSlimeTransform, settings);
            }
        }
    }
}
using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace Abilities
{
    public class MagicDice : Ability
    {
        int R1_Dmg, R2_HP, R3_Dmg, R4_Duration, R4_Amount, R5_Amount, R6_Dmg, R7_Duration, R7_Dmg, R8_Duration, R8_HP, R8_Dmg,
            R9_Count, R9_Dmg, R10_Ticks, R11_Cap, R12_Duration, R15_Dmg;

        float R8_Scale, R10_Duration;

        int[] roll4List = { BuffID.Blackout, BuffID.VortexDebuff, BuffID.WitheredArmor, BuffID.WitheredWeapon,
            BuffID.CursedInferno, BuffID.MoonLeech, BuffID.Ichor, BuffID.Lucky, BuffID.RapidHealing, BuffID.Endurance,
            BuffID.Thorns, BuffID.Honey, BuffID.Ironskin, BuffID.Regeneration, BuffID.ShadowDodge, BuffID.Invisibility };


        public MagicDice(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                R1_Dmg = (int)((60 + (abilityLevel - 1) * 35) * (1 + abilityLevel / 5f));
                R2_HP = 75 * abilityLevel;
                R3_Dmg = (int)((15 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
                R4_Duration = (int)(600 + (abilityLevel - 1) * 120);
                R4_Amount = (int)(2 + (abilityLevel / 2));
                R5_Amount = (int)(2 + abilityLevel);
                R6_Dmg = (int)((200 + (abilityLevel - 1) * 100) * (1 + abilityLevel / 5f));
                R7_Duration = (int)(5000 + (abilityLevel - 1) * 200);
                R7_Dmg = (int)(15 + (abilityLevel - 1) * 10);
                R8_Duration = (int)(300 + (abilityLevel - 1) * 120);
                R8_HP = (int)(20 + (abilityLevel - 1) * 10);
                R8_Dmg = (int)((25 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
                R8_Scale = 1f + (abilityLevel - 1) * 0.5f;
                R9_Count = (int)(3 + abilityLevel);
                R9_Dmg = (int)((60 + (abilityLevel - 1) * 35) * (1 + abilityLevel / 5f));
                R10_Ticks = (int)(420 + (abilityLevel - 1) * 60);
                R10_Duration = (float)(7 + (abilityLevel - 1) * 1);
                R11_Cap = (int)(250 + (abilityLevel - 1) * 500);
                R12_Duration = (int)(10000 + (abilityLevel - 1) * 3000);
                R15_Dmg = (int)(25 + (abilityLevel - 1) * 15);
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);
            PlayVisuals(plr, 0);

            int i = 0;
            float i2 = 0;

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                if (plr.Dead) return;
                Vector2 savedPos = plr.LastNetPosition;
                ParticleOrchestraSettings settings = new()
                {
                    IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                    MovementVector = new(0, 0),
                    PositionInWorld = new(plr.X + 16, plr.Y),
                    UniqueInfoPiece = 1
                };
                switch (WorldGen.genRand.Next(1, 16))
                {
                    case 1:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Explosion?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        plr.SetBuff(BuffID.Dazed, 650);
                        plr.SetBuff(BuffID.WitheredArmor, 650);
                        plr.SetBuff(BuffID.WitheredWeapon, 650);
                        int explosionTimer = 10;
                        await Task.Delay(1000);
                        while (explosionTimer > 0 && !plr.Dead)
                        {
                            if (plr.Dead) return;

                            PlayVisuals(plr, 1);

                            NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral(explosionTimer.ToString()), (int)new Color(255, 25, 25).PackedValue, plr.X + 16, plr.Y + 33);
                            explosionTimer--;
                            await Task.Delay(1000);
                        }
                        if (!plr.Dead)
                        {
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, 7, 0, 950, R1_Dmg, 8, plr.Index);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, 5.5f, 5.5f, 950, R1_Dmg, 8, plr.Index);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, 0, 7, 950, R1_Dmg, 8, plr.Index);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, -5.5f, 5.5f, 950, R1_Dmg, 8, plr.Index);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, -7, 0, 950, R1_Dmg, 8, plr.Index);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, -5.5f, -5.5f, 950, R1_Dmg, 8, plr.Index);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, 0, -7, 950, R1_Dmg, 8, plr.Index);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, 5.5f, -5.5f, 950, R1_Dmg, 8, plr.Index);
                        }

                        break;
                    case 2:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Heal!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y - 20);
                        plr.Heal(WorldGen.genRand.Next(10, R2_HP));

                        PlayVisuals(plr, 2);
                        break;
                    case 3:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Stone."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        plr.SetBuff(BuffID.Stoned, 100);

                        PlayVisuals(plr, 3);

                        i = 0;
                        while (i < 15)
                        {
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, WorldGen.genRand.Next(-70, 70) / 10f, WorldGen.genRand.Next(-70, 70) / 10f, ProjectileID.PewMaticHornShot, R3_Dmg, 1.75f, plr.Index);
                            i++;
                            await Task.Delay(50);
                        }
                        break;
                    case 4:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Buff Shuffle?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R4_Amount;

                        PlayVisuals(plr, 4);

                        while (i > 0)
                        {
                            plr.SetBuff(roll4List[WorldGen.genRand.Next(0, roll4List.Length)], R4_Duration);
                            i--;
                        }
                        break;
                    case 5:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Nebula Burst!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R5_Amount;
                        while (i > 0)
                        {
                            switch (WorldGen.genRand.Next(1, 4))
                            {
                                case 1:
                                    Terraria.Item.NewItem(new Terraria.DataStructures.EntitySource_DebugCommand(), (int)plr.X + 16, (int)plr.Y, (int)plr.TPlayer.Size.X, (int)plr.TPlayer.Size.X, Terraria.ID.ItemID.NebulaPickup1, 1);
                                    break;
                                case 2:
                                    Terraria.Item.NewItem(new Terraria.DataStructures.EntitySource_DebugCommand(), (int)plr.X + 16, (int)plr.Y, (int)plr.TPlayer.Size.X, (int)plr.TPlayer.Size.X, Terraria.ID.ItemID.NebulaPickup2, 1);
                                    break;
                                case 3:
                                    Terraria.Item.NewItem(new Terraria.DataStructures.EntitySource_DebugCommand(), (int)plr.X + 16, (int)plr.Y, (int)plr.TPlayer.Size.X, (int)plr.TPlayer.Size.X, Terraria.ID.ItemID.NebulaPickup3, 1);
                                    break;
                            }
                            i--;
                        }
                        break;
                    case 6:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("The dice exploded..?"), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y, 0, 0, ProjectileID.DD2ExplosiveTrapT3Explosion, R6_Dmg, 20, plr.Index);
                        plr.DamagePlayer(R6_Dmg / 4);
                        break;
                    case 7:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Overcharge?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R7_Duration;
                        plr.SetBuff(BuffID.NebulaUpDmg3, 600, true);
                        plr.SetBuff(BuffID.NebulaUpLife3, 600, true);
                        while (i > 0 && !plr.Dead)
                        {
                            PlayVisuals(plr, 7);

                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y + 16, 0, 0, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, plr.Index, 0f, 0.45f);
                            await Task.Delay(100);
                            i -= 100;
                        }
                        if (!plr.Dead) plr.KillPlayer();
                        break;
                    case 8:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Rooted?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R8_Duration;
                        plr.SetBuff(BuffID.Webbed, R8_Duration);
                        while (i > 0 && !plr.Dead)
                        {
                            plr.Heal(R8_HP);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y + 16, WorldGen.genRand.Next(-10, 11) / 10f, WorldGen.genRand.Next(-10, 11) / 10f, ProjectileID.SharpTears, R8_Dmg, 7, plr.Index, 0f, R8_Scale);
                            await Task.Delay(250);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y + 16, WorldGen.genRand.Next(-10, 11) / 10f, WorldGen.genRand.Next(-10, 11) / 10f, ProjectileID.SharpTears, R8_Dmg, 7, plr.Index, 0f, R8_Scale);
                            await Task.Delay(250);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y + 16, WorldGen.genRand.Next(-10, 11) / 10f, WorldGen.genRand.Next(-10, 11) / 10f, ProjectileID.SharpTears, R8_Dmg, 7, plr.Index, 0f, R8_Scale);
                            await Task.Delay(250);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y + 16, WorldGen.genRand.Next(-10, 11) / 10f, WorldGen.genRand.Next(-10, 11) / 10f, ProjectileID.SharpTears, R8_Dmg, 7, plr.Index, 0f, R8_Scale);
                            await Task.Delay(250);
                            i -= 60;
                        }
                        break;
                    case 9:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Teleportation?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R9_Count;
                        await Task.Delay(750);
                        while (i > 0 && !plr.Dead)
                        {
                            PlayVisuals(plr, 9_1);

                            plr.Teleport(WorldGen.genRand.Next(-480, 481) + plr.X, WorldGen.genRand.Next(-480, 481) + plr.Y);
                            AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y + 16, 0, 0, 950, R9_Dmg, 14, plr.Index);
                            await Task.Delay(750);
                            i--;
                        }

                        PlayVisuals(plr, 9_2);

                        plr.Teleport(savedPos.X, savedPos.Y);
                        AbilityExtentions.SpawnProjectile(plr.X + 16, plr.Y + 16, 0, 0, 950, R9_Dmg, 14, plr.Index);
                        break;
                    case 10:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Smoke Bomb!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i2 = R10_Duration;

                        PlayVisuals(plr, 10);

                        plr.SetBuff(BuffID.Invisibility, R10_Ticks);
                        plr.SetBuff(BuffID.WitheredWeapon, R10_Ticks);
                        while (i2 >= 1.33)
                        {
                            plr.SendData(PacketTypes.PlayerDodge, number: plr.Index, number2: 2);
                            i2 -= 0.1f;
                            await Task.Delay(100);
                        }
                        break;
                    case 11:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Magic Cleaver!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        PlayVisuals(plr, 11);

                        foreach (NPC npc in Main.npc)
                        {
                            int cleaveDMG = 0;
                            if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(plr.TPlayer.position, 60 * 16))
                            {
                                if (npc.lifeMax > R11_Cap) cleaveDMG += npc.life / 8;
                                else cleaveDMG += npc.life / 2;
                                if (npc.aiStyle == 6 || npc.netID == 267) cleaveDMG /= 10;
                                else if (npc.aiStyle == 37) cleaveDMG /= 100;
                                TSPlayer.Server.StrikeNPC(npc.whoAmI, cleaveDMG, 0, 0);
                                AbilityExtentions.SpawnProjectile(npc.position.X + 16, npc.position.Y - 160, 0, 12, ProjectileID.LightsBane, 0, 0, plr.Index, 2.5f);
                            }
                        }
                        break;
                    case 12:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Explosive Effect!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        PlayVisuals(plr, 12);

                        AbilityExtentions.ExplosiveEffectState++;
                        await Task.Delay(R12_Duration);
                        AbilityExtentions.ExplosiveEffectState--;
                        break;
                    case 13:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Inked."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        PlayVisuals(plr, 13);

                        plr.SetBuff(BuffID.Obstructed, 600);
                        plr.SetBuff(BuffID.Blackout, 600);
                        break;
                    case 14:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Withered."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        PlayVisuals(plr, 14);

                        plr.SetBuff(BuffID.WitheredArmor, 900);
                        plr.SetBuff(BuffID.WitheredWeapon, 900);
                        break;
                    case 15:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Zapped."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        PlayVisuals(plr, 15);

                        plr.DamagePlayer(R15_Dmg);
                        plr.SetBuff(BuffID.Electrified, 600);
                        break;
                }
            });

        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            int num = (int)args[1];

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(plr.X + 16, plr.Y),
                UniqueInfoPiece = 1
            };

            ParticleOrchestraSettings settings2 = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(plr.X + 16, plr.Y + 16),
                UniqueInfoPiece = 1
            };

            switch (num)
            {
                case 0:
                    NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
                        Terraria.Localization.NetworkText.FromLiteral("You roll..."),
                        (int)new Color(255, 255, 255).PackedValue, plr.X + 16, plr.Y - 20);

                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    break;
                case 1:
                    settings2.PositionInWorld.X = plr.X + 16;
                    settings2.PositionInWorld.Y = plr.Y + 16;
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings2);
                    break;
                case 2:
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings);
                    break;
                case 3:
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PetExchange, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PetExchange, settings);
                    break;
                case 4:
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPCSend, settings);
                    break;
                case 7:
                    settings2.PositionInWorld.X = plr.X + 16;
                    settings2.PositionInWorld.Y = plr.Y + 16;
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings2);
                    break;
                case 9_1:
                    settings2.PositionInWorld.X = plr.X + 16;
                    settings2.PositionInWorld.Y = plr.Y + 16;
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings2);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings2);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings2);
                    break;
                case 9_2:
                    settings2.PositionInWorld.X = plr.X + 16;
                    settings2.PositionInWorld.Y = plr.Y + 16;
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings2);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings2);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings2);
                    break;
                case 10:
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PetExchange, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PetExchange, settings);
                    break;
                case 11:
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                    break;
                case 12:
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                    break;
                case 13:
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                    break;
                case 14:
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings);
                    break;
                case 15:
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerArrow, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    break;
            }
        }
    }
}
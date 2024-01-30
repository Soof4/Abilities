using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;
using System.Drawing;
using Org.BouncyCastle.Asn1.X509;

namespace Abilities
{
    public class Alchemist : Ability
    {
        private double potionSize, potionLifetime;
        private int pot1Heal, pot2Dmg, pot3Dmg, pot4Duration;

        public Alchemist(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                potionSize = (double)1 + (abilityLevel / 2.0);
                potionLifetime = (double)1 + (abilityLevel / 2.5);
                pot1Heal = (int)8 + (abilityLevel * 4);
                pot2Dmg = (int)((15 + (abilityLevel * 9)) * (1 + (abilityLevel - 1) / 5f));
                pot3Dmg = (int)((8 + (abilityLevel * 4)) * (1 + (abilityLevel - 1) / 10f));
                pot4Duration = (int)180 + (abilityLevel * 60);
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);
            PlayVisuals(plr);

            Task.Run(async () =>
            {
                await Task.Delay(100);
                if (plr.Dead) return;
                HealPotion(plr);
                await Task.Delay(100);
                if (plr.Dead) return;
                DmgPotion(plr);
                await Task.Delay(100);
                if (plr.Dead) return;
                VortexPotion(plr);
                await Task.Delay(100);
                if (plr.Dead) return;
                if (abilityLevel >= 2) CursePotion(plr);
                await Task.Delay(100);
                if (plr.Dead) return;
                if (abilityLevel >= 3) ShieldPotion(plr);
                await Task.Delay(100);
                if (plr.Dead) return;
                if (abilityLevel >= 4) PowerPotion(plr);
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
                UniqueInfoPiece = ItemID.Cauldron
            };

            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
            settings.PositionInWorld.Y = plr.Y + 16;
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerArrow, settings);
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerArrow, settings);
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
        }

        //i seperated these to keep it nice

        private void HealPotion(TSPlayer plr)
        {
            Task.Run(async () =>
            {
                int loops = (int)Math.Round(7 * potionLifetime);
                int scale = (int)(85 * potionSize);
                Vector2 splashPos = new Vector2(plr.X + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
                ParticleOrchestraSettings settings = new()
                {
                    IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                    PositionInWorld = new(plr.X + 16, plr.Y),
                    MovementVector = new(splashPos.X - plr.X, splashPos.Y - plr.Y),
                    UniqueInfoPiece = ItemID.SonarPotion
                };
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
                await Task.Delay(900);
                while (loops-- > 0)
                {
                    ParticleOrchestraSettings settings2 = new()
                    {
                        PositionInWorld = new(splashPos.X, splashPos.Y),
                        UniqueInfoPiece = ItemID.SoulofSight
                    };
                    for (double i = 0; i < Math.Tau; i += 0.261816841)
                    {
                        settings2.MovementVector = new Vector2(
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).X * scale,
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).Y * scale);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings2);
                    }
                    await Task.Delay(300);
                    foreach (TSPlayer aplr in TShock.Players)
                    {
                        if (aplr != null)
                        {
                            if (aplr.Active)
                            {
                                if (!aplr.Dead && aplr.TPlayer.position.WithinRange(splashPos, scale))
                                {
                                    aplr.Heal(pot1Heal);
                                    if (AbilityLevel == 5) aplr.SetBuff(BuffID.Honey, 360);
                                }
                            }
                        }
                    }
                    await Task.Delay(200);
                }
            });
        }
        private void DmgPotion(TSPlayer plr)
        {
            Task.Run(async () =>
            {
                int loops = (int)Math.Round(8 * potionLifetime);
                int scale = (int)(105 * potionSize);
                Vector2 splashPos = new Vector2(plr.X + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
                ParticleOrchestraSettings settings = new()
                {
                    IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                    PositionInWorld = new(plr.X + 16, plr.Y),
                    MovementVector = new(splashPos.X - plr.X, splashPos.Y - plr.Y),
                    UniqueInfoPiece = ItemID.WrathPotion
                };
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
                await Task.Delay(900);
                while (loops-- > 0)
                {
                    ParticleOrchestraSettings settings2 = new()
                    {
                        PositionInWorld = new(splashPos.X, splashPos.Y),
                        UniqueInfoPiece = ItemID.SoulofFright
                    };
                    for (double i = 0; i < Math.Tau; i += 0.261816841)
                    {
                        settings2.MovementVector = new Vector2(
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).X * scale,
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).Y * scale);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings2);
                    }
                    await Task.Delay(300);
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(splashPos, scale + (npc.width / 2) + (npc.height / 2)))
                        {
                            TSPlayer.Server.StrikeNPC(npc.whoAmI, pot2Dmg, 0, 0);
                        }
                    }
                    await Task.Delay(200);
                }
            });
        }
        private void VortexPotion(TSPlayer plr)
        {
            Task.Run(async () =>
            {
                int loops = (int)Math.Round(9 * potionLifetime);
                int scale = (int)(135 * potionSize);
                Vector2 splashPos = new Vector2(plr.X + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
                ParticleOrchestraSettings settings = new()
                {
                    IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                    PositionInWorld = new(plr.X + 16, plr.Y),
                    MovementVector = new(splashPos.X - plr.X, splashPos.Y - plr.Y),
                    UniqueInfoPiece = ItemID.WormholePotion
                };
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
                await Task.Delay(900);
                while (loops-- > 0)
                {
                    ParticleOrchestraSettings settings2 = new()
                    {
                        PositionInWorld = new(splashPos.X, splashPos.Y),
                        UniqueInfoPiece = ItemID.SoulofFlight
                    };
                    for (double i = 0; i < Math.Tau; i += 0.261816841)
                    {
                        settings2.MovementVector = new Vector2(
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).X * scale,
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).Y * scale);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings2);
                    }
                    await Task.Delay(300);
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 && npc.netID != 113 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(splashPos, scale + (npc.width / 2) + (npc.height / 2)))
                        {
                            TSPlayer.Server.StrikeNPC(npc.whoAmI, pot3Dmg, 0, 0);
                            npc.velocity = npc.position.DirectionTo(splashPos) * (scale / 25f);
                            NetMessage.SendData((int)PacketTypes.NpcUpdate, -1, -1, null, npc.whoAmI);
                        }
                    }
                    await Task.Delay(200);
                }
            });
        }
        private void CursePotion(TSPlayer plr)
        {
            Task.Run(async () =>
            {
                int loops = (int)Math.Round(6.5 * potionLifetime);
                int scale = (int)(80 * potionSize);
                Vector2 splashPos = new Vector2(plr.X + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
                ParticleOrchestraSettings settings = new()
                {
                    IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                    PositionInWorld = new(plr.X + 16, plr.Y),
                    MovementVector = new(splashPos.X - plr.X, splashPos.Y - plr.Y),
                    UniqueInfoPiece = ItemID.GravitationPotion
                };
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
                await Task.Delay(900);
                while (loops-- > 0)
                {
                    ParticleOrchestraSettings settings2 = new()
                    {
                        PositionInWorld = new(splashPos.X, splashPos.Y),
                        UniqueInfoPiece = ItemID.SoulofNight
                    };
                    for (double i = 0; i < Math.Tau; i += 0.261816841)
                    {
                        settings2.MovementVector = new Vector2(
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).X * scale,
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).Y * scale);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings2);
                    }
                    await Task.Delay(300);
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(splashPos, scale + (npc.width / 2) + (npc.height / 2)))
                        {
                            npc.AddBuff(BuffID.CursedInferno, pot4Duration);
                            npc.AddBuff(BuffID.Ichor, pot4Duration);
                            if (AbilityLevel == 5)
                            {
                                npc.AddBuff(BuffID.Oiled, pot4Duration);
                                npc.AddBuff(BuffID.BetsysCurse, pot4Duration);
                            }
                            TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
                        }
                    }
                    await Task.Delay(200);
                }
            });
        }
        private void ShieldPotion(TSPlayer plr)
        {
            Task.Run(async () =>
            {
                int loops = (int)Math.Round(2.75 * potionLifetime);
                int scale = (int)(40 * potionSize);
                Vector2 splashPos = new Vector2(plr.X + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
                ParticleOrchestraSettings settings = new()
                {
                    IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                    PositionInWorld = new(plr.X + 16, plr.Y),
                    MovementVector = new(splashPos.X - plr.X, splashPos.Y - plr.Y),
                    UniqueInfoPiece = ItemID.CalmingPotion
                };
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
                await Task.Delay(900);
                while (loops-- > 0)
                {
                    ParticleOrchestraSettings settings2 = new()
                    {
                        PositionInWorld = new(splashPos.X, splashPos.Y),
                        UniqueInfoPiece = ItemID.SoulofMight
                    };
                    for (double i = 0; i < Math.Tau; i += 0.261816841)
                    {
                        settings2.MovementVector = new Vector2(
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).X * scale,
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).Y * scale);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings2);
                    }
                    await Task.Delay(300);
                    foreach (TSPlayer aplr in TShock.Players)
                    {
                        if (aplr != null)
                        {
                            if (aplr.Active)
                            {
                                if (!aplr.Dead && aplr.TPlayer.position.WithinRange(splashPos, scale))
                                {
                                    aplr.SendData(PacketTypes.PlayerDodge, number: plr.Index, number2: 2);
                                }
                            }
                        }
                    }
                    await Task.Delay(200);
                }
            });
        }
        private void PowerPotion(TSPlayer plr)
        {
            Task.Run(async () =>
            {
                int loops = (int)Math.Round(4.5 * potionLifetime);
                int scale = (int)(80 * potionSize);
                Vector2 splashPos = new Vector2(plr.X + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Extensions.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
                ParticleOrchestraSettings settings = new()
                {
                    IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                    PositionInWorld = new(plr.X + 16, plr.Y),
                    MovementVector = new(splashPos.X - plr.X, splashPos.Y - plr.Y),
                    UniqueInfoPiece = ItemID.BiomeSightPotion
                };
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
                await Task.Delay(900);
                while (loops-- > 0)
                {
                    ParticleOrchestraSettings settings2 = new()
                    {
                        PositionInWorld = new(splashPos.X, splashPos.Y),
                        UniqueInfoPiece = ItemID.SoulofLight
                    };
                    for (double i = 0; i < Math.Tau; i += 0.261816841)
                    {
                        settings2.MovementVector = new Vector2(
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).X * scale,
                            new Vector2(splashPos.X, splashPos.Y).DirectionTo(new Vector2(splashPos.X + 10 * (float)Math.Cos(i), splashPos.Y + 10 * (float)Math.Sin(i))).Y * scale);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings2);
                    }
                    await Task.Delay(300);
                    foreach (TSPlayer aplr in TShock.Players)
                    {
                        if (aplr != null)
                        {
                            if (aplr.Active)
                            {
                                if (!aplr.Dead && aplr.TPlayer.position.WithinRange(splashPos, scale))
                                {
                                    if (AbilityLevel < 5) aplr.SetBuff(BuffID.NebulaUpDmg2, 90);
                                    else aplr.SetBuff(BuffID.NebulaUpDmg3, 90);
                                }
                            }
                        }
                    }
                    await Task.Delay(200);
                }
            });
        }
    }
}

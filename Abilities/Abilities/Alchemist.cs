using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace Abilities
{
    // Author of this ability is @strangelad on Discord
    public class Alchemist : Ability
    {
        public double PotionSize, PotionLifetime;
        public int Pot1Heal, Pot2Dmg, Pot3Dmg, Pot4Duration;

        public Alchemist(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                PotionSize = (double)1 + (AbilityLevel / 2.0);
                PotionLifetime = (double)1 + (AbilityLevel / 2.5);
                Pot1Heal = (int)8 + (AbilityLevel * 4);
                Pot2Dmg = (int)((15 + (AbilityLevel * 9)) * (1 + (AbilityLevel - 1) / 5f));
                Pot3Dmg = (int)((8 + (AbilityLevel * 4)) * (1 + (AbilityLevel - 1) / 10f));
                Pot4Duration = (int)180 + (AbilityLevel * 60);
            };
        }

        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
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
                int loops = (int)Math.Round(7 * PotionLifetime);
                int scale = (int)(85 * PotionSize);
                Vector2 splashPos = new Vector2(plr.X + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
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
                        if (aplr.IsAlive() && aplr.TPlayer.position.WithinRange(splashPos + new Vector2(16, 40), scale) && Utils.CanDamageThisPlayer(plr, aplr) == false)
                        {
                            aplr.Heal(Pot1Heal);
                            if (AbilityLevel == 5) aplr.SetBuff(BuffID.Honey, 360);
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
                int loops = (int)Math.Round(8 * PotionLifetime);
                int scale = (int)(105 * PotionSize);
                Vector2 splashPos = new Vector2(plr.X + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
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
                        if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(splashPos + new Vector2(npc.width / 2, npc.height / 2), scale))
                        {
                            TSPlayer.Server.StrikeNPC(npc.whoAmI, Pot2Dmg, 0, 0);
                        }
                    }

                    foreach (TSPlayer aplr in TShock.Players)
                    {
                        if (aplr.IsAlive() && aplr.TPlayer.position.WithinRange(splashPos + new Vector2(16, 40), scale) && Utils.CanDamageThisPlayer(plr, aplr) == true)
                        {
                            aplr.DamagePlayer(Pot2Dmg);
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
                int loops = (int)Math.Round(9 * PotionLifetime);
                int scale = (int)(135 * PotionSize);

                Vector2 splashPos = new Vector2(plr.X + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
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
                        if (npc.IsAlive() && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 && npc.netID != 113 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(splashPos + new Vector2(npc.width / 2, npc.height / 2), scale))
                        {
                            TSPlayer.Server.StrikeNPC(npc.whoAmI, Pot3Dmg, 0, 0);
                            npc.velocity = npc.position.DirectionTo(splashPos) * (scale / 25f);
                            NetMessage.SendData((int)PacketTypes.NpcUpdate, -1, -1, null, npc.whoAmI);
                        }
                    }

                    foreach (TSPlayer aplr in TShock.Players)
                    {
                        if (aplr.IsAlive() && aplr.TPlayer.position.WithinRange(splashPos + new Vector2(16, 40), scale) && Utils.CanDamageThisPlayer(plr, aplr) == true)
                        {
                            aplr.DamagePlayer(Pot3Dmg);
                            aplr.TPlayer.velocity = aplr.TPlayer.position.DirectionTo(splashPos) * (scale / 25f);
                            TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", plr.Index);
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
                int loops = (int)Math.Round(6.5 * PotionLifetime);
                int scale = (int)(80 * PotionSize);
                double gameModeBuffFactor = 1;

                if (Main.GameMode == GameModeID.Master)
                {
                    gameModeBuffFactor = 2.5;
                }
                else if (Main.GameMode == GameModeID.Expert)
                {
                    gameModeBuffFactor = 2;
                }

                Vector2 splashPos = new Vector2(plr.X + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));
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
                        if (npc.IsAlive() && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(splashPos + new Vector2(npc.width / 2, npc.height / 2), scale))
                        {
                            npc.AddBuff(BuffID.CursedInferno, Pot4Duration);
                            npc.AddBuff(BuffID.Ichor, Pot4Duration);
                            if (AbilityLevel == 5)
                            {
                                npc.AddBuff(BuffID.Oiled, Pot4Duration);
                                npc.AddBuff(BuffID.BetsysCurse, Pot4Duration);
                            }
                            TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
                        }
                    }
                    foreach (TSPlayer aplr in TShock.Players)
                    {
                        if (aplr.IsAlive() && aplr.TPlayer.position.WithinRange(splashPos + new Vector2(16, 40), scale) && Utils.CanDamageThisPlayer(plr, aplr) == true)
                        {
                            aplr.SetBuff(BuffID.CursedInferno, (int)(Pot4Duration / gameModeBuffFactor));
                            aplr.SetBuff(BuffID.Ichor, (int)(Pot4Duration / gameModeBuffFactor));
                            if (AbilityLevel == 5)
                            {
                                aplr.SetBuff(BuffID.Oiled, Pot4Duration);
                                aplr.SetBuff(BuffID.BetsysCurse, Pot4Duration);
                            }
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
                int loops = (int)Math.Round(2.75 * PotionLifetime);
                int scale = (int)(40 * PotionSize);
                Vector2 splashPos = new Vector2(plr.X + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));

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
                        if (aplr.IsAlive() && aplr.TPlayer.position.WithinRange(splashPos + new Vector2(16, 40), scale) && Utils.CanDamageThisPlayer(plr, aplr) == false)
                        {
                            aplr.SendData(PacketTypes.PlayerDodge, number: plr.Index, number2: 2);
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
                int loops = (int)Math.Round(4.5 * PotionLifetime);
                int scale = (int)(80 * PotionSize);
                Vector2 splashPos = new Vector2(plr.X + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1), plr.Y + Utils.Random.Next((int)(scale * -1.6f), (int)(scale * 1.6) + 1));

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
                        if (aplr.IsAlive() && aplr.TPlayer.position.WithinRange(splashPos + new Vector2(16, 40), scale) && Utils.CanDamageThisPlayer(plr, aplr) == false)
                        {
                            if (AbilityLevel < 5) aplr.SetBuff(BuffID.NebulaUpDmg2, 90);
                            else aplr.SetBuff(BuffID.NebulaUpDmg3, 90);
                        }
                    }
                    await Task.Delay(200);
                }
            });
        }
    }
}

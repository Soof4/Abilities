using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace Abilities.Abilities
{
    public class SilentOrchestra : Ability
    {
        public double RealAbilityTime;
        public int BaseDamage, RealBaseDamage, Movement4Counter;
        public bool HitEnemy;

        public SilentOrchestra(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                BaseDamage = 7 + ((AbilityLevel - 1) * 3);
            };

            UpdateStats();
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr, 0);
            int AbilityLoops = 0;
            bool movement2 = false;
            bool movement3 = false;
            bool movement4 = false;
            double TimeDecrease = 1;
            RealAbilityTime = 20;
            RealBaseDamage = BaseDamage;
            Task.Run(async () =>
            {
                NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
                                       Terraria.Localization.NetworkText.FromLiteral("Movement 1"),
                                       (int)new Color(255, 255, 255).PackedValue, plr.X + 16, plr.Y - 20);
                while (RealAbilityTime > 0)
                {
                    CalcTime(plr, RealAbilityTime);
                    HitEnemy = false;
                    if (AbilityLoops >= 45)
                    {
                        Movement4(plr);
                        if (!movement4)
                        {
                            movement4 = true;
                            NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
                                       Terraria.Localization.NetworkText.FromLiteral("Movement 4"),
                                       (int)new Color(255, 255, 255).PackedValue, plr.X + 16, plr.Y - 20);
                        }
                    }
                    await Task.Delay(25);
                    if (AbilityLoops >= 30)
                    {
                        Movement3(plr);
                        if (!movement3)
                        {
                            movement3 = true;
                            NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
                                       Terraria.Localization.NetworkText.FromLiteral("Movement 3"),
                                       (int)new Color(255, 255, 255).PackedValue, plr.X + 16, plr.Y - 20);
                        }
                    }
                    await Task.Delay(25);
                    if (AbilityLoops >= 15)
                    {
                        Movement2(plr);
                        if (!movement2)
                        {
                            movement2 = true;
                            NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
                                       Terraria.Localization.NetworkText.FromLiteral("Movement 2"),
                                       (int)new Color(255, 255, 255).PackedValue, plr.X + 16, plr.Y - 20);
                        }
                    }
                    await Task.Delay(25);
                    Movement1(plr);
                    if (movement4) HitSound(plr, 4);
                    else if (movement3) HitSound(plr, 3);
                    else if (movement2) HitSound(plr, 2);
                    else HitSound(plr, 1);
                    AbilityLoops++;
                    await Task.Delay(500);
                    if (HitEnemy) RealAbilityTime -= TimeDecrease / 4;
                    else
                    {
                        RealAbilityTime -= TimeDecrease;
                        TimeDecrease *= 1.1;
                    }
                    await Task.Delay(425);
                    if (plr.Dead) RealAbilityTime = 0;
                }
                if (AbilityLoops >= 46)
                {
                    int counter = 5;
                    counter += Movement4Counter / 5;
                    if (plr.Dead) counter += 15;
                    Finale(plr, counter);
                    NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
                                       Terraria.Localization.NetworkText.FromLiteral("Finale"),
                                       (int)new Color(255, 255, 255).PackedValue, plr.X + 16, plr.Y - 20);
                    await Task.Delay(1500);
                }
                EndAbility(plr);
            });
        }

        protected override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            int num = (int)args[1];
            switch (num)
            {
                case 0:
                    ParticleOrchestraSettings settings = new()
                    {
                        IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                        MovementVector = new(0, 0),
                        PositionInWorld = new(plr.X + 16, plr.Y),
                        UniqueInfoPiece = 1
                    };

                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    break;

                case 1:
                    ParticleOrchestraSettings SparkleSettings = new()
                    {
                        PositionInWorld = new(plr.X, plr.Y)
                    };

                    for (double i = 0; i < Math.Tau; i += 0.3926991)
                    {
                        SparkleSettings.MovementVector = new Vector2(
                            new Vector2(SparkleSettings.PositionInWorld.X, SparkleSettings.PositionInWorld.Y).DirectionTo(new Vector2(SparkleSettings.PositionInWorld.X + 10 * (float)Math.Cos(i), SparkleSettings.PositionInWorld.Y + 10 * (float)Math.Sin(i))).X * 15,
                            new Vector2(SparkleSettings.PositionInWorld.X, SparkleSettings.PositionInWorld.Y).DirectionTo(new Vector2(SparkleSettings.PositionInWorld.X + 10 * (float)Math.Cos(i), SparkleSettings.PositionInWorld.Y + 10 * (float)Math.Sin(i))).Y * 15);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PaladinsHammer, SparkleSettings);
                    }
                    break;
                case 2:
                    Vector2 Pos = plr.TPlayer.position;
                    int sizeCounter = 64;
                    Task.Run(async () =>
                    {
                        while (sizeCounter < 896)
                        {
                            makeCircle2(Pos.X, Pos.Y, sizeCounter, 2.0944, 1.5708, (byte)plr.Index, ItemID.DiamondGemsparkWall);
                            makeCircle2(Pos.X, Pos.Y, sizeCounter, 2.0944, 4.71239, (byte)plr.Index, ItemID.DiamondGemsparkWallOff);
                            sizeCounter += 64;
                            await Task.Delay(150);
                        }
                    });
                    break;
            }
        }
        private void EndAbility(TSPlayer plr)
        {
            plr.SetBuff(BuffID.WitheredArmor, 1200, true);
            plr.SetBuff(BuffID.WitheredWeapon, 1200, true);
        }
        private void Movement1(TSPlayer plr)
        {
            Vector2 Pos = plr.TPlayer.position;
            Task.Run(async () =>
            {
                makeCircle2(Pos.X, Pos.Y, 150, 1.5708, 0, (byte)plr.Index, ItemID.DiamondGemsparkWall);
                makeCircle2(Pos.X, Pos.Y, 150, 1.5708, 0.785415617, (byte)plr.Index, ItemID.DiamondGemsparkWallOff);
                await Task.Delay(375);
                foreach (NPC npc in Main.npc)
                {
                    if (Utils.CanDamageThisEnemy(npc) &&
                    npc.position.WithinRange(Pos - new Vector2(npc.width / 2, npc.height / 2), 150))
                    {
                        if (!npc.SpawnedFromStatue && npc.type != 488 && npc.type != 210 && npc.type != 211) HitEnemy = true;
                        TSPlayer.Server.StrikeNPC(npc.whoAmI, RealBaseDamage + (npc.defense / 2), 0, 0);
                        ParticleOrchestraSettings settings = new()
                        {
                            PositionInWorld = new(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2)
                        };
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    }
                }
                foreach (TSPlayer aplr in TShock.Players)
                {
                    if (aplr != null)
                    {
                        if (aplr.Active)
                        {
                            if (!aplr.Dead && aplr.TPlayer.position.WithinRange(Pos - new Vector2(16, 40), 150) && Utils.CanDamageThisPlayer(plr, aplr) == true)
                            {
                                aplr.DamagePlayer((int)(RealBaseDamage * 1.5) + (aplr.TPlayer.statDefense / 2));
                                HitEnemy = true;
                                ParticleOrchestraSettings settings = new()
                                {
                                    PositionInWorld = new(aplr.TPlayer.position.X + 8, aplr.TPlayer.position.Y + 24)
                                };
                                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                            }
                        }
                    }
                }
            });
        }
        private void Movement2(TSPlayer plr)
        {
            Vector2 Pos = plr.TPlayer.position;
            plr.SetBuff(BuffID.NebulaUpLife1, 120, true);
            Task.Run(async () =>
            {
                makeCircle2(Pos.X, Pos.Y, 250, 0.785415617, 0, (byte)plr.Index, ItemID.AmberGemsparkWall);
                makeCircle2(Pos.X, Pos.Y, 250, 0.785415617, 0.3926991, (byte)plr.Index, ItemID.AmberGemsparkWallOff);
                await Task.Delay(375);
                foreach (NPC npc in Main.npc)
                {
                    if (Utils.CanDamageThisEnemy(npc) &&
                    npc.position.WithinRange(Pos - new Vector2(npc.width / 2, npc.height / 2), 250))
                    {
                        if (!npc.SpawnedFromStatue && npc.type != 488 && npc.type != 210 && npc.type != 211) HitEnemy = true;
                        TSPlayer.Server.StrikeNPC(npc.whoAmI, (int)(RealBaseDamage * 1.25) + (npc.defense / 2), 0, 0);
                        ParticleOrchestraSettings settings = new()
                        {
                            PositionInWorld = new(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2)
                        };
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    }
                }
                foreach (TSPlayer aplr in TShock.Players)
                {
                    if (aplr != null)
                    {
                        if (aplr.Active)
                        {
                            if (!aplr.Dead && aplr.TPlayer.position.WithinRange(Pos - new Vector2(16, 40), 250) && Utils.CanDamageThisPlayer(plr, aplr) == true)
                            {
                                aplr.DamagePlayer((int)(RealBaseDamage * 2) + (aplr.TPlayer.statDefense / 2));
                                HitEnemy = true;
                                ParticleOrchestraSettings settings = new()
                                {
                                    PositionInWorld = new(aplr.TPlayer.position.X + 8, aplr.TPlayer.position.Y + 24)
                                };
                                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                            }
                        }
                    }
                }
            });
        }
        private void Movement3(TSPlayer plr)
        {
            Vector2 Pos = plr.TPlayer.position;
            plr.SetBuff(BuffID.NebulaUpMana1, 120, true);
            Task.Run(async () =>
            {
                makeCircle2(Pos.X, Pos.Y, 350, 0.3926991, 0, (byte)plr.Index, ItemID.SapphireGemsparkWall);
                makeCircle2(Pos.X, Pos.Y, 350, 0.3926991, 0.1963512862, (byte)plr.Index, ItemID.SapphireGemsparkWallOff);
                await Task.Delay(375);
                foreach (NPC npc in Main.npc)
                {
                    if (Utils.CanDamageThisEnemy(npc) &&
                    npc.position.WithinRange(Pos - new Vector2(npc.width / 2, npc.height / 2), 350))
                    {
                        if (!npc.SpawnedFromStatue && npc.type != 488 && npc.type != 210 && npc.type != 211) HitEnemy = true;
                        TSPlayer.Server.StrikeNPC(npc.whoAmI, (int)(RealBaseDamage * 1.5) + (npc.defense / 2), 0, 0);
                        ParticleOrchestraSettings settings = new()
                        {
                            PositionInWorld = new(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2)
                        };
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    }
                }
                foreach (TSPlayer aplr in TShock.Players)
                {
                    if (aplr != null)
                    {
                        if (aplr.Active)
                        {
                            if (!aplr.Dead && aplr.TPlayer.position.WithinRange(Pos - new Vector2(16, 40), 350) && Utils.CanDamageThisPlayer(plr, aplr) == true)
                            {
                                aplr.DamagePlayer((int)(RealBaseDamage * 2.5) + (aplr.TPlayer.statDefense / 2));
                                HitEnemy = true;
                                ParticleOrchestraSettings settings = new()
                                {
                                    PositionInWorld = new(aplr.TPlayer.position.X + 8, aplr.TPlayer.position.Y + 24)
                                };
                                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                            }
                        }
                    }
                }
            });
        }
        private void Movement4(TSPlayer plr)
        {
            Movement4Counter++;
            Vector2 Pos = plr.TPlayer.position;
            plr.SetBuff(BuffID.NebulaUpDmg1, 120, true);
            Task.Run(async () =>
            {
                makeCircle2(Pos.X, Pos.Y, 450, 0.1963512862, 0, (byte)plr.Index, ItemID.AmethystGemsparkWall);
                makeCircle2(Pos.X, Pos.Y, 450, 0.1963512862, 0.0981765158, (byte)plr.Index, ItemID.AmethystGemsparkWallOff);
                await Task.Delay(375);
                foreach (NPC npc in Main.npc)
                {
                    if (Utils.CanDamageThisEnemy(npc) &&
                    npc.position.WithinRange(Pos - new Vector2(npc.width / 2, npc.height / 2), 450))
                    {
                        if (!npc.SpawnedFromStatue && npc.type != 488 && npc.type != 210 && npc.type != 211) HitEnemy = true;
                        TSPlayer.Server.StrikeNPC(npc.whoAmI, (int)(RealBaseDamage * 1.75) + (npc.defense / 2), 0, 0);
                        ParticleOrchestraSettings settings = new()
                        {
                            PositionInWorld = new(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2)
                        };
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                    }
                }
                foreach (TSPlayer aplr in TShock.Players)
                {
                    if (aplr != null)
                    {
                        if (aplr.Active)
                        {
                            if (!aplr.Dead && aplr.TPlayer.position.WithinRange(Pos - new Vector2(16, 40), 450) && Utils.CanDamageThisPlayer(plr, aplr) == true)
                            {
                                aplr.DamagePlayer((int)(RealBaseDamage * 3) + (aplr.TPlayer.statDefense / 2));
                                HitEnemy = true;
                                ParticleOrchestraSettings settings = new()
                                {
                                    PositionInWorld = new(aplr.TPlayer.position.X + 8, aplr.TPlayer.position.Y + 24)
                                };
                                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                            }
                        }
                    }
                }
            });
        }
        private void Finale(TSPlayer plr, int count)
        {
            (ushort, int) sound1 = Utils.GetSoundIndexAndId(2, 159);
            (ushort, int) sound2 = Utils.GetSoundIndexAndId(2, 145);
            int loopCount = count;
            plr.SetBuff(BuffID.Webbed, 180, true);
            Vector2 Pos = plr.TPlayer.position;
            Task.Run(async () =>
            {
                if (!plr.Dead) await Task.Delay(1000);
                plr.SetBuff(BuffID.NebulaUpLife3, 300, true);
                PlayVisuals(plr, 1);
                PlayVisuals(plr, 2);
                NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound1.Item1, sound1.Item2, 1f, 1f));
                NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound2.Item1, sound2.Item2, 2f, -0.1f));
                makeCircle2(Pos.X, Pos.Y, 900, 0.1963512862, 0, (byte)plr.Index, ItemID.DiamondGemsparkWall);
                makeCircle2(Pos.X, Pos.Y, 900, 0.1963512862, 0.0981765158, (byte)plr.Index, ItemID.DiamondGemsparkWallOff);
                await Task.Delay(375);
                while (loopCount > 0)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (Utils.CanDamageThisEnemy(npc) &&
                    npc.position.WithinRange(Pos - new Vector2(npc.width / 2, npc.height / 2), 900))
                        {
                            TSPlayer.Server.StrikeNPC(npc.whoAmI, (RealBaseDamage * 3) + (npc.defense / 2), 0, 0);
                            ParticleOrchestraSettings settings = new()
                            {
                                PositionInWorld = new(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2)
                            };
                            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                        }
                    }
                    foreach (TSPlayer aplr in TShock.Players)
                    {
                        if (aplr != null)
                        {
                            if (aplr.Active)
                            {
                                if (!aplr.Dead && aplr.TPlayer.position.WithinRange(Pos - new Vector2(16, 40), 900) && Utils.CanDamageThisPlayer(plr, aplr) == true)
                                {
                                    aplr.DamagePlayer((RealBaseDamage * 6) + (aplr.TPlayer.statDefense / 2));
                                    ParticleOrchestraSettings settings = new()
                                    {
                                        PositionInWorld = new(aplr.TPlayer.position.X + 8, aplr.TPlayer.position.Y + 24)
                                    };
                                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                                }
                            }
                        }
                    }
                    loopCount--;
                    await Task.Delay(150);
                }
            });
        }

        private void makeCircle2(float posX, float posY, float size, double deltaRadian, double startRadian, byte indexOfPlayerWhoInvokedThis, int uniqueInfoPiece = 1)
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
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
            }
        }

        private void HitSound(TSPlayer plr, int movement)
        {
            (ushort, int) sound1 = Utils.GetSoundIndexAndId(2, 133);
            (ushort, int) sound2 = Utils.GetSoundIndexAndId(2, 134);
            (ushort, int) sound3 = Utils.GetSoundIndexAndId(2, 135);
            (ushort, int) sound4 = Utils.GetSoundIndexAndId(2, 136);
            (ushort, int) sound5 = Utils.GetSoundIndexAndId(2, 137);
            (ushort, int) sound6 = Utils.GetSoundIndexAndId(2, 138);
            (ushort, int) dumbassflutesound1 = Utils.GetSoundIndexAndId(42, 268);
            (ushort, int) dumbassflutesound2 = Utils.GetSoundIndexAndId(42, 269);
            (ushort, int) dumbassflutesound3 = Utils.GetSoundIndexAndId(42, 270);
            if (movement == 1)
                switch (Utils.Random.Next(6))
                {
                    case 0:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound1.Item1, sound1.Item2, 0.3f));
                        break;
                    case 1:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound2.Item1, sound2.Item2, 0.3f));
                        break;
                    case 2:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound3.Item1, sound3.Item2, 0.3f));
                        break;
                    case 3:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound4.Item1, sound4.Item2, 0.3f));
                        break;
                    case 4:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound5.Item1, sound5.Item2, 0.3f));
                        break;
                    case 5:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound6.Item1, sound6.Item2, 0.3f));
                        break;
                }
            else if (movement == 2)
                switch (Utils.Random.Next(6))
                {
                    case 0:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound1.Item1, sound1.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound2.Item1, sound2.Item2, 0.15f, -0.25f));
                        break;
                    case 1:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound2.Item1, sound2.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound3.Item1, sound3.Item2, 0.15f, -0.25f));
                        break;
                    case 2:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound3.Item1, sound3.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound4.Item1, sound4.Item2, 0.15f, -0.25f));
                        break;
                    case 3:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound4.Item1, sound4.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound5.Item1, sound5.Item2, 0.15f, -0.25f));
                        break;
                    case 4:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound5.Item1, sound5.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound6.Item1, sound6.Item2, 0.15f, -0.25f));
                        break;
                    case 5:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound6.Item1, sound6.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound1.Item1, sound1.Item2, 0.15f, -0.25f));
                        break;
                }
            else if (movement == 3)
                switch (Utils.Random.Next(2))
                {
                    case 0:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, dumbassflutesound2.Item1, dumbassflutesound2.Item2, 0.4f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, dumbassflutesound1.Item1, dumbassflutesound1.Item2, 0.4f, -0.2f));
                        break;
                    case 1:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, dumbassflutesound3.Item1, dumbassflutesound3.Item2, 0.4f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, dumbassflutesound1.Item1, dumbassflutesound1.Item2, 0.4f, -0.2f));
                        break;
                }

            else if (movement == 4)
                switch (Utils.Random.Next(6))
                {
                    case 0:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound1.Item1, sound1.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound3.Item1, sound3.Item2, 0.2f, -0.4f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound2.Item1, sound2.Item2, 0.2f, -0.25f));
                        break;
                    case 1:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound2.Item1, sound2.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound4.Item1, sound4.Item2, 0.2f, -0.4f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound3.Item1, sound3.Item2, 0.2f, -0.25f));
                        break;
                    case 2:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound3.Item1, sound3.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound5.Item1, sound5.Item2, 0.2f, -0.4f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound4.Item1, sound4.Item2, 0.2f, -0.25f));
                        break;
                    case 3:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound4.Item1, sound4.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound6.Item1, sound6.Item2, 0.2f, -0.4f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound5.Item1, sound5.Item2, 0.2f, -0.25f));
                        break;
                    case 4:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound5.Item1, sound5.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound1.Item1, sound1.Item2, 0.2f, -0.4f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound6.Item1, sound6.Item2, 0.2f, -0.25f));
                        break;
                    case 5:
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound6.Item1, sound6.Item2, 0.3f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound2.Item1, sound2.Item2, 0.2f, -0.4f));
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, sound1.Item1, sound1.Item2, 0.2f, -0.25f));
                        break;
                }
        }

        private void CalcTime(TSPlayer plr, double Time)
        {
            int visualTime = (int)Time;
            if (visualTime > 10) NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
                                      Terraria.Localization.NetworkText.FromLiteral(visualTime.ToString()),
                                      (int)new Color(100, 255, 100).PackedValue, plr.X + 16, plr.Y);
            else if (visualTime > 5) NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
                                      Terraria.Localization.NetworkText.FromLiteral(visualTime.ToString()),
                                      (int)new Color(255, 255, 100).PackedValue, plr.X + 16, plr.Y);
            else NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
                                      Terraria.Localization.NetworkText.FromLiteral(visualTime.ToString()),
                                      (int)new Color(255, 100, 100).PackedValue, plr.X + 16, plr.Y);
        }
    }
}
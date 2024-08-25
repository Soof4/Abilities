using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace Abilities.Abilities
{
    // Author of this ability is @strangelad on Discord
    public class MagicDice : Ability
    {
        public int R1_Dmg, R2_HP, R3_Dmg, R4_HP, R5_Amount, R6_Dmg, R7_Duration, R7_Dmg, R8_Duration, R8_HP, R8_Dmg,
            R9_Count, R9_Dmg, R10_Ticks, R11_Cap, R12_Range, R13_Duration, R14_Duration, R15_Duration, R15_Dmg, R16_Count,
            R16_Dmg, R17_Duration, R17_Dmg, R18_Duration, R18_Dmg, R19_Duration, R19_Dmg, R20_Health, R21_Dmg, R21_Count;

        public float R8_Scale, R10_Duration;

        int[] roll4List = { BuffID.Blackout, BuffID.VortexDebuff, BuffID.WitheredArmor, BuffID.WitheredWeapon,
            BuffID.CursedInferno, BuffID.MoonLeech, BuffID.Ichor, BuffID.Lucky, BuffID.RapidHealing, BuffID.Endurance,
            BuffID.Thorns, BuffID.Honey, BuffID.Ironskin, BuffID.Regeneration, BuffID.ShadowDodge, BuffID.Invisibility };

        public MagicDice(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                R1_Dmg = (int)((60 + (AbilityLevel - 1) * 35) * (1 + (AbilityLevel - 1) / 5f));
                R2_HP = 50 + (50 * AbilityLevel);
                R3_Dmg = (int)((15 + (AbilityLevel - 1) * 15) * (1 + (AbilityLevel - 1) / 10f));
                R4_HP = (int)(100 + (AbilityLevel - 1) * 50);
                R5_Amount = (int)(1 + AbilityLevel);
                R6_Dmg = (int)((260 + (AbilityLevel - 1) * 210) * (1 + (AbilityLevel - 1) / 5f));
                R7_Duration = (int)(4000 + (AbilityLevel - 1) * 1000);
                R7_Dmg = (int)(15 + (AbilityLevel - 1) * 15);
                R8_Duration = (int)(300 + (AbilityLevel - 1) * 120);
                R8_HP = (int)(20 + (AbilityLevel - 1) * 10);
                R8_Dmg = (int)((30 + (AbilityLevel - 1) * 15) * (1 + (AbilityLevel - 1) / 10f));
                R8_Scale = 1f + (AbilityLevel - 1) * 0.5f;
                R9_Count = (int)(3 + AbilityLevel);
                R9_Dmg = (int)((75 + (AbilityLevel - 1) * 50) * (1 + (AbilityLevel - 1) / 10f));
                R10_Ticks = (int)(300 + (AbilityLevel - 1) * 60);
                R10_Duration = (float)(5 + (AbilityLevel - 1));
                R11_Cap = (int)(300 + 300 * ((AbilityLevel - 1) * (AbilityLevel - 1)));
                R12_Range = (int)(50 + (AbilityLevel - 1) * 25);
                R13_Duration = (int)(300 + (AbilityLevel - 1) * 30);
                R14_Duration = (int)(240 + (AbilityLevel - 1) * 60);
                R15_Duration = (int)(240 + (AbilityLevel - 1) * 30);
                R15_Dmg = (int)(25 * AbilityLevel);
                R16_Count = (int)(2 + AbilityLevel);
                R16_Dmg = (int)((35 + (AbilityLevel - 1) * 20) * (1 + (AbilityLevel - 1) / 5f));
                R17_Duration = (int)(300 + (AbilityLevel - 1) * 60);
                R17_Dmg = (int)((20 + (AbilityLevel - 1) * 15) * (1 + (AbilityLevel - 1) / 5f));
                R18_Duration = (int)(3 + (AbilityLevel - 1) * 2);
                R18_Dmg = (int)(10 + (AbilityLevel - 1) * 10);
                R19_Duration = (int)(300 + (AbilityLevel - 1) * 60);
                R19_Dmg = (int)((20 + (AbilityLevel - 1) * 15) * (1 + (AbilityLevel - 1) / 5f));
                R20_Health = (int)(15 + (AbilityLevel - 1) * 10);
                R21_Dmg = (int)((60 + (AbilityLevel - 1) * 30) * (1 + (AbilityLevel - 1) / 5f));
                R21_Count = (int)(3 + (AbilityLevel - 1) * 2);
            };

            UpdateStats();
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr, 0);

            int i = 0;
            float i2 = 0;
            NPC? target = null;

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
                switch (Utils.Random.Next(1, 22))
                {
                    case 1:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Explosion?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        plr.SetBuff(BuffID.Dazed, 650, true);
                        plr.SetBuff(BuffID.WitheredArmor, 650, true);
                        plr.SetBuff(BuffID.WitheredWeapon, 650, true);
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
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(1, 0)).X * 7, Vector2.Zero.DirectionTo(new Vector2(1, 0)).Y * 7, 950, R1_Dmg, 8, plr.Index);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(1, 1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(1, 1)).Y * 7, 950, R1_Dmg, 8, plr.Index);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(0, 1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(0, 1)).Y * 7, 950, R1_Dmg, 8, plr.Index);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(-1, 1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(-1, 1)).Y * 7, 950, R1_Dmg, 8, plr.Index);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(-1, 0)).X * 7, Vector2.Zero.DirectionTo(new Vector2(-1, 0)).Y * 7, 950, R1_Dmg, 8, plr.Index);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(-1, -1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(-1, -1)).Y * 7, 950, R1_Dmg, 8, plr.Index);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(0, -1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(0, -1)).Y * 7, 950, R1_Dmg, 8, plr.Index);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(1, -1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(1, -1)).Y * 7, 950, R1_Dmg, 8, plr.Index);
                        }

                        break; //explosion
                    case 2:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Heal!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y - 20);
                        plr.Heal(Utils.Random.Next(10, R2_HP));

                        PlayVisuals(plr, 2);
                        break; //heal
                    case 3:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Stone."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        plr.SetBuff(BuffID.Stoned, 120, true);

                        PlayVisuals(plr, 3);

                        i = 0;
                        while (i < 15)
                        {
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Utils.Random.Next(-70, 70) / 10f, Utils.Random.Next(-70, 70) / 10f, ProjectileID.PewMaticHornShot, R3_Dmg, 1.75f, plr.Index);
                            i++;
                            await Task.Delay(50);
                        }
                        break; //stone
                    case 4:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Sacrifice?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        int dmgCounter = 0;
                        await Task.Delay(500);
                        foreach (TSPlayer aplr in TShock.Players)
                        {
                            if (aplr != null)
                            {
                                if (aplr.Active)
                                {
                                    if (!aplr.Dead && aplr != plr && aplr.TPlayer.position.WithinRange(plr.TPlayer.position, 1600))
                                    {
                                        aplr.Heal(R4_HP);
                                        dmgCounter += R4_HP / 2;
                                    }
                                }
                            }
                        }
                        if (dmgCounter == 0) NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("But there was nobody else."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        else plr.DamagePlayer(dmgCounter);
                        break; //sacrifice
                    case 5:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Nebula Burst!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R5_Amount;
                        while (i > 0)
                        {
                            switch (Utils.Random.Next(1, 4))
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
                        break; //nebula burst
                    case 6:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("The dice exploded..?"), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        Utils.SpawnProjectile(plr.X + 16, plr.Y, 0, 0, ProjectileID.DD2ExplosiveTrapT3Explosion, R6_Dmg, 20, plr.Index);
                        plr.DamagePlayer(R6_Dmg / 4);
                        break; //dice explosion
                    case 7:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Overcharge?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R7_Duration;
                        plr.SetBuff(BuffID.NebulaUpDmg3, 600, true);
                        plr.SetBuff(BuffID.NebulaUpLife3, 600, true);
                        while (i > 0 && !plr.Dead)
                        {
                            PlayVisuals(plr, 7);

                            Utils.SpawnProjectile(plr.X + 16, plr.Y + 16, 0, 0, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, -1, 0f, 0.45f);
                            await Task.Delay(100);
                            i -= 100;
                        }
                        if (!plr.Dead)
                        {
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(1, 0)).X * 7, Vector2.Zero.DirectionTo(new Vector2(1, 0)).Y * 7, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, -1, 0f, 0.45f);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(1, 1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(1, 1)).Y * 7, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, -1, 0f, 0.45f);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(0, 1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(0, 1)).Y * 7, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, -1, 0f, 0.45f);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(-1, 1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(-1, 1)).Y * 7, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, -1, 0f, 0.45f);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(-1, 0)).X * 7, Vector2.Zero.DirectionTo(new Vector2(-1, 0)).Y * 7, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, -1, 0f, 0.45f);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(-1, -1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(-1, -1)).Y * 7, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, -1, 0f, 0.45f);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(0, -1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(0, -1)).Y * 7, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, -1, 0f, 0.45f);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y, Vector2.Zero.DirectionTo(new Vector2(1, -1)).X * 7, Vector2.Zero.DirectionTo(new Vector2(1, -1)).Y * 7, ProjectileID.FairyQueenMagicItemShot, R7_Dmg, 2, -1, 0f, 0.45f);
                            plr.KillPlayer();
                        }
                        break; //overcharge
                    case 8:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Rooted?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R8_Duration;
                        plr.SetBuff(BuffID.Webbed, R8_Duration, true);
                        while (i > 0 && !plr.Dead)
                        {
                            plr.Heal(R8_HP);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y + 16, Utils.Random.Next(-10, 11) / 10f, Utils.Random.Next(-10, 11) / 10f, ProjectileID.SharpTears, R8_Dmg, 7, plr.Index, 0f, R8_Scale);
                            await Task.Delay(250);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y + 16, Utils.Random.Next(-10, 11) / 10f, Utils.Random.Next(-10, 11) / 10f, ProjectileID.SharpTears, R8_Dmg, 7, plr.Index, 0f, R8_Scale);
                            await Task.Delay(250);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y + 16, Utils.Random.Next(-10, 11) / 10f, Utils.Random.Next(-10, 11) / 10f, ProjectileID.SharpTears, R8_Dmg, 7, plr.Index, 0f, R8_Scale);
                            await Task.Delay(250);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y + 16, Utils.Random.Next(-10, 11) / 10f, Utils.Random.Next(-10, 11) / 10f, ProjectileID.SharpTears, R8_Dmg, 7, plr.Index, 0f, R8_Scale);
                            await Task.Delay(250);
                            i -= 60;
                        }
                        break; //rooted
                    case 9:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Teleportation?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R9_Count;
                        await Task.Delay(750);
                        while (i > 0 && !plr.Dead)
                        {
                            PlayVisuals(plr, 9_1);

                            plr.Teleport(Utils.Random.Next(-480, 481) + plr.X, Utils.Random.Next(-480, 481) + plr.Y);
                            Utils.SpawnProjectile(plr.X + 16, plr.Y + 16, 0, 0, 950, R9_Dmg, 14, plr.Index);
                            await Task.Delay(750);
                            i--;
                        }

                        PlayVisuals(plr, 9_2);

                        plr.Teleport(savedPos.X, savedPos.Y);
                        Utils.SpawnProjectile(plr.X + 16, plr.Y + 16, 0, 0, 950, R9_Dmg, 14, plr.Index);
                        break; //teleportation
                    case 10:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Smoke Bomb!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i2 = R10_Duration;

                        PlayVisuals(plr, 10);

                        plr.SetBuff(BuffID.Invisibility, R10_Ticks, true);
                        plr.SetBuff(BuffID.WitheredWeapon, R10_Ticks, true);
                        while (i2 >= 1.33)
                        {
                            plr.SendData(PacketTypes.PlayerDodge, number: plr.Index, number2: 2);
                            i2 -= 0.1f;
                            await Task.Delay(100);
                        }
                        break; //smoke bomb
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
                                Utils.SpawnProjectile(npc.position.X + 16, npc.position.Y - 160, 0, 12, ProjectileID.LightsBane, 0, 0, plr.Index, 2.5f);
                            }
                        }
                        break; //magic cleaver
                    case 12:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Explosive Effect!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        PlayVisuals(plr, 12);
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc != null && npc.active && npc.rarity != 11 && npc.type != 0 && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 &&
                                !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(plr.TPlayer.position, R12_Range * 16))
                            {
                                Utils.SpawnProjectile(npc.position.X + 16, npc.position.Y, 0, 0, 950, 0, 8, -1);
                                npc.rarity = 10;
                            }
                        }
                        break; //explosive effect
                    case 13:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Inked."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        PlayVisuals(plr, 13);

                        plr.SetBuff(BuffID.Obstructed, R13_Duration, true);
                        break; //inked
                    case 14:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Withered."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        PlayVisuals(plr, 14);

                        plr.SetBuff(BuffID.WitheredArmor, R14_Duration, true);
                        plr.SetBuff(BuffID.WitheredWeapon, R14_Duration, true);
                        break; //withered
                    case 15:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Zapped."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        PlayVisuals(plr, 15);

                        plr.DamagePlayer(R15_Dmg);
                        plr.SetBuff(BuffID.Electrified, R15_Duration, true);
                        break; //zapped
                    case 16:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Hyper Dash!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y);

                        i = R16_Count;
                        while (i > 0)
                        {
                            target = Utils.GetNearestEnemy(plr.TPlayer.position);
                            if (target != null && !plr.Dead)
                            {
                                PlayVisuals(plr, 13);
                                plr.SendData(PacketTypes.PlayerDodge, number: (byte)plr.Index, number2: 2);
                                plr.TPlayer.velocity = plr.TPlayer.position.DirectionTo(target.position) * 30;
                                TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", plr.Index);
                                for (int i3 = 0; i3 < 15; i3++)
                                {
                                    Utils.SpawnProjectile(plr.TPlayer.position.X + 16, plr.TPlayer.position.Y + 16, Utils.Random.Next(-10, 11) / 10f, Utils.Random.Next(-10, 11) / 10f, ProjectileID.LightsBane, R16_Dmg, 2, plr.Index, 2.25f);
                                    await Task.Delay(20);
                                }
                            }
                            await Task.Delay(200);
                            i--;
                        }
                        break; //hyper dash
                    case 17:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Shoop Da Whoop!"), (int)new Color(75, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R17_Duration;
                        while (i > 0 && !plr.Dead)
                        {
                            Utils.SpawnProjectile(plr.X + 16, plr.Y + 13, plr.TPlayer.direction * 20, Utils.Random.Next(-10, 11) / 10f, ProjectileID.LaserMachinegunLaser, R17_Dmg, 6, plr.Index);
                            await Task.Delay(50);
                            i -= 3;
                        }
                        break; //shoop da whoop
                    case 18:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Bullet Hell."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R18_Duration;
                        while (i > 0 && !plr.Dead)
                        {
                            foreach (NPC npc in Main.npc)
                            {
                                if (npc != null && npc.active && npc.type != 0 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(plr.TPlayer.position, 1600))
                                {
                                    Utils.SpawnProjectile(npc.position.X + 16, npc.position.Y, npc.position.DirectionTo(plr.TPlayer.position).X * 12, npc.position.DirectionTo(plr.TPlayer.position).Y * 12, ProjectileID.RuneBlast, R18_Dmg, 0, -1);
                                    await Task.Delay(80);
                                }
                            }
                            await Task.Delay(500);
                            i--;
                        }
                        break; //bullet hell
                    case 19:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Cursed Inferno?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R19_Duration;
                        plr.SetBuff(BuffID.CursedInferno, R19_Duration / 2, true);
                        plr.SetBuff(BuffID.Panic, R19_Duration, true);
                        while (i > 0 && !plr.Dead)
                        {
                            Utils.SpawnProjectile(plr.X + 16, plr.Y + 5, Utils.Random.Next(-100, 101) / 10f, Utils.Random.Next(-100, 101) / 10f, ProjectileID.CursedFlameFriendly, R19_Dmg, 3, plr.Index);
                            await Task.Delay(200);
                            i -= 10;
                        }
                        break; //cursed inferno
                    case 20:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("..."), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        Utils.SpawnProjectile(savedPos.X, savedPos.Y, 0, 0, 950, 0, 8, -1);
                        await Task.Delay(850);
                        Utils.SpawnProjectile(savedPos.X, savedPos.Y, 0, 0, 950, 0, 8, -1);
                        await Task.Delay(850);
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("THE SKELETON APPEARS"), (int)new Color(255, 75, 75).PackedValue, plr.X + 16, plr.Y);
                        int enemyID = Utils.NewNPC((int)savedPos.X - 48, (int)savedPos.Y - 48, 68);
                        Main.npc[enemyID].lifeMax = R20_Health;
                        Main.npc[enemyID].life = R20_Health;
                        Main.npc[enemyID].active = true;
                        Main.npc[enemyID].value = 1f;
                        Main.npc[enemyID].rarity = 11;
                        NetMessage.SendData((int)PacketTypes.NpcUpdate, -1, -1, null, enemyID);
                        break; //THE SKELETON APPEARS
                    case 21:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Bomb Jump?"), (int)new Color(255, 255, 75).PackedValue, plr.X + 16, plr.Y);
                        i = R21_Count;
                        Utils.SpawnProjectile(plr.X + 16, plr.Y, 0, 0, ProjectileID.DD2ExplosiveTrapT3Explosion, R21_Dmg, 9, plr.Index);
                        plr.DamagePlayer(R21_Dmg / 2);
                        plr.TPlayer.velocity = new Vector2(0, -15);
                        TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", plr.Index);
                        await Task.Delay(1000);
                        while (i > 0)
                        {
                            Utils.SpawnProjectile(plr.X + 16, plr.Y + 16, plr.TPlayer.direction * 8, Utils.Random.Next(-60, 61) / 10f, ProjectileID.BouncyGrenade, R21_Dmg, 4, plr.Index);
                            await Task.Delay(80);
                            i--;
                        }
                        break; //bomb jump
                }
            });

        }

        protected override void PlayVisuals(params object[] args)
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
using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Abilities
{
    public class EmpressOfLight : Ability
    {
        int LanceDmg, DashDmg, BoltDmg, BoltSpawnInterval, DanceDmg;

        public EmpressOfLight(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                LanceDmg = (int)((50 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
                DashDmg = (int)((100 + (abilityLevel - 1) * 30) * (1 + abilityLevel / 10f));
                BoltDmg = (int)((10 + (abilityLevel - 1) * 10) * (1 + abilityLevel / 10));
                BoltSpawnInterval = 400 * (10 - abilityLevel) / 10;
                DanceDmg = (int)((55 + (abilityLevel - 1) * 20) * (1 + abilityLevel / 10f));
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);

            if (!Extensions.EmpressCycles.ContainsKey(plr.Name))
            {
                AbilityExtentions.EmpressCycles.Add(plr.Name, 1);
            }

            switch (AbilityExtentions.EmpressCycles[plr.Name])
            {
                case 1:    // Ethereal Lance
                    float startingYPos = plr.Y - 8 * 16;
                    for (int i = 0; i < 19; i += 3)
                    {
                        AbilityExtentions.SpawnProjectile(
                            posX: plr.X + (i % 2 * 60 - 30) * 16,
                            posY: startingYPos + i * 16,
                            speedX: i % 2 * 60 * -1 + 30,
                            speedY: 0,
                            type: ProjectileID.FairyQueenRangedItemShot,
                            damage: LanceDmg,
                            knockback: 5,
                            owner: plr.Index,
                            ai_1: AbilityExtentions.GetNextHallowedWeaponColor()
                            );
                    }
                    break;
                case 2:    // Dash
                    plr.SendData(PacketTypes.PlayerDodge, number: (byte)plr.Index, number2: 2);

                    int direction = AbilityExtentions.GetVelocityXDirection(plr.TPlayer);

                    Task.Run(async () =>
                    {
                        plr.TPlayer.velocity.X = 36 * direction;
                        TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", plr.Index);

                        for (int i = 0; i < 12; i++)
                        {
                            AbilityExtentions.SpawnProjectile(
                                posX: plr.X + 16,
                                posY: plr.Y + 16,
                                speedX: 0,
                                speedY: i % 2 == 0 ? -0.5f : +0.5f,
                                type: ProjectileID.PrincessWeapon,
                                damage: DashDmg,
                                knockback: 14,
                                owner: plr.Index
                                );

                            if (i == 9)
                            {
                                plr.SetBuff(BuffID.Webbed, 1);
                            }

                            await Task.Delay(17);
                        }
                    });
                    break;
                case 3:    // Prismatic Bolts
                    Task.Run(async () =>
                    {
                        int ms = 0;
                        while (ms < 2500)
                        {
                            AbilityExtentions.SpawnProjectile(
                                posX: plr.X + WorldGen.genRand.Next(33),
                                posY: plr.Y + WorldGen.genRand.Next(-8, 33),
                                speedX: WorldGen.genRand.Next(-4, 5),
                                speedY: WorldGen.genRand.Next(-4, 5),
                                type: ProjectileID.FairyQueenMagicItemShot,
                                damage: BoltDmg,
                                knockback: 6,
                                owner: plr.Index,
                                ai_1: AbilityExtentions.GetNextHallowedWeaponColor()
                                );

                            ms += BoltSpawnInterval;
                            await Task.Delay(BoltSpawnInterval);
                        }
                    });
                    break;
                case 4:    // Sundance
                    plr.SetBuff(BuffID.Webbed, 140, true);

                    Task.Run(async () =>
                    {
                        for (int i = 0; i < 3; i++)
                        {    // TODO: Rewrite this without trigonometric functions.
                            for (double j = i % 2 == 0 ? 0.7853 : 1.5707; j < Math.Tau; j += 1.5707)
                            {
                                AbilityExtentions.SpawnProjectile(
                                posX: plr.X + 16 + 64 * (float)Math.Cos(j),
                                posY: plr.Y + 16 + 64 * (float)Math.Sin(j),
                                speedX: 2 * (float)Math.Cos(j),
                                speedY: 2 * (float)Math.Sin(j),
                                type: ProjectileID.PrincessWeapon,
                                damage: DanceDmg,
                                knockback: 5,
                                owner: plr.Index
                                );
                            }
                            await Task.Delay(500);
                        }
                    });
                    break;
            }

            if (AbilityExtentions.EmpressCycles[plr.Name] < 4)
            {
                AbilityExtentions.EmpressCycles[plr.Name]++;
            }
            else
            {
                AbilityExtentions.EmpressCycles[plr.Name] = 1;
            }
        }

        internal override void PlayVisuals(params object[] args) { }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace Abilities
{

    public class Extensions
    {
        internal static Dictionary<byte, DateTime> Cooldowns = new();
        internal static Dictionary<byte, int> CooldownLengths = new();
        internal static Dictionary<string, int> FairyOfLightCycles = new();
        internal static Dictionary<string, int> TwilightCycles = new();
        internal static float HallowedWeaponColor = 0;
        internal static int ExplosiveEffectState = 0;
        internal static Random Random = new Random();

        internal static bool IsPosTeleportable(int x, int y)
        {
            return x + 1 < Main.maxTilesX && y + 2 < Main.maxTilesY &&
                   Main.tile[x, y].collisionType == 0 &&
                   Main.tile[x, y + 1].collisionType == 0 &&
                   Main.tile[x, y + 2].collisionType == 0 &&
                   Main.tile[x + 1, y].collisionType == 0 &&
                   Main.tile[x + 1, y + 1].collisionType == 0 &&
                   Main.tile[x + 1, y + 2].collisionType == 0;
        }

        internal static float GetNextHallowedWeaponColor()
        {
            if (HallowedWeaponColor >= 1)
            {
                HallowedWeaponColor = 0;
            }
            else
            {
                HallowedWeaponColor += 0.015f;
            }
            return HallowedWeaponColor;
        }

        public static int SpawnProjectile(float posX, float posY, float speedX, float speedY, int type,
            int damage, float knockback, int owner = -1, float ai_0 = 0, float ai_1 = 0, float ai_2 = 0)
        {
            int projIndex = Projectile.NewProjectile(
                                spawnSource: Projectile.GetNoneSource(),
                                X: posX,
                                Y: posY,
                                SpeedX: speedX,
                                SpeedY: speedY,
                                Type: type,
                                Damage: damage,
                                KnockBack: knockback,
                                Owner: owner,
                                ai0: ai_0,
                                ai1: ai_1,
                                ai2: ai_2
                            );

            Main.projectile[projIndex].ai[0] = ai_0;
            Main.projectile[projIndex].ai[1] = ai_1;
            Main.projectile[projIndex].ai[2] = ai_2;

            if (owner != -1)
            {
                NetMessage.SendData((int)PacketTypes.ProjectileNew, -1, -1, null, projIndex);
            }

            return projIndex;
        }
        public static int NewNPC(int X, int Y, int Type)
        {
            int index = 200;
            for (int i = 0; i < 200; i++)
            {
                if (!Main.npc[i].active)
                {
                    index = i;
                    break;
                }
            }
            Main.npc[index].SetDefaults(Type);
            Main.npc[index].velocity.X = 0;
            Main.npc[index].velocity.Y = 0;
            Main.npc[index].position.X = X;
            Main.npc[index].position.Y = Y;
            return index;
        }
        public static NPC? GetNearestEnemy(Vector2 pos)
        {
            NPC? nearestEnemy = null;
            foreach (NPC npc in Main.npc)
            {
                if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 && !npc.friendly && !npc.CountsAsACritter)
                {
                    if (nearestEnemy == null) nearestEnemy = npc;
                    if (npc.position.Distance(pos) < nearestEnemy.position.Distance(pos)) nearestEnemy = npc;
                }
            }
            return nearestEnemy;
        }
        public static void ExplosiveEffectEffect(Vector2 pos, int HP)
        {
            if (ExplosiveEffectState <= 0) return;
            Extensions.SpawnProjectile(pos.X + 16, pos.Y, 5, 0, 950, HP, 8, -1);
            Extensions.SpawnProjectile(pos.X + 16, pos.Y, 4, 4, 950, HP, 8, -1);
            Extensions.SpawnProjectile(pos.X + 16, pos.Y, 0, 5, 950, HP, 8, -1);
            Extensions.SpawnProjectile(pos.X + 16, pos.Y, -4, 4, 950, HP, 8, -1);
            Extensions.SpawnProjectile(pos.X + 16, pos.Y, -5, 0, 950, HP, 8, -1);
            Extensions.SpawnProjectile(pos.X + 16, pos.Y, -4, -4, 950, HP, 8, -1);
            Extensions.SpawnProjectile(pos.X + 16, pos.Y, 0, -5, 950, HP, 8, -1);
            Extensions.SpawnProjectile(pos.X + 16, pos.Y, 4, -4, 950, HP, 8, -1);
        }

        public static float GetDistance(float posX1, float posY1, float posX2, float posY2)
        {
            return (float)Math.Sqrt(Math.Pow(posX2 - posX1, 2) + Math.Pow(posY2 - posY1, 2));
        }
        public static float GetDistance(Entity entity1, Entity entity2)
        {
            return GetDistance(entity1.position.X, entity1.position.Y, entity2.position.X, entity2.position.Y);
        }

        public static int GetVelocityXDirection(Player player)
        {
            return (player.velocity.X == 0) ? player.direction : (int)(player.velocity.X / Math.Abs(player.velocity.X));
        }

        /// <summary>
        /// Register this into ServerApi.Hooks.NetSendData in order to have respawn buff timers.
        /// </summary>
        public static void RespawnCooldownBuffAdder(SendDataEventArgs args)
        {

            if (args.MsgId != PacketTypes.PlayerSpawn ||
                !Cooldowns.ContainsKey((byte)args.number) ||
                (DateTime.UtcNow - Cooldowns[(byte)args.number]).TotalSeconds < 0)
            {
                return;
            }

            TShock.Players[(byte)args.number].SetBuff(307, (int)((CooldownLengths[(byte)args.number] - (DateTime.UtcNow - Cooldowns[(byte)args.number]).TotalSeconds) * 60), true);

        }
        
        internal static bool IsInCooldown(byte casterId, int cooldown)
        {
            if (Cooldowns.ContainsKey(casterId))
            {
                if ((DateTime.UtcNow - Cooldowns[casterId]).TotalSeconds < cooldown)
                {
                    return true;
                }

                Cooldowns[casterId] = DateTime.UtcNow;
                CooldownLengths[casterId] = cooldown;
            }
            else
            {
                Cooldowns.Add(casterId, DateTime.UtcNow);
                CooldownLengths.Add(casterId, cooldown);
            }

            TShock.Players[casterId].SetBuff(307, cooldown * 60, true);
            return false;
        }
    }
}

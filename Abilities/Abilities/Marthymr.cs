using Terraria.ID;
using TShockAPI;


namespace Abilities.Abilities
{
    public class Marthymr : Ability
    {
        public float SpeedFactor;
        public int Damage;

        public Marthymr(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                SpeedFactor = 9 + AbilityLevel;
                Damage = 75 + 100 * AbilityLevel;
            };

            UpdateStats();
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            for (double i = 0; i < Math.Tau; i += 0.3926)
            {
                Utils.SpawnProjectile(
                    posX: plr.X + 16 * (float)Math.Cos(i),
                    posY: plr.Y + 16 * (float)Math.Sin(i),
                    speedX: SpeedFactor * (float)Math.Cos(i),
                    speedY: SpeedFactor * (float)Math.Sin(i),
                    type: ProjectileID.DD2SquireSonicBoom,
                    damage: Damage,
                    knockback: 30,
                    owner: -1
                    );
            }

            plr.KillPlayer();
        }

        protected override void PlayVisuals(params object[] args) { }
    }
}
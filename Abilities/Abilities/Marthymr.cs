using Terraria.ID;
using TShockAPI;


namespace Abilities
{
    public class Marthymr : Ability
    {
        private float SpeedFactor;
        private int Damage;

        public Marthymr(int abilityLevel) : base(abilityLevel) { }


        internal override void CalculateProperties()
        {
            SpeedFactor = 9 + AbilityLevel;
            Damage = 75 + 100 * AbilityLevel;
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            for (double i = 0; i < Math.Tau; i += 0.3926)
            {
                Extensions.SpawnProjectile(
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

        internal override void PlayVisuals(params object[] args) { }
    }
}
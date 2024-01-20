using Terraria.ID;
using TShockAPI;


namespace Abilities
{
    public class Marthymr : Ability
    {
        private float SpeedFactor;
        private int Damage;

        public Marthymr(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                SpeedFactor = 9 + abilityLevel;
                Damage = 250 + 100 * abilityLevel;
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);

            for (double i = 0; i < Math.Tau; i += 0.3926)
            {
                AbilityExtentions.SpawnProjectile(
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
        }

        internal override void PlayVisuals(params object[] args) { }
    }
}
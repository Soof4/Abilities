using TShockAPI;
using Terraria;

namespace Abilities
{
    public abstract class Ability
    {
        public delegate void StatsCalculator();

        protected int AbilityLevel = 0;

        /// <summary>
        /// Calculates the ability's stats for it's current level.
        /// </summary>
        public StatsCalculator UpdateStats = () => { };

        protected Ability(int abilityLevel)
        {
            AbilityLevel = abilityLevel;
            UpdateStats();
        }

        /// <summary>
        /// Cast method for dependents to use.
        /// </summary>
        /// <param name="plr">Caster of the ability.</param>
        /// <param name="cooldown">Cooldown of the ability (in seconds).</param>
        /// <param name="abilityLevel">Level of the ability.
        /// <para>
        /// Property calculations are implemented for level 1 to 5 (inclusive).<br></br>
        /// Casting an ability with higher or lower values than [1, 5], can result in buggy behavior.
        /// </para>
        /// </param>
        public void Cast(TSPlayer plr, int cooldown, int? abilityLevel = null)
        {
            if (!Utils.IsInCooldown((byte)plr.Index, cooldown))
            {
                if (abilityLevel != null && abilityLevel != AbilityLevel)
                {
                    AbilityLevel = (int)abilityLevel;
                    UpdateStats();
                }

                Function(plr, cooldown, AbilityLevel);
            }
        }

        /// <summary>
        /// Ability's function.
        /// </summary>
        /// <param name="plr">Caster of the ability.</param>
        /// <param name="cooldown">Cooldown of the ability (in seconds).</param>
        /// <param name="abilityLevel">Level of the ability.
        /// <para>
        /// Property calculations are implemented for level 1 to 5 (inclusive).<br></br>
        /// Casting an ability with higher or lower values than [1, 5], can result in buggy behavior.
        /// </para>
        /// </param>
        protected abstract void Function(TSPlayer plr, int cooldown, int abilityLevel = 1);

        /// <summary>
        /// Ability's visuals.
        /// </summary>
        /// <param name="args">Arguments that might be used for visualization.</param>
        protected abstract void PlayVisuals(params object[] args);

        /// <summary>
        /// Cycle method for dependents to use for cyclable abilities.
        /// </summary>
        /// <param name="plr">The player who wants to cycle their ability.</param>
        /// <para>
        /// This method won't do anything for non-cyclable abilities.
        /// </para>
        protected virtual void CycleLogic(TSPlayer plr) { }

        /// <summary>
        /// Cycle method for dependents to use.
        /// </summary>
        /// <param name="plr">Caster of the ability.</param>
        public void Cycle(TSPlayer plr)
        {
            if (!Utils.IsInCycleCooldown((byte)plr.Index))
            {
                CycleLogic(plr);
            }
        }

        /// <summary>
        /// Upgrades the ability to specified level and calls UpdateStats method.
        /// </summary>
        /// <param name="level">New level of the ability.</param>
        public void Upgrade(int level)
        {
            AbilityLevel = level;
            UpdateStats();
        }
    }
}
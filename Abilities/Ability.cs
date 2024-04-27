using TShockAPI;
using Terraria;

namespace Abilities
{
    public abstract class Ability
    {
        internal protected int AbilityLevel = 0;

        internal protected Ability(int abilityLevel)
        {
            AbilityLevel = abilityLevel;
            CalculateProperties();
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
        public void Cast(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            if (!Extensions.IsInCooldown((byte)plr.Index, cooldown))
            {
                if (abilityLevel != AbilityLevel)
                {
                    AbilityLevel = abilityLevel;
                    CalculateProperties();
                }
                Function(plr, cooldown, abilityLevel);
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
        internal abstract void Function(TSPlayer plr, int cooldown, int abilityLevel = 1);

        /// <summary>
        /// Ability's visuals.
        /// </summary>
        /// <param name="args">Arguments that might be used for visualization.</param>
        internal abstract void PlayVisuals(params object[] args);

        /// <summary>
        /// Ability's property calculations.
        /// <para>
        /// Since instances of abilities are kept and Cast method is called over and over for that instance.<br></br>
        /// It is suggested to check if the abilityLevel argument of Cast method is the same as AbilityLevel of the instance.
        /// </para>
        /// </summary>
        /// <example>
        /// <code>
        /// int abilityLevel = (int)args[0];
        /// if (abilityLevel == AbilityLevel) {
        ///     // modify the properties here
        /// }
        /// </code>
        /// </example>
        /// <param name="args">Arguments that might be used for the calculations.</param>
        internal abstract void CalculateProperties();

        /// <summary>
        /// Cycle method for dependents to use for cyclable abilities.
        /// </summary>
        /// <param name="plr">The player who wants to cycle their ability.</param>
        /// <para>
        /// This method won't do anything for non-cyclable abilities.
        /// </para>
        public virtual void Cycle(TSPlayer plr) {}
    }

}
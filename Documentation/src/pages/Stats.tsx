import CategoryPage from "../components/CategoryPage";
import Navbar from "../components/Navbar";

const Stats = () => {
  return (
    <>
      <Navbar />
      <div className="container min-vh-100">
        <div className="row">
          <CategoryPage />
          <div className="col m-3">
            <h1 className="statsName">Adrenaline</h1>
            <p className="statsDescription">
              Pumps adrenaline into your blood. Increases movement speed and
              damage of the player.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <tr className="statsRow">
                    <th className="statsHeadCell">Propeties</th>
                    <th className="statsHeadCell">Level 1</th>
                    <th className="statsHeadCell">Level 2</th>
                    <th className="statsHeadCell">Level 3</th>
                    <th className="statsHeadCell">Level 4</th>
                    <th className="statsHeadCell">Level 5</th>
                  </tr>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Buffs Duration</td>
                    <td className="statsCell">10.00s</td>
                    <td className="statsCell">11.00s</td>
                    <td className="statsCell">13.00s</td>
                    <td className="statsCell">14.00s</td>
                    <td className="statsCell">16.00s</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">Alchemist</h1>
            <p className="statsDescription">
              Throws different kinds of potions around the player that lasts for
              a few seconds in their location:
              <br />
              1: Heal Potion
              <br />
              2: Harm Potion
              <br />
              3: Vortex Potion
              <br />
              4: Curse Potion
              <br />
              5: Shield Potion
              <br />
              6: Power Potion
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Thrown Potion Types</td>
                    <td className="statsCell">Heal, Damage, Vortex</td>
                    <td className="statsCell">Heal, Damage, Vortex, Curse</td>
                    <td className="statsCell">
                      Heal, Damage, Vortex, Curse, Shield
                    </td>
                    <td className="statsCell">
                      Heal, Damage, Vortex, Curse, Shield, Power
                    </td>
                    <td className="statsCell">
                      Heal, Damage, Vortex, Curse, Shield, Power
                    </td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Potion Size</td>
                    <td className="statsCell">1.5</td>
                    <td className="statsCell">2</td>
                    <td className="statsCell">2.5</td>
                    <td className="statsCell">3</td>
                    <td className="statsCell">3.5</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Potion Lifetime</td>
                    <td className="statsCell">1.4s</td>
                    <td className="statsCell">1.8s</td>
                    <td className="statsCell">2.2s</td>
                    <td className="statsCell">2.6s</td>
                    <td className="statsCell">3s</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Heal Pot. Heal</td>
                    <td className="statsCell">12</td>
                    <td className="statsCell">16</td>
                    <td className="statsCell">20</td>
                    <td className="statsCell">24</td>
                    <td className="statsCell">28</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Damage Pot. Damage</td>
                    <td className="statsCell">24</td>
                    <td className="statsCell">39</td>
                    <td className="statsCell">58</td>
                    <td className="statsCell">81</td>
                    <td className="statsCell">108</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Vortex Pot. Damage</td>
                    <td className="statsCell">12</td>
                    <td className="statsCell">17</td>
                    <td className="statsCell">24</td>
                    <td className="statsCell">31</td>
                    <td className="statsCell">39</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Curse Pot. Debuff Types</td>
                    <td className="statsCell">Cursed Inferno, Ichor</td>
                    <td className="statsCell">Cursed Inferno, Ichor</td>
                    <td className="statsCell">Cursed Inferno, Ichor</td>
                    <td className="statsCell">
                      Cursed Inferno, Ichor, Oiled, Betsy's Curse
                    </td>
                    <td className="statsCell">
                      Cursed Inferno, Ichor, Oiled, Betsy's Curse
                    </td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Curse Pot. Debuffs Duration</td>
                    <td className="statsCell">4.00s</td>
                    <td className="statsCell">5.00s</td>
                    <td className="statsCell">6.00s</td>
                    <td className="statsCell">7.00s</td>
                    <td className="statsCell">8.00s</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">DryadsRingOfHealing</h1>
            <p className="statsDescription">
              Summons a healing circle around the player. The circle heals
              everyone inside it and gives them Dryad's Blessing buff.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Healing Percentage</td>
                    <td className="statsCell">5.00%</td>
                    <td className="statsCell">6.00%</td>
                    <td className="statsCell">7.00%</td>
                    <td className="statsCell">8.00%</td>
                    <td className="statsCell">9.00%</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Buff Duration</td>
                    <td className="statsCell">7.00s</td>
                    <td className="statsCell">7.33s</td>
                    <td className="statsCell">7.67s</td>
                    <td className="statsCell">8.00s</td>
                    <td className="statsCell">8.33s</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">FairyOfLight</h1>
            <p className="statsDescription">
              Cycles through 4 different sub-abilities:
              <br />
              1: Ethereal Lance -&gt; Spawns horizontal ethereal lances near the
              player.
              <br />
              2: Dash -&gt; Dashes towards the direction player is looking at,
              and creates projectiles behind the player which can damage
              enemies.
              <br />
              3: Prismatic Bolts -&gt; Periodically spawns prismatic bolts
              around the player that target the nearby enemies.
              <br />
              4: Sundance -&gt; Webs the player in the location and spawns
              projectiles that resemble Empress of Light's sundance attack.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Ethereal Lance Damage</td>
                    <td className="statsCell">55</td>
                    <td className="statsCell">78</td>
                    <td className="statsCell">104</td>
                    <td className="statsCell">133</td>
                    <td className="statsCell">165</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Dash Damage</td>
                    <td className="statsCell">110</td>
                    <td className="statsCell">156</td>
                    <td className="statsCell">208</td>
                    <td className="statsCell">266</td>
                    <td className="statsCell">330</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Bolt Damage</td>
                    <td className="statsCell">10</td>
                    <td className="statsCell">20</td>
                    <td className="statsCell">30</td>
                    <td className="statsCell">40</td>
                    <td className="statsCell">50</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Bolt Spawn Interval</td>
                    <td className="statsCell">0.36s</td>
                    <td className="statsCell">0.32s</td>
                    <td className="statsCell">0.28s</td>
                    <td className="statsCell">0.24s</td>
                    <td className="statsCell">0.20s</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Sundance Damage</td>
                    <td className="statsCell">60</td>
                    <td className="statsCell">90</td>
                    <td className="statsCell">123</td>
                    <td className="statsCell">161</td>
                    <td className="statsCell">202</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">Harvest</h1>
            <p className="statsDescription">
              Harvest the enemies nearby the player with Reaper's dark magic.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Damage</td>
                    <td className="statsCell">15</td>
                    <td className="statsCell">35</td>
                    <td className="statsCell">68</td>
                    <td className="statsCell">124</td>
                    <td className="statsCell">214</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Spawn Interval</td>
                    <td className="statsCell">0.71s</td>
                    <td className="statsCell">0.58s</td>
                    <td className="statsCell">0.50s</td>
                    <td className="statsCell">0.45s</td>
                    <td className="statsCell">0.41s</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">HyperCrit</h1>
            <p className="statsDescription">This will be written later on.</p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Uses</td>
                    <td className="statsCell">50</td>
                    <td className="statsCell">60</td>
                    <td className="statsCell">70</td>
                    <td className="statsCell">80</td>
                    <td className="statsCell">90</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">IceGolem</h1>
            <p className="statsDescription">This will be written later on.</p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Buffs Duration</td>
                    <td className="statsCell">11.00s</td>
                    <td className="statsCell">13.00s</td>
                    <td className="statsCell">15.00s</td>
                    <td className="statsCell">17.00s</td>
                    <td className="statsCell">19.00s</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">Marthymr</h1>
            <p className="statsDescription">
              Blow yourself up to damage enemies nearby with shockwave.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Damage</td>
                    <td className="statsCell">175</td>
                    <td className="statsCell">275</td>
                    <td className="statsCell">375</td>
                    <td className="statsCell">475</td>
                    <td className="statsCell">575</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Projectile Speed</td>
                    <td className="statsCell">10 mph?</td>
                    <td className="statsCell">11 mph?</td>
                    <td className="statsCell">12 mph?</td>
                    <td className="statsCell">13 mph?</td>
                    <td className="statsCell">14 mph?</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">Paranoia</h1>
            <p className="statsDescription">This will be written later on.</p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Duration</td>
                    <td className="statsCell">5s</td>
                    <td className="statsCell">6s</td>
                    <td className="statsCell">7s</td>
                    <td className="statsCell">8s</td>
                    <td className="statsCell">9s</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Range</td>
                    <td className="statsCell">35 blocks</td>
                    <td className="statsCell">45 blocks</td>
                    <td className="statsCell">55 blocks</td>
                    <td className="statsCell">65 blocks</td>
                    <td className="statsCell">75 blocks</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">Pentagram</h1>
            <p className="statsDescription">This will be written later on.</p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Range</td>
                    <td className="statsCell">20 blocks</td>
                    <td className="statsCell">25 blocks</td>
                    <td className="statsCell">30 blocks</td>
                    <td className="statsCell">35 blocks</td>
                    <td className="statsCell">40 blocks</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">RandomTeleport</h1>
            <p className="statsDescription">
              Opens a teleportation portal to a random location for everyone
              around the caster.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Range</td>
                    <td className="statsCell">60 blocks</td>
                    <td className="statsCell">70 blocks</td>
                    <td className="statsCell">80 blocks</td>
                    <td className="statsCell">90 blocks</td>
                    <td className="statsCell">100 blocks</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">RingOfDracula</h1>
            <p className="statsDescription">
              Summons a circle around the player that chooses the enemy with
              highest hp and steals some health from it.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Range</td>
                    <td className="statsCell">24 blocks</td>
                    <td className="statsCell">24 blocks</td>
                    <td className="statsCell">24 blocks</td>
                    <td className="statsCell">24 blocks</td>
                    <td className="statsCell">24 blocks</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">SetsBlessing</h1>
            <p className="statsDescription">
              Turns you into sand which gives you immunity-frames for some
              amount of time.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Duration</td>
                    <td className="statsCell">3.20s</td>
                    <td className="statsCell">3.40s</td>
                    <td className="statsCell">3.60s</td>
                    <td className="statsCell">3.80s</td>
                    <td className="statsCell">4.00s</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">Shockwave</h1>
            <p className="statsDescription">This will be written later on.</p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Base Damage</td>
                    <td className="statsCell">50</td>
                    <td className="statsCell">84</td>
                    <td className="statsCell">126</td>
                    <td className="statsCell">176</td>
                    <td className="statsCell">234</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Range</td>
                    <td className="statsCell">6.00 blocks</td>
                    <td className="statsCell">6.00 blocks</td>
                    <td className="statsCell">7.00 blocks</td>
                    <td className="statsCell">8.00 blocks</td>
                    <td className="statsCell">8.00 blocks</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Knockback</td>
                    <td className="statsCell">25</td>
                    <td className="statsCell">35</td>
                    <td className="statsCell">45</td>
                    <td className="statsCell">55</td>
                    <td className="statsCell">65</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">SilentOrchestra</h1>
            <p className="statsDescription">This will be written later on.</p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Base Damage</td>
                    <td className="statsCell">7</td>
                    <td className="statsCell">10</td>
                    <td className="statsCell">13</td>
                    <td className="statsCell">16</td>
                    <td className="statsCell">19</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Base Ability Duration</td>
                    <td className="statsCell">20s</td>
                    <td className="statsCell">20s</td>
                    <td className="statsCell">20s</td>
                    <td className="statsCell">20s</td>
                    <td className="statsCell">20s</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">TheBound</h1>
            <p className="statsDescription">
              Bound with the nearest player and get healed every second as long
              as the bound is not broken.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Heal Amount</td>
                    <td className="statsCell">1</td>
                    <td className="statsCell">2</td>
                    <td className="statsCell">3</td>
                    <td className="statsCell">4</td>
                    <td className="statsCell">5</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Max Distance</td>
                    <td className="statsCell">100.00 blocks</td>
                    <td className="statsCell">100.00 blocks</td>
                    <td className="statsCell">100.00 blocks</td>
                    <td className="statsCell">100.00 blocks</td>
                    <td className="statsCell">100.00 blocks</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">Twilight</h1>
            <p className="statsDescription">
              Cycles through 3 different sub-abilities:
              <br />
              1: Brilliant Eyes -&gt; Get vision buffs and spawn projectiles
              around the player that target the nearby enemies.
              <br />
              2: Judgement -&gt; Dazes the player for a bit and attacks enemies
              within a range. If an enemy dies after the attack, they'll spawn a
              projectile which will attack other enemies.
              <br />
              3: Punishment -&gt; Creates an explosion near the player that
              damages enemies nearby and buffs the player for a few seconds.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Brilliant Eyes Damage</td>
                    <td className="statsCell">11</td>
                    <td className="statsCell">18</td>
                    <td className="statsCell">26</td>
                    <td className="statsCell">35</td>
                    <td className="statsCell">45</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Brilliant Eyes Buffs Duration</td>
                    <td className="statsCell">12.00s</td>
                    <td className="statsCell">16.00s</td>
                    <td className="statsCell">20.00s</td>
                    <td className="statsCell">24.00s</td>
                    <td className="statsCell">28.00s</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Brilliant Eyes Count</td>
                    <td className="statsCell">10</td>
                    <td className="statsCell">14</td>
                    <td className="statsCell">18</td>
                    <td className="statsCell">22</td>
                    <td className="statsCell">26</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Judgement Base Damage</td>
                    <td className="statsCell">25</td>
                    <td className="statsCell">50</td>
                    <td className="statsCell">75</td>
                    <td className="statsCell">100</td>
                    <td className="statsCell">125</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Judgement Range</td>
                    <td className="statsCell">50 blocks</td>
                    <td className="statsCell">65 blocks</td>
                    <td className="statsCell">80 blocks</td>
                    <td className="statsCell">95 blocks</td>
                    <td className="statsCell">110 blocks</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Punishment Damage</td>
                    <td className="statsCell">60</td>
                    <td className="statsCell">105</td>
                    <td className="statsCell">160</td>
                    <td className="statsCell">225</td>
                    <td className="statsCell">300</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Punishment Knockback</td>
                    <td className="statsCell">25</td>
                    <td className="statsCell">35</td>
                    <td className="statsCell">45</td>
                    <td className="statsCell">55</td>
                    <td className="statsCell">65</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
            <h1 className="statsName">Witch</h1>
            <p className="statsDescription">
              Curse your enemies with debuffs by summoning an ancient magical
              circle.
            </p>
            <div className="statsTableWrapper">
              <table className="statTable">
                <thead className="statsHead">
                  <th className="statsHeadCell">Propeties</th>
                  <th className="statsHeadCell">Level 1</th>
                  <th className="statsHeadCell">Level 2</th>
                  <th className="statsHeadCell">Level 3</th>
                  <th className="statsHeadCell">Level 4</th>
                  <th className="statsHeadCell">Level 5</th>
                </thead>
                <tbody className="statsBody">
                  <tr className="statsRow">
                    <td className="statsCell">Range</td>
                    <td className="statsCell">16 blocks</td>
                    <td className="statsCell">18 blocks</td>
                    <td className="statsCell">20 blocks</td>
                    <td className="statsCell">22 blocks</td>
                    <td className="statsCell">24 blocks</td>
                  </tr>
                  <tr className="statsRow">
                    <td className="statsCell">Debuffs</td>
                    <td className="statsCell">Frostburn</td>
                    <td className="statsCell">Frostburn, Bleeding</td>
                    <td className="statsCell">ShadowFlame</td>
                    <td className="statsCell">Venom</td>
                    <td className="statsCell">ShadowFlame, Venom</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <br />
          </div>
        </div>
      </div>
    </>
  );
};

export default Stats;

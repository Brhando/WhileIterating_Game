# WhileIterating RPG – Game Design Overview

## Main Loop
Start in base → Explore → Collect resources / Enter combat →  
Upgrade character / Craft items / Upgrade base → Sleep → Repeat

---

## Origins (Weapon-Based Classes)
> “Your class is what you wield, not what you're born into.”

Classes are based on weapon type and evolve into specialized movesets depending on playstyle. These are referred to as **Origins**.

- **Short Sword** → Warrior-style
- **Unfilled Tome** → Mage-style
- **Jack-O-All** → no specialized attacks, but reduced costs

Subclasses will be determined dynamically through tracked behavior over time.

---

## Resources (TBD)
Used to upgrade:
- Equipment
- Base facilities
- Crafting options
- Class-based items/ weapon upgrades
- Class-based plants
- Dungeon Heart
- Items with activations


*Will define once the base system is implemented.*

---

## Stat Tracking – Playstyle-Driven Progression

Stats evolve based on combat behavior and offer moves/upgrades that match player tendencies.

| Stat               | Tracks...                                     |
|--------------------|-----------------------------------------------|
| **Agility**         | Number of dodge moves prepped or executed     |
| **Bravery**         | Number of offensive (attack) moves chosen     |
| **Discipline**      | Number of blocks chosen                       |
| **Insight**         | Number of **charged spells** cast             |
| **Emotional Turmoil** | Number of **non-charged spells** cast     |

> ⚔️ **Note**: Consider tradeoff mechanics between dodging and blocking (e.g., add parry windows, stamina costs, or cooldown variance)

**Some ideas for combat**
- Chained actions gain extra ability: Attack -> attack -> double hit
- Defend -> defend -> parry. 
> needs a fallback (last higher cost? Lock out a turn?)

---

## Races (Simplified, Functional)

| Race     | Trait                                       |
|----------|---------------------------------------------|
| **Velari**   | May perform **1 extra combat action** per encounter |
| **Grathun**  | May use **1 free heal outside of combat** per dungeon |

---

# World Structure

## Overworld Map
- Top-down UI map with **selectable zones**
- **Day Timer** has 6 bars:  
  - Morning  
  - Mid-Morning  
  - Noon  
  - Afternoon  
  - Evening  
  - Night

### Time Costs:
- Traveling to most zones: **3 bars**
- Traveling to dungeon: **6 bars**
- **Non-travel actions** (farming, crafting) before travel cost **0 bars**

> Once all 6 bars are consumed, the player must return to base and sleep.

---

## Zones

Each zone contains:
- 🌿 **Resource Collection Area** (foraging, mining, etc.)
- 🏚️ **Exploration/Story Area** (ruins, shrine, town, etc.)
- 🕳️ **Dungeon** (combat-heavy area with increasing difficulty)

> Resources in a zone do **not replenish** until the zone's dungeon has been cleared.

---

## Dungeon Layout

- Internally loaded as a **separate tilemap scene**
- Semi-random layout including:
  - 2–3 minor encounters
  - 1 final boss fight
- Procedural room order and thematic flavor
- Asset reuse encouraged for development speed

---

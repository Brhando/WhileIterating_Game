# WhileIterating RPG – Game Design Overview

## Main Loop
Start in base → Explore → Collect resources / Enter combat →  
Upgrade character / Craft items / Upgrade base → Sleep → Repeat

---

## Possible Win Conditions:
The *Turning* and *rebirth* are the main functions of the world. It should be the main motivator.
- Lore unlocks with every run and main dungeon being defeated
- The goal becomes clear over time. Win = Uncover the “truth” behind the Turning, or your past lives.
- Win = Beat the final dungeon boss and trigger “The Turning,” which:
- Resets the world with subtle changes (new class unlock, persistent upgrades).
- Progressively reveals deeper lore layers and unlocks “true endings.”

---

## NPC Archetypes & Functions
- Craftsman: Improves weapons, offers rare upgrades if befriended.
- Scout: Reveals new map zones based on their mood/allegiance.
- Healer: Offers temporary buffs or second chances in dungeon runs.
- Lorekeeper: Unlocks past-life memories (turning-related perks).

**Each should have their own personal quests and buffs to the player**

---

## Origins (Weapon-Based Classes)
> “Your class is what you wield, not what you're born into.”

Classes are based on weapon type and evolve into specialized movesets depending on playstyle. These are referred to as **Origins**.

- **Short Sword** → Warrior-style: Berserker and Warder; Introduce *stamina* resource
- **Unfilled Tome** → Mage-style: chained vs planned heavy; Introduce *mana-like* resource
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


Item ideas:
- Food: provides resource buffs (restoration to mana/stamina)
- Herbs/plant concoctions give buffs to fighting (poison/fire resist, chances to trigger events)

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

1. Player Turn:
   → Choose 1–3 Actions (from equipped skill set)
   → Apply Effects (damage, buffs, etc.)
   → Resolve turn

2. Enemy Turn:
   → Each enemy performs their action(s)

3. Reset & Status Tick:
   → Buffs/Debuffs reduce duration
   → Cooldowns tick
   → Resource regeneration (Stamina/Mana)

**Repeat until one side is defeated.**



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
- **Day Timer** has 5 bars:  
  - Morning  
  - Mid-Morning  
  - Noon  
  - Afternoon    
  - Night

### Time Costs:
- Traveling to most zones: **1 bar**
- Traveling to dungeon: **5 bars**
- **Non-travel actions** (farming, crafting) before travel cost **0 bars**

> Once all 5 bars are consumed, the player must return to base and sleep.

---

## Zones

Each zone contains:
- 🌿 **Resource Collection Area** (foraging, mining, etc.)
- 🏚️ **Exploration/Story Area** (ruins, shrine, town, etc.)
- 🕳️ **Dungeon** (combat-heavy area with increasing difficulty)
- **Forest** (exploration and random items with higher danger meter)

> Resources in a zone do **not replenish** until the zone's dungeon has been cleared.

---

## Danger Meter & Prep Choices for Travelling

- increases steadily and resets upon entering combat
- higher meter -> higher chance of combat

Before entering a zone, player can:

- Sneak In (reduce danger, reduce loot)
- Scout Ahead (spend time to remove combat entirely)
- Charge In (increase danger, increase XP/loot if combat triggers)

## Dungeon Layout

- Internally loaded as a **separate tilemap scene**
- Semi-random layout including:
  - 2–3 minor encounters
  - 1 final boss fight
- Procedural room order and thematic flavor
- Asset reuse encouraged for development speed

---

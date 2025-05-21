# TaskList

## Immediate Goals
- [ ] Create WorkBench script and scene
- [ ] Prototype an early "day/night" system
- [ ] Start working on a player's inventory system that persists throughout the game
- [ ] Figure out how to make a player's hp persist throughout the "day"


## To Fix/Add After Vertical Completion
- [ ] Clamp health in both entities (in PlayerEntity and EnemyEntity)
- [ ] Add enemy animations for attacks and damage (in EnemyEntity BattleScene)
- [ ] Replace magic numbers with constants (i.e. add a max health variable, damage variable, etc. to both entities)
- [ ] Add a "Battle Over" state to disable input fully (in BattleScene) and transition to next scene
- [ ] Add `Tome` and its pickup interaction
- [ ] Add SFX to the entire game
- [ ] Add interactions and VFX/SFX for the Bed and Chest

## Completed
|Task                                          |Date                   |
|----------------------------------------------|-----------------------|
| [x] Rebuild `DialogUI` scene cleanly         |20 May, 2025           |
| [x] Test new positioning and visibility      |20 May, 2025           |
| [x] Assign player class on item pickup       |20 May, 2025           |
| [x] Set up Home Base area                    |21 May, 2025           |
| [x] Create Chest script and scene            |21 May, 2025           |
| [x] Create Bed script and Scene              |21 May, 2025           |
| [x] Setup global sigleton ScriptData         |21 May, 2025           |
| [x] Setup and get spawn points working       |21 May, 2025           |
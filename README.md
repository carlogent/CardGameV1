Card Game System Documentation

Keyconcepts:

### Cards
The cards in the game are vessels for card effects, cards themselves dont have much logic beyond holding a list of effects.

### Card Effects
card effects are what drive the logic of the game, these are attached to cards and have a number of qualities which make up their function:
- **Conditional**: this determines the requirements for the the effect to activate. for example a effect that targets a monster in the graveyard must have a monster in the graveyard. also checks if any active effects will prevent it from occuring by creating an attempt event (more on that later)
- **Activate**: this occurs when the conditional is met, a/multiple *Action/s* and a *ActivationEffectData* is created. The Action will set all required events and the ActivationEffectData will store 

### Actions
...

### Events
events are objects that contain information about 1. when an action attempted/activated/resolved by using a sequence number and 2. information about the event itself such as a reference to the Action that created it or anything else. Events are contained within a *DuelEventData*.

Events are used by card effects to determine if something occured to which they can respond to. ex: a card that cares about a monster being destroyed would look in the OnDestroy event lists to check if a monster was just destroyed, also to perhaps check the reference to the monster if needed (perhaps can only activate if a certain type of monster was destroyed).

##### event types:
- **attempt**: this is used by an action when it wants to determine if it can even activate. this is used to determine things like if a card/effect should be highlighted that it can be played/activated. if this is negated then the effect will not even have an option to activate. ex: if an active effect states that no spell cards can be activated for rest of the turn, then that effect would always look in the 'EffectAction' event lists and specifically in the Attempt event list to negate any EffectActions attempts that reference a spell card.
- **activate**: this is used by an action when it successfully 
- **resolved**:

### GameDuelEvents
This is just a list that congregates the three types of an events that an action must have (attempt event/activate event/resolved event) into their own lists.

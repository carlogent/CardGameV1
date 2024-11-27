# Card Game System Documentation

## Keyconcepts

### Cards
The cards in the game are vessels for card effects, cards themselves dont have much logic beyond holding a list of effects.

### Card Effects
card effects are what drive the logic of the game, these are attached to cards and have a number of qualities which make up their function:
- **Conditional()**: this determines the requirements for the the effect to activate. For example, a effect that targets a monster in the graveyard must have a monster in the graveyard. Also checks if any active effects will prevent it from occuring by creating an attempt event (more on that later). Used to determine if a card can activate and thus should be highlighted.
- **Activate()**: this occurs when the conditional is met, *Action(s)* and a *ActivationEffectData* are created. The Action will set all required events and the ActivationEffectData will store the data related to that specific activation and the *SequenceNumber* of the activation as the key for processing later when the *CardEffectActivationChain* is processed.
- **Effect()**: this occurs when the activated effect finally executes when the CardEffectActivationChain is resolving. it creates the desired effect by using the actions which it created in Activate().

### Actions
Actions are the smallest building block of things that can change the game, anything that can be done should be done through an action thus maintaining consistency. Effects can have many actions such as GainLifePoints Action and DestroyCard Action. Actions are the entities that create events, and make it easy and consistent to check if something can be done, and to the effect itself.

- **CanAction()**: checks if the action can be attempted
- **SetEvents()**: sets the events when the effect that creates the action activates.
- **Action()**: creates the effect that the action has, ex: drawing cards, destroying card, etc.

Actions can be modified by other card effects to create dynamic effects, ex: DrawAction can be modified to draw different amounts of cards.

### Events
Events are objects that contain information about when an action attempted/activated/resolved by using a sequence number, and information about the event itself such as a reference to the Action that created it or anything else. Events of a related type are contained within a *DuelEventData*.

Events are used by card effects to determine if something occured to which they can respond to. ex: a card that cares about a monster being destroyed would look in the OnDestroy event lists to check if a monster was just destroyed, also to perhaps check the reference to the monster if needed (perhaps can only activate if a certain type of monster was destroyed).

##### Event types:
- **Attempt**: this is used by an action when it wants to determine if it can even activate. This is used to determine things like if a card/effect should be highlighted to show that it can be played/activated. if this is negated then the effect will not even have an option to activate. ex: if an active effect states that no spell cards can be activated for rest of the turn, then that effect would always look in the 'EffectAction' event lists and specifically in the Attempt event list to negate any EffectActions attempts that reference a spell card.
- **Activate**: this is used by an action when it successfully activates.
- **Resolved**: this is created by an action when it successfully resolves its Action().

### GameDuelEvents
This is just a list that congregates the three types of an events that an action must have (attempt event/activate event/resolved event) into their own lists.

### CardEffectActivationChain, CardEffectActivation and SequenceNumber
The *CardEffectActivationChain* is a list that holds *CardEffectActivation(s)*. the *CardEffectActivation* data type that is created when a card effect successfully activates and it simply holds the *SequenceNumber* associated with the card effect activation aswell as a reference to the card effect. The CardEffect holds the specific activation being processed as the *ActivationEffectData* as mentioned before, this is in a dictionary with the key being the SequenceNumber, thus allowing the card effect to process the correct instance of the activation.

- **SequenceNumber:** a number only used to show when something happened after or before another event. Can be used for multiple things so it is sparce thus cannot be used to determine if something happened right after another.

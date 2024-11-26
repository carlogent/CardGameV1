using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectActivationChain: SingletonBase<CardEffectActivationChain> 
{
    public List<CardEffectActivation> chain;

    public CardEffectActivationChain(){
        chain = new List<CardEffectActivation>();
    }
}


/*

just finished programming and cleaning some more bullshit for my card system, i am once again able to pass turn, 'change turn', and 'draw for turn'. also added new features i guess, forgor most of em probs.
i did kinda "figure out" how to implement refreshing/deleting card effects using rules tho ill likely have to change it later.
also cleaned my project files so it doesnt look like a slum

>current problem: sometimes a card effect may cause multiple actions/events.. currently stuff like the 'RefreshEffectOnTurnChangeEffect' will look at the last event in the TurnChange event list and then compare a sequence number to its sequence number and if its matching then it was the last event but... this can cause these types of effects to miss certain events if they are stacked.. thus.. either i have each effect have one action or check a CardEffectActivation object and search for the required event using casting? (yuck).............. every CardEffectActivation object recorded in the list (chain) has an associated index number, store this effect event index in the event record in the event list. to use to relate it back to last effect...  this feels wrong... we should look at last chain (chainStartSequenceNumber ) and not last effect.. we would have to use chainStartSequenceNumber as the start sequence number when searching for events that "just happened" such as a turn change.... we should have lastEffectSequenceNumber so see which actions relate to which Effect activation in the sequence........

>thus i think i really just need one main ultra-fine-grain "master event record list": which holds meta data about the events of the effect.. effect -> create bunch of events, -> also creates an entry in the master event record list with ICardEffect, List of events in order, starting and ending sequence id, chain id.... [also keep track of things such as **turn** start and end sequence numbers, **chain** start and end sequence numbers ]  



// ********** METHOD 1: keeping everything in objects ************

each event holds: eventSequenceId (index), cardEffectActivationSequenceId (index), chainSequenceId (index), turnSequenceId (index)
- all of which is created in-full when the event is created.

each card-effect-activation holds: chainSequenceId (index), turnSequenceId (index), events
- events added as they are created, such as resolved events.

-> thus we need: 

//this is used to execute the effects
cardEffectActivationChain : chain keeping all the card effects that happen

//this is used to check events (requires casting to check BAD!)
eventChain : chain keeping all the events that happen
chainChain : each chain, keeps the events which occured within it (index)
turnChain : each entry is a list of chains in order that happened in that turn (index)



// ********** METHOD 2: keeping everything in event list and use meta data ************

each event holds: eventSequenceId (index)

//this is used to execute the effects
cardEffectActivationChain : each entry, contains the card effect and the events associated with it. *is this even useful for checking events? maybe only useful for actually efectuing the effects in a chain..

//this is used to check events
eventChain : chain keeping all the events that happen (index is eventSequenceId!)
chainChain : each entry, is the start eventSequenceId of a chain (check next chain to get end eventSequenceId of last chain) [a single effect with no response is still a chain]
turnChain : each entry, is the start eventSequenceId of a turn (check next turn to get end eventSequenceId of last turn)


THE KEY: to have an effect that reacts to an event and is the first to activate and is silent to other effects by not creating events (game mechanic effects)
- Create an effect that triggers when its conditional is met, with a max spell speed such that it executes first, and does not create any events or CardEffectActivation such that other effects can keep responding since the max-spell-speed game mechanic effect is not recorded 
- this requires every effect to properly register itself in the eventChain, and cardEffectActivationChain -- this is error prone and time consuming thus have a bool variable called "registerEffects" in the ICardEffect which if true will register all the events created by the ICardEffect, to do this, have the event seting go through a function wrapper in the base class which checks if "registerEffects" is true before registering the events. 

*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable



/*
* Make it so that there exists a 'game-mechanic-effects' card belonging to neither player, there should be
*   a trigger dictionary (with trigger-type enum as the ID), which keeps registered functions that get called when the trigger is triggered (ex: OnTurnChangeEffect trigger)
*   this cuts-down on the amount duplicated refresh effects that get called for every single card.
*/

//used as a refresh effect
public class OnTurnChangeTriggerEffect: CardEffectBase, ICardEffect 
{
    //Action callback; // this object can only be executed once in a chain!
    int lastSequenceId = 0; //prevents retriggering, must only respond to higher one


    public OnTurnChangeTriggerEffect(ICard? card = null): 
    base(
    _card: card,
    _activationState: ActivationState.ConstantActivated,
    _requirementType: RequirementType.Mandatory,
    _speed: SpellSpeed.none, // ConstantActivated dont have a spell speed since they are already activated
    _dependency: EffectDependency.Managed, 
    _originalActivations: 1, 
    _isAnimated: false)
    {
        //lastSequenceId = GameState.sequenceIdSource; //? crash?
    }

    // Check if a turn changed occured (can be a conditional for activation() or effect() depending on the activationState.. done in the executing functions in gametools)
    public override bool Conditional()
    {
        //check if a change flag is true for the event list first, if not, no new events have been added thus nothing new for us to process 
        if(GameDuelEvents.OnGamePhaseChange.Resolved.isAppended.check == false){ return false; }

        //get the last completed chain, since we are looking for resolved events (resolved events usually only reacted to after the chain)
        ChainSequence targetChain = GameState.chainStartSequenceNumberList[^1];

        //get index of first event to process
        int startIndex = GameDuelEvents.OnGamePhaseChange.Resolved.Count-1;

        while(true)
        {
            if(startIndex < 0){ return false; } // return false if found none

            GamePhaseChangeResolvedEvent duelEvent = GameDuelEvents.OnGamePhaseChange.Resolved[startIndex];

            // if event sequence id is less than last processed event sequenceId, or less than chain starting sequence id return false
            if(duelEvent.eventSequenceId <= lastSequenceId || targetChain.chainSequenceNumberStart > duelEvent.eventSequenceId)
            { 
                return false;
            }

            // if event is within the sequenceId bounds (already checked lower bound)
            if(duelEvent.eventSequenceId <= targetChain.chainSequenceNumberEnd)
            { 
                // if event is changing the correct phases
                if(duelEvent.originalPhase == GamePhase.EndPhase && duelEvent.targetPhase == GamePhase.DrawPhaseStart) 
                { 
                    return true; //this is a turn change
                }
            }
            
            startIndex--;
        }
    }

    //this is how constant activated effects work, they dont activate they just apply their effect.
    public override void Activate(int cardeffectActivationChainIndex)
    {
        //Effect(cardeffectActivationChainIndex);
    }

    public override void Effect(int cardEffectActivationChainIndex)
    {
       //callback();
    }
}




        /*
        // get last resolved chain
        ChainSequence targetChain;
        if(GameState.chainStartSequenceNumberList[^1].chainSequenceNumberEnd != null){ // could be a second responding effect and a new chain may have started thus check for null (new chain) and if so target the chain before that
            targetChain = GameState.chainStartSequenceNumberList[^1];
        }else{
            targetChain = GameState.chainStartSequenceNumberList[^2];
        }*/
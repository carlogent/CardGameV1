using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#nullable enable


// GOLD STANDARD EFFECT, MOST UP TO DATE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//later figure out how to move extra stuff to base class

//this effect requires refreshing at the end of everyturn..
public class DrawForTurnEffect: CardEffectBase
{
    //create a struct that holds the entire data needed for a activation
    public struct DrawForTurnEffectData
    {
        public DrawAction? drawAction;
    }

    // make sure once the activation is executed with effect() it is then deleted from here
    Dictionary<int, DrawForTurnEffectData> activationsData; //the key (int) is the CardEffectActivation index in the CardEffectActivationChain //move to base

    public DrawForTurnEffect(ICard? card = null): base(
    _card: card,  // ! having GameState.defaultCard here causes a stack overflow.. make sure things are initialized properly and in the proper order
    _activationState: ActivationState.Activating,
    _requirementType: RequirementType.Mandatory,
    _speed: SpellSpeed.Speed_1,
    _dependency: EffectDependency.Independant, 
    _originalActivations: 1, 
    _isAnimated: false)
    {
        activationsData = new Dictionary<int, DrawForTurnEffectData>(); //move to base
        GameState.triggers[DuelTriggerType.OnTurnChangeResolved].subscribers += Refresh; // refresh activations available on turn-change
    }

    public override bool Conditional()
    {
        //create temporary actions to check if attempt is possible, will be discarded later by the garbage collector.
        var action = new DrawAction(this, GameState.GenerateUniqueSequenceId(), GameState.GetTurnPlayer(),_numberOfDraws: 1);
        if(action.CanAction() && activationsRemaining > 0 && GameState.gamePhase == GamePhase.DrawPhase){ 
            // we can clean up the events from the attempt pools here if nessessary 
            return true;
        }
        
        return false;
    }

    // this gets executed when the effect is activated
    public override void Activate(int cardEffectActivationChainIndex){

        //use actions
        var drawAction = new DrawAction(this, GameState.GenerateUniqueSequenceId(), GameState.GetTurnPlayer(),_numberOfDraws: 1);
        drawAction.SetEvents();
        activationsRemaining -= 1;

        //add the actions to its own activation data, allowing for multiple activations
        activationsData.Add(cardEffectActivationChainIndex, new DrawForTurnEffectData{
            drawAction = drawAction
        });
    }

    // this gets executed when the effect is applied
    public override void Effect(int cardEffectActivationChainIndex)
    {
        var activationData = activationsData[cardEffectActivationChainIndex];
        
        if(activationData.drawAction != null){
            activationData.drawAction.Action();
        }else{
            throw new Exception("class DrawForTurnEffect - the action of the effect is null, this should never happen, did you forget to activate the effect before executing it?");
        }

    }
}


/*
        // creates attempt version of the events 
        // since attempt events only gets processed by constant-activated-effects;
        // > create the activation actions
        // > action.SetAttemptEvents()
        // > let effects process the attempt events (may need to filter for type.ConstantActivatedEffect)
        // > check the attempt events for any negates then return false, thus the effect cannot be activated.
        //this is all done within the attempt function
*/


/* ONCE PER TURN
    //.. check if newest activationsData key (sequenceId) is within the bounds of the chain newest.. (should only happen once per turn)
    if(activationsData.Keys.Last() > GameState.turnStartSequenceNumberList[^1]){
        //if the sequence number of the last effect activation is greater then the sequence number of when the turn changed, that means this effect activated this turn.
        return false;
    }
*/
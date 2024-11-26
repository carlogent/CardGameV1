using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#nullable enable


// GOLD STANDARD EFFECT, MOST UP TO DATE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//later figure out how to move extra stuff to base class

//this effect requires refreshing at the end of everyturn..
public class DrawPhaseStartToDrawPhaseEffect: CardEffectBase
{
    //create a struct that holds the entire data needed for a activation
    public struct DrawPhaseStartToDrawPhaseEffectData
    {
        //public DrawAction? drawAction;
        public GamePhase currentGamePhase;
        public GamePhase nextGamePhase;
        public ChangeGamePhaseAction? advanceGamePhaseAction;
    }

    // make sure once the activation is executed with effect() it is then deleted from here
    SortedDictionary<int, DrawPhaseStartToDrawPhaseEffectData> activationsData; //the key (int) is the CardEffectActivation index in the CardEffectActivationChain //move to base.. or declare in interface

    public DrawPhaseStartToDrawPhaseEffect(ICard? card = null): base(
    _card: card,  // ! having GameState.defaultCard here causes a stack overflow.. make sure things are initialized properly and in the proper order
    _activationState: ActivationState.Activating, //activates instead of just effect
    _requirementType: RequirementType.Mandatory,
    _speed: SpellSpeed.Speed_1,
    _dependency: EffectDependency.Independant, 
    _originalActivations: 1, 
    _isAnimated: false)
    {
        activationsData = new SortedDictionary<int, DrawPhaseStartToDrawPhaseEffectData>(); //move to base
        GameState.triggers[DuelTriggerType.OnTurnChangeResolved].subscribers += Refresh; // refresh activations available on turn-change
    }

    //in these type of effects, should check the actiate conditional too even in the chainNextEffect
    public override bool Conditional()
    {
        if(GameState.GetGamePhase() != GamePhase.DrawPhaseStart || 
        GameState.nextEffect == this || // should be checked before calling ActivateConditional()
        activationsRemaining < 1 || // should be check before calling ActivateConditional()
        activationsData.Keys.Last() > GameState.turnStartSequenceNumberList[^1]) // if the sequence number of the last effect activation is greater then the sequence number of when the turn changed, that means this effect activated this turn.
        {
            return false;
        }

        // create temporary actions to check if attempt is possible, will be discarded later by the garbage collector.
        ChangeGamePhaseAction advanceGamePhaseAction = new ChangeGamePhaseAction(this, GameState.GenerateUniqueSequenceId(), GameState.GetGamePhase(), EnumExtensions.GetNextEnum(GameState.GetGamePhase()) );

        if(advanceGamePhaseAction.CanAction()){
            return true;
        }
        else{
            return false;
        }
    }

    // this gets executed when the effect is activated
    public override void Activate(int cardEffectActivationChainIndex){

        //use actions
        ChangeGamePhaseAction advanceGamePhaseAction = new ChangeGamePhaseAction(this, GameState.GenerateUniqueSequenceId(), GameState.GetGamePhase(), EnumExtensions.GetNextEnum(GameState.GetGamePhase()) );

        advanceGamePhaseAction.SetEvents();
        activationsRemaining -= 1;

        //add the actions to its own activation data, allowing for multiple activations
        activationsData.Add(cardEffectActivationChainIndex, new DrawPhaseStartToDrawPhaseEffectData(){
            advanceGamePhaseAction = advanceGamePhaseAction
        });
    }

    // this gets executed when the effect is applied
    public override void Effect(int cardEffectActivationChainIndex)
    {
        var activationData = activationsData[cardEffectActivationChainIndex];
        
        
        if(activationData.advanceGamePhaseAction != null){
            //register next effect
            GameState.nextEffect = GameState.defaultCard.effects[typeof(DrawPhaseToMainPhaseEffect)];

            activationData.advanceGamePhaseAction.Action();
        }else{
            throw new Exception("class DrawPhaseStartToDrawPhaseEffect - the action of the effect is null, this should never happen, did you forget to activate the effect before executing it?");
        }/**/

    }
}

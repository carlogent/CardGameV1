using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

/*
public class AdvanceGamePhaseEffect : CardEffectBase, ICardEffect // !!!!!!!!!!!!!!!!!!    CREATE THE CONCEPT OF RULE CARDS ex: add enum type CardType.Rule, giving certain privlages, maybe instead of liveCards put them into a new ruleCards list
    {
        GamePhaseChangeActivateEvent gamePhaseChangeActivateEvent;
        GamePhase nextGamePhase;
        public AdvanceGamePhaseEffect() : base(_card: null, _originalActivationsAmount: 999)
        {}

        public override bool _Conditional() 
        {
            if(CardTools.IsNegated(this)){
                return false;
            }
            return true;
        }

        public override void _SetActivateEvents(){
            // build the event data, add event to appropriate event-list, check for cards that respond to the events
            nextGamePhase = EnumExtensions.GetNextEnum(GameState.GetGamePhase());

            // build the event data, add event to appropriate event-list, check for cards that respond to the events [announcing that we are attempting to apply the effects: activating a gamephase change event]
            gamePhaseChangeActivateEvent = new GamePhaseChangeActivateEvent(this, GameState.GenerateEventSequenceId(), nextGamePhase); //create event
            GameDuelEvents.OnGamePhaseChange.AddActivateEvents( new List<GamePhaseChangeActivateEvent>(){gamePhaseChangeActivateEvent});// responding effects can also modify the event, to modify its execution, feeding single event as list.. must do all at once
        }

        public override void _Effect()
        {
            Debug.Log("AdvanceGamePhaseEffect ExecuteEffect");
            // actually apply the effects (a GamePhaseChangeActivateEvent cannot be modified and only negated so no need to check the GamePhaseChangeActivateEvent object for modifications to relavant variables)
            if(!CardTools.IsNegated(this)){
                GameState.SetGamePhase(gamePhaseChangeActivateEvent);

                // signal that the gamephase change is resolved
                GamePhaseChangeResolvedEvent gamePhaseChangeResolvedEvent = new GamePhaseChangeResolvedEvent(this, GameState.GenerateEventSequenceId(), gamePhaseChangeActivateEvent); //create resolved event
                Debug.Log("Adding the resolved gamechange event.................... +" + gamePhaseChangeActivateEvent.gamePhase);
                GameDuelEvents.OnGamePhaseChange.AddResolvedEvents(new List<GamePhaseChangeResolvedEvent>(){gamePhaseChangeResolvedEvent});// event ->  attempt to change gamephase completed

                // if the turn changed, also process the turn change actions
                if(GameState.gamePhase == GamePhase.DrawPhaseStart){
                    GameTools.TurnChangeActions(GameState.GetTurnPlayer());
                }

                //GamePhaseManager.ProcessGamePhase(); //does this let things return? it does not! it is executing another effect!! VERY WRONG
            }
        }
    }*/


//usually not linked to a card, origin of this effect should be the player clicking next phase button -- also handles changing the turn
public class AdvanceGamePhaseEffect: CardEffectBase
{


    //create a struct that holds the entire data needed for a activation
    public struct AdvanceGamePhaseEffectData
    {
        public GamePhase currentGamePhase;
        public GamePhase nextGamePhase;
        public ChangeGamePhaseAction? advanceGamePhaseAction;
    }


    // make sure once the activation is executed with effect() it is then deleted from here
    Dictionary<int, AdvanceGamePhaseEffectData> activationsData; //the key (int) is the CardEffectActivation index in the CardEffectActivationChain //move to base
    

    public AdvanceGamePhaseEffect(ICard? card = null): 
    base(
        _card: card, 
        _activationState: ActivationState.Activating,
        _requirementType: RequirementType.Optional,
        _speed: SpellSpeed.Speed_1,
        _dependency: EffectDependency.Independant, 
        _originalActivations: 9999999, 
        _isAnimated: false)
    {
        activationsData = new Dictionary<int, AdvanceGamePhaseEffectData>();
        Debug.Log("AdvanceGamePhaseEffect");
        //SetRefreshEffect(new RefreshOnTurnChangeEffect(GameState.defaultCard, Refresh));
    }

    public override bool Conditional()
    {
        //anything in the state that says we cant do it?

        //temp actoion for testing
        ChangeGamePhaseAction advanceGamePhaseAction = new ChangeGamePhaseAction(this, GameState.GenerateUniqueSequenceId(), GameState.GetGamePhase(), EnumExtensions.GetNextEnum(GameState.GetGamePhase()) );

        if(advanceGamePhaseAction.CanAction()){
            return true;
        }
        return false;
    }

    public override void Activate(int cardeffectActivationChainIndex) //give chance to register events before execution 
    {
        // build the event data, add event to appropriate event-list, check for cards that respond to the events
        AdvanceGamePhaseEffectData activation = new AdvanceGamePhaseEffectData
        {
            currentGamePhase = GameState.GetGamePhase(),
            nextGamePhase = EnumExtensions.GetNextEnum(GameState.GetGamePhase())
        };

        //use actions
        activation.advanceGamePhaseAction = new ChangeGamePhaseAction(this, GameState.GenerateUniqueSequenceId(), activation.currentGamePhase, activation.nextGamePhase);
        activation.advanceGamePhaseAction.SetEvents();

        activationsData.Add(cardeffectActivationChainIndex, activation);
    }

    public override void Effect(int cardeffectActivationChainIndex)
    {
        AdvanceGamePhaseEffectData activation =  activationsData[cardeffectActivationChainIndex];

        if(activation.advanceGamePhaseAction != null){
            //Debug.Log("AdvanceGamePhaseEffect executing! entering:" + action.gamePhase);
            activation.advanceGamePhaseAction.Action();
        }else{
            throw new Exception("class AdvanceGamePhaseEffect (advance game phase action) - the action of the effect is null, this should never happen, did you forget to activate the effect before executing it?");
        }


        //GameTools.EvaluateCardsConditionals();
    }

    
}



